using Dapper;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data.Helpers.Interfaces;
using RaceBoard.Data.Repositories.Base.Abstract;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;
using static RaceBoard.Data.Helpers.SqlQueryBuilder;

namespace RaceBoard.Data.Repositories
{
    public class BoatRepository : AbstractRepository, IBoatRepository
    {
        #region Private Members

        private readonly Dictionary<string, string> _columnsMapping = new()
        {
            { "Id", "[Boat].Id" },
            { "Name", "[Boat].Name" },
            { "SailNumber", "[Boat].SailNumber"},
            { "HullNumber", "[Boat].HullNumber"},
            { "RaceClass.Id", "[RaceClass].Id" },
            { "RaceClass.Name", "[RaceClass].Name" },
            { "RaceCategory.Id", "[RaceCategory].Id" },
            { "RaceCategory.Name", "[RaceCategory].Name" }
        };

        #endregion

        #region Constructors

        public BoatRepository(IContextResolver contextResolver, IQueryBuilder queryBuilder) : base(contextResolver, queryBuilder)
        {
        }

        #endregion

        #region IBoatRepository implementation

        public ITransactionalContext GetTransactionalContext(TransactionContextScope scope = TransactionContextScope.Internal)
        {
            return base.GetTransactionalContext(scope);
        }

        public void ConfirmTransactionalContext(ITransactionalContext context)
        {
            base.ConfirmTransactionalContext(context);
        }

        public void CancelTransactionalContext(ITransactionalContext context)
        {
            base.CancelTransactionalContext(context);
        }

        public bool Exists(int id, ITransactionalContext? context = null)
        {
            return base.Exists(id, "Boat", "Id", context);
        }

        public bool ExistsDuplicate(Boat boat, ITransactionalContext? context = null)
        {
            string condition = "[IdRaceClass] = @idRaceClass AND [SailNumber] = @sailNumber AND [HullNumber] = @hullNumber";

            string existsQuery = base.GetExistsDuplicateQuery("[Boat]", condition, "Id", "@id");

            QueryBuilder.AddCommand(existsQuery);
            QueryBuilder.AddParameter("idRaceClass", boat.RaceClass.Id);
            QueryBuilder.AddParameter("sailNumber", boat.SailNumber);
            QueryBuilder.AddParameter("hullNumber", boat.HullNumber);
            QueryBuilder.AddParameter("id", boat.Id);

            return base.Execute<bool>(context);
        }

        public PaginatedResult<Boat> Search(string searchTerm, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            return this.SearchBoats(searchTerm, paginationFilter, sorting, context);
        }

        public PaginatedResult<Boat> Get(BoatSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            return this.GetBoats(searchFilter, paginationFilter, sorting, context);
        }

        public void Create(Boat boat, ITransactionalContext? context = null)
        {
            this.CreateBoat(boat, context);
        }

        public void Update(Boat boat, ITransactionalContext? context = null)
        {
            this.UpdateBoat(boat, context);
        }

        public int Delete(int id, ITransactionalContext? context = null)
        {
            return base.Delete("[Boat]", id, "Id", context);
        }

        #endregion

        #region Private Methods

        private PaginatedResult<Boat> SearchBoats(string searchTerm, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            string sql = $@"SELECT
	                            [Boat].Id [Id],	                            
	                            [Boat].Name [Name],
                                [Boat].SailNumber [SailNumber],
                                [Boat].HullNumber [HullNumber],
                                [RaceClass].Id [Id],
                                [RaceClass].Name [Name]
                            FROM [Boat] [Boat]
                            INNER JOIN [RaceClass] [RaceClass] ON [RaceClass].Id = [Boat].IdRaceClass";

            QueryBuilder.AddCommand(sql);
            QueryBuilder.AddCondition("[Boat].Name LIKE '%' + @searchTerm + '%'", LogicalOperator.Or);
            QueryBuilder.AddCondition("[Boat].SailNumber LIKE '%' + @searchTerm + '%'", LogicalOperator.Or);
            QueryBuilder.AddCondition("[Boat].HullNumber LIKE '%' + @searchTerm + '%'", LogicalOperator.Or);
            QueryBuilder.AddParameter("searchTerm", searchTerm);
            QueryBuilder.AddSorting(sorting, _columnsMapping);
            QueryBuilder.AddPagination(paginationFilter);

            var boats = new List<Boat>();

            PaginatedResult<Boat> items = base.GetPaginatedResults<Boat>
                (
                    (reader) =>
                    {
                        return reader.Read<Boat, RaceClass, Boat>
                        (
                            (boat, raceClass) =>
                            {
                                boat.RaceClass = raceClass;

                                boats.Add(boat);

                                return boat;
                            },
                            splitOn: "Id, Id"
                        ).AsList();
                    },
                    context
                );

            items.Results = boats;

            return items;
        }

        private PaginatedResult<Boat> GetBoats(BoatSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            string sql = $@"SELECT
                                [Boat].Id [Id],
                                [Boat].Name [Name],
                                [Boat].SailNumber [SailNumber],
                                [Boat].HullNumber [HullNumber],
                                [RaceClass].Id [Id],
                                [RaceClass].Name [Name],
                                [RaceCategory].Id [Id],
                                [RaceCategory].Name [Name]
                            FROM [Boat] [Boat]
                            INNER JOIN [RaceClass] [RaceClass] ON [RaceClass].Id = [Boat].IdRaceClass
                            INNER JOIN [RaceCategory] [RaceCategory] ON [RaceCategory].Id = [RaceClass].IdRaceCategory";

            QueryBuilder.AddCommand(sql);

            ProcessSearchFilter(searchFilter);

            QueryBuilder.AddSorting(sorting, _columnsMapping);
            QueryBuilder.AddPagination(paginationFilter);

            var boats = new List<Boat>();

            PaginatedResult<Boat> items = base.GetPaginatedResults<Boat>
                (
                    (reader) =>
                    {
                        return reader.Read<Boat, RaceClass, RaceCategory, Boat>
                        (
                            (boat, raceClass, raceCategory) =>
                            {
                                raceClass.RaceCategory = raceCategory;
                                boat.RaceClass = raceClass;

                                boats.Add(boat);

                                return boat;
                            },
                            splitOn: "Id, Id, Id"
                        ).AsList();
                    },
                    context
                );

            items.Results = boats;

            return items;
        }

        private void ProcessSearchFilter(BoatSearchFilter? searchFilter = null)
        {
            if (searchFilter == null)
                return;

            base.AddFilterCriteria(ConditionType.In, "[Boat]", "Id", "ids", searchFilter.Ids);
            base.AddFilterCriteria(ConditionType.Equal, "[RaceClass]", "Id", "idRaceClass", searchFilter.RaceClass?.Id);
            base.AddFilterCriteria(ConditionType.Equal, "[RaceCategory]", "IdRaceCategory", "idRaceCategory", searchFilter.RaceCategory?.Id);
            base.AddFilterCriteria(ConditionType.Like, "[Boat]", "Name", "name", searchFilter.Name);
            base.AddFilterCriteria(ConditionType.Like, "[Boat]", "SailNumber", "sailNumber", searchFilter.SailNumber);
            base.AddFilterCriteria(ConditionType.Like, "[Boat]", "HullNumber", "hullNumber", searchFilter.HullNumber);
        }

        private void CreateBoat(Boat boat, ITransactionalContext? context = null)
        {
            string sql = @" INSERT INTO [Boat]
                                ( IdRaceClass, Name, SailNumber, HullNumber )
                            VALUES
                                ( @idRaceClass, @name, @sailNumber, @hullNumber )";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("idRaceClass", boat.RaceClass.Id);
            QueryBuilder.AddParameter("name", boat.Name);
            QueryBuilder.AddParameter("sailNumber", boat.SailNumber);
            QueryBuilder.AddParameter("hullNumber", boat.HullNumber);

            QueryBuilder.AddReturnLastInsertedId();

            boat.Id = base.Execute<int>(context);
        }

        private void UpdateBoat(Boat boat, ITransactionalContext? context = null)
        {
            string sql = @" UPDATE [Boat] SET
                                Name = @name,
                                SailNumber = @sailNumber
                                HullNumber = @hullNumber";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("name", boat.Name);
            QueryBuilder.AddParameter("sailNumber", boat.SailNumber);
            QueryBuilder.AddParameter("hullNumber", boat.HullNumber);

            QueryBuilder.AddParameter("id", boat.Id);
            QueryBuilder.AddCondition("Id = @id");

            base.ExecuteAndGetRowsAffected(context);
        }

        #endregion
    }
}
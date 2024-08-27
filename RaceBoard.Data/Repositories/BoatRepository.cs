using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data.Helpers.Interfaces;
using RaceBoard.Data.Repositories.Base.Abstract;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;

namespace RaceBoard.Data.Repositories
{
    public class BoatRepository : AbstractRepository, IBoatRepository
    {
        #region Private Members

        private readonly Dictionary<string, string> _columnsMapping = new()
        {
            { "Id", "[Boat].Id" },
            { "Name", "[Boat].Name" },
            { "SailNumber", "[Boat].SailNumber"}
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
            string existsQuery = base.GetExistsQuery("[Boat]", "[Id] = @id");

            QueryBuilder.AddCommand(existsQuery);
            QueryBuilder.AddParameter("id", id);

            return base.Execute<bool>(context);
        }

        public bool ExistsDuplicate(Boat boat, ITransactionalContext? context = null)
        {
            string existsQuery = base.GetExistsDuplicateQuery("[Boat]", "[SailNumber] = @sailNumber", "Id", "@id");

            QueryBuilder.AddCommand(existsQuery);
            QueryBuilder.AddParameter("sailNumber", boat.SailNumber);
            QueryBuilder.AddParameter("id", boat.Id);

            return base.Execute<bool>(context);
        }

        public PaginatedResult<Boat> Get(BoatSearchFilter searchFilter, PaginationFilter paginationFilter, Sorting sorting, ITransactionalContext? context = null)
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

        private PaginatedResult<Boat> GetBoats(BoatSearchFilter searchFilter, PaginationFilter paginationFilter, Sorting sorting, ITransactionalContext? context = null)
        {
            string sql = $@"SELECT
                                [Boat].Id [Id],
                                [Boat].Name [Name],
                                [Boat].SailNumber [SailNumber]
                            FROM [Boat] [Boat]";

            QueryBuilder.AddCommand(sql);

            ProcessSearchFilter(searchFilter);

            QueryBuilder.AddSorting(sorting, _columnsMapping);
            QueryBuilder.AddPagination(paginationFilter);

            return base.GetMultipleResultsWithPagination<Boat>(context);
        }

        private void ProcessSearchFilter(BoatSearchFilter searchFilter)
        {
            if (searchFilter.Ids != null && searchFilter.Ids.Length > 0)
            {
                QueryBuilder.AddCondition($"[Boat].Id IN @ids");
                QueryBuilder.AddParameter("ids", searchFilter.Ids);
            }

            if (!string.IsNullOrEmpty(searchFilter.Name))
            {
                QueryBuilder.AddCondition($"[Boat].Name LIKE {AddLikeWildcards("@name")}");
                QueryBuilder.AddParameter("name", searchFilter.Name);
            }

            if (!string.IsNullOrEmpty(searchFilter.SailNumber))
            {
                QueryBuilder.AddCondition($"[Boat].SailNumber LIKE {AddLikeWildcards("@sailNumber")}");
                QueryBuilder.AddParameter("sailNumber", searchFilter.SailNumber);
            }
        }

        private void CreateBoat(Boat boat, ITransactionalContext? context = null)
        {
            string sql = @" INSERT INTO [Boat]
                                ( Name, SailNumber )
                            VALUES
                                ( @name, @sailNumber )";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("name", boat.Name);
            QueryBuilder.AddParameter("sailNumber", boat.SailNumber);

            QueryBuilder.AddReturnLastInsertedId();

            boat.Id = base.Execute<int>(context);
        }

        private void UpdateBoat(Boat boat, ITransactionalContext? context = null)
        {
            string sql = @" UPDATE [Boat] SET
                                Name = @name,
                                SailNumber = @sailNumber";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("name", boat.Name);
            QueryBuilder.AddParameter("sailNumber", boat.SailNumber);

            QueryBuilder.AddParameter("id", boat.Id);
            QueryBuilder.AddCondition("Id = @id");

            base.ExecuteAndGetRowsAffected(context);
        }

        #endregion
    }
}
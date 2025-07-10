using Dapper;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data.Helpers.Interfaces;
using RaceBoard.Data.Repositories.Base.Abstract;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;

namespace RaceBoard.Data.Repositories
{
    public class BoatOrganizationRepository : AbstractRepository, IBoatOrganizationRepository
    {
        #region Private Members

        private readonly Dictionary<string, string> _columnsMapping = new()
        {
            { "Id", "[Boat_Organization].Id" },
            { "Organization.Id", "[Organization].Id" },
            { "Organization.Name", "[Organization].Name" },
            { "Boat.Id", "[Boat].Id" },
            { "Boat.Name", "[Boat].Name" },
            { "Boat.SailNumber", "[Boat].SailNumber"},
            { "Boat.HullNumber", "[Boat].HullNumber"},
            { "Boat.RaceClass.Id", "[RaceClass].Id" },
            { "Boat.RaceClass.Name", "[RaceClass].Name" },
            { "Boat.RaceClass.RaceCategory.Id", "[RaceCategory].Id" },
            { "Boat.RaceClass.RaceCategory.Name", "[RaceCategory].Name" }
        };

        #endregion

        #region Constructors

        public BoatOrganizationRepository(IContextResolver contextResolver, IQueryBuilder queryBuilder) : base(contextResolver, queryBuilder)
        {
        }

        #endregion

        #region IBoatOrganizationRepository implementation

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
            return base.Exists(id, "Boat_Organization", "Id", context);
        }

        public bool ExistsDuplicate(BoatOrganization boatOrganization, ITransactionalContext? context = null)
        {
            string condition = "[IdBoat] = @idBoat AND [IdOrganization] = @idOrganization AND [StartDate] = @startDate";

            string existsQuery = base.GetExistsDuplicateQuery("[Boat_Organization]", condition, "Id", "@id");

            QueryBuilder.AddCommand(existsQuery);
            QueryBuilder.AddParameter("idBoat", boatOrganization.Boat.Id);
            QueryBuilder.AddParameter("idOrganization", boatOrganization.Organization.Id);
            QueryBuilder.AddParameter("startDate", boatOrganization.StartDate);
            QueryBuilder.AddParameter("id", boatOrganization.Id);

            return base.Execute<bool>(context);
        }

        public PaginatedResult<BoatOrganization> Get(BoatOrganizationSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            return this.GetBoatOrganizations(searchFilter, paginationFilter, sorting, context);
        }

        public void Create(BoatOrganization boatOrganization, ITransactionalContext? context = null)
        {
            this.CreateBoatOrganization(boatOrganization, context);
        }

        public void Update(BoatOrganization boatOrganization, ITransactionalContext? context = null)
        {
            this.UpdateBoatOrganization(boatOrganization, context);
        }

        public int Delete(int id, ITransactionalContext? context = null)
        {
            return base.Delete("[Boat_Organization]", id, "Id", context);
        }

        #endregion

        #region Private Methods

        private PaginatedResult<BoatOrganization> GetBoatOrganizations(BoatOrganizationSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            string sql = $@"SELECT
                                [Boat_Organization].Id [Id],
                                [Boat_Organization].StartDate [StartDate],
                                [Boat_Organization].EndDate [EndDate],
                                [Organization].Id [Id],
                                [Organization].Name [Name],
                                [Boat].Id [Id],
                                [Boat].Name [Name],
                                [Boat].SailNumber [SailNumber],
                                [Boat].HullNumber [HullNumber],
                                [RaceClass].Id [Id],
                                [RaceClass].Name [Name],
                                [RaceCategory].Id [Id],
                                [RaceCategory].Name [Name]
                            FROM [Boat_Organization] [Boat_Organization]
                            INNER JOIN [Boat] [Boat] ON [Boat].Id = [Boat_Organization].IdBoat
                            INNER JOIN [Organization] [Organization] ON [Organization].Id = [Boat_Organization].IdOrganization
                            INNER JOIN [RaceClass] [RaceClass] ON [RaceClass].Id = [Boat].IdRaceClass
                            INNER JOIN [RaceCategory] [RaceCategory] ON [RaceCategory].Id = [RaceClass].IdRaceCategory";

            QueryBuilder.AddCommand(sql);

            ProcessSearchFilter(searchFilter);

            QueryBuilder.AddSorting(sorting, _columnsMapping);
            QueryBuilder.AddPagination(paginationFilter);

            var boatOrganizations = new List<BoatOrganization>();

            PaginatedResult<BoatOrganization> items = base.GetPaginatedResults<BoatOrganization>
                (
                    (reader) =>
                    {
                        return reader.Read<BoatOrganization, Organization, Boat, RaceClass, RaceCategory, BoatOrganization>
                        (
                            (boatOrganization, organization, boat, raceClass, raceCategory) =>
                            {
                                boatOrganization.Boat = boat;
                                boatOrganization.Organization = organization;

                                raceClass.RaceCategory = raceCategory;
                                boat.RaceClass = raceClass;

                                boatOrganizations.Add(boatOrganization);

                                return boatOrganization;
                            },
                            splitOn: "Id, Id, Id, Id, Id"
                        ).AsList();
                    },
                    context
                );

            items.Results = boatOrganizations;

            return items;
        }

        private void ProcessSearchFilter(BoatOrganizationSearchFilter? searchFilter = null)
        {
            if (searchFilter == null)
                return;

            base.AddFilterCriteria(ConditionType.In, "[Boat_Organization]", "Id", "ids", searchFilter.Ids);
            base.AddFilterCriteria(ConditionType.GreaterOrEqualThan, "[Boat_Organization]", "StartDate", "startDate", searchFilter.StartDate);
            base.AddFilterCriteria(ConditionType.LessOrEqualThan, "[Boat_Organization]", "EndDate", "endDate", searchFilter.EndDate);
            base.AddFilterCriteria(ConditionType.Equal, "[Boat]", "Id", "idBoat", searchFilter.Boat?.Id);
            base.AddFilterCriteria(ConditionType.Equal, "[Organization]", "Id", "idOrganization", searchFilter.Organization?.Id);
        }

        private void CreateBoatOrganization(BoatOrganization boatOrganization, ITransactionalContext? context = null)
        {
            string sql = @" INSERT INTO [Boat_Organization]
                                ( IdBoat, IdOrganization, StartDate, EndDate )
                            VALUES
                                ( @idBoat, @idOrganization, @startDate, @endDate )";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("idBoat", boatOrganization.Boat.Id);
            QueryBuilder.AddParameter("idOrganization", boatOrganization.Organization.Id);
            QueryBuilder.AddParameter("startDate", boatOrganization.StartDate);
            QueryBuilder.AddParameter("endDate", boatOrganization.EndDate);

            QueryBuilder.AddReturnLastInsertedId();

            boatOrganization.Id = base.Execute<int>(context);
        }

        private void UpdateBoatOrganization(BoatOrganization boatOrganization, ITransactionalContext? context = null)
        {
            string sql = @" UPDATE [Boat_Organization] SET
                                EndDate = @endDate";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("endDate", boatOrganization.EndDate);

            QueryBuilder.AddParameter("id", boatOrganization.Id);
            QueryBuilder.AddCondition("Id = @id");

            base.ExecuteAndGetRowsAffected(context);
        }

        #endregion
    }
}

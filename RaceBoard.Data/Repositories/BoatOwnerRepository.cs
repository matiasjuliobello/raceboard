using Dapper;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data.Helpers.Interfaces;
using RaceBoard.Data.Repositories.Base.Abstract;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;

namespace RaceBoard.Data.Repositories
{
    public class BoatOwnerRepository : AbstractRepository, IBoatOwnerRepository
    {
        #region Private Members

        private readonly Dictionary<string, string> _columnsMapping = new()
        {
            { "Id", "[Boat_Owner].Id" },
            { "StartDate", "[Boat_Owner].StartDate" },
            { "EndDate", "[Boat_Owner].EndDate" },
            { "Owner.Id", "[Owner].Id" },
            { "Owner.Firstname", "[Owner].Firstname" },
            { "Owner.Lastname", "[Owner].Lastname" },
            { "Boat.Id", "[Boat].Id" },
            { "Boat.Name", "[Boat].Name" },
            { "Boat.SailNumber", "[Boat].SailNumber"},
            { "Boat.HullNumber", "[Boat].HullNumber"}
        };

        #endregion

        #region Constructors

        public BoatOwnerRepository(IContextResolver contextResolver, IQueryBuilder queryBuilder) : base(contextResolver, queryBuilder)
        {
        }

        #endregion

        #region IBoatOwnerRepository implementation

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
            return base.Exists(id, "Boat_Owner", "Id", context);
        }

        public bool ExistsDuplicate(BoatOwner boatOwner, ITransactionalContext? context = null)
        {
            string condition = "[IdBoat] = @idBoat AND [IdPerson] = @idOwner AND [StartDate] = @startDate";

            string existsQuery = base.GetExistsDuplicateQuery("[Boat_Owner]", condition, "Id", "@id");

            QueryBuilder.AddCommand(existsQuery);
            QueryBuilder.AddParameter("idBoat", boatOwner.Boat.Id);
            QueryBuilder.AddParameter("idOwner", boatOwner.Person.Id);
            QueryBuilder.AddParameter("startDate", boatOwner.StartDate);
            QueryBuilder.AddParameter("id", boatOwner.Id);

            return base.Execute<bool>(context);
        }

        public PaginatedResult<BoatOwner> Get(BoatOwnerSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            return this.GetBoatOwners(searchFilter, paginationFilter, sorting, context);
        }

        public void Set(List<BoatOwner> boatOwners, ITransactionalContext? context = null)
        {
            this.SetBoatOwners(boatOwners, context);
        }

        #endregion

        #region Private Methods

        private PaginatedResult<BoatOwner> GetBoatOwners(BoatOwnerSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            string sql = $@"SELECT
                                [Boat_Owner].Id [Id],
                                [Boat_Owner].StartDate [StartDate],
                                [Boat_Owner].EndDate [EndDate],
                                [Boat_Owner].IsActive [IsActive],
                                [Person].Id [Id],
                                [Person].Lastname [Firstname],
                                [Person].Firstname [Lastname],
                                [Boat].Id [Id],
                                [Boat].Name [Name],
                                [Boat].SailNumber [SailNumber],
                                [Boat].HullNumber [HullNumber]
                            FROM [Boat_Owner] [Boat_Owner]
                            INNER JOIN [Boat] [Boat] ON [Boat].Id = [Boat_Owner].IdBoat
                            INNER JOIN [Person] [Person] ON [Person].Id = [Boat_Owner].IdPerson";

            QueryBuilder.AddCommand(sql);

            ProcessSearchFilter(searchFilter);

            QueryBuilder.AddSorting(sorting, _columnsMapping);
            QueryBuilder.AddPagination(paginationFilter);

            var boatOrganizations = new List<BoatOwner>();

            PaginatedResult<BoatOwner> items = base.GetPaginatedResults<BoatOwner>
                (
                    (reader) =>
                    {
                        return reader.Read<BoatOwner, Person, Boat, BoatOwner>
                        (
                            (boatOwner, person, boat) =>
                            {
                                boatOwner.Boat = boat;
                                boatOwner.Person = person;

                                boatOrganizations.Add(boatOwner);

                                return boatOwner;
                            },
                            splitOn: "Id, Id, Id"
                        ).AsList();
                    },
                    context
                );

            items.Results = boatOrganizations;

            return items;
        }

        private void ProcessSearchFilter(BoatOwnerSearchFilter? searchFilter = null)
        {
            if (searchFilter == null)
                return;

            base.AddFilterCriteria(ConditionType.In, "[Boat_Owner]", "Id", "ids", searchFilter.Ids);
            base.AddFilterCriteria(ConditionType.GreaterOrEqualThan, "[Boat_Owner]", "StartDate", "startDate", searchFilter.StartDate);
            base.AddFilterCriteria(ConditionType.LessOrEqualThan, "[Boat_Owner]", "EndDate", "endDate", searchFilter.EndDate);
            base.AddFilterCriteria(ConditionType.Equal, "[Boat_Owner]", "IsActive", "isActive", searchFilter.IsActive);
            base.AddFilterCriteria(ConditionType.Equal, "[Boat]", "Id", "idBoat", searchFilter.Boat?.Id);
            base.AddFilterCriteria(ConditionType.Equal, "[Person]", "Id", "idOwner", searchFilter.Person?.Id);
        }

        private void SetBoatOwners(List<BoatOwner> newBoatOwners, ITransactionalContext? context = null)
        {
            if (newBoatOwners.Count == 0)
                return;

            int idBoat = newBoatOwners.First().Boat.Id;

            var idsPerson = newBoatOwners.Select(x => x.Person.Id).ToList();

            string sql = @"UPDATE [Boat_Owner] SET EndDate = GETUTCDATE(), IsActive = @isActive ";

            QueryBuilder.AddCommand(sql);
            QueryBuilder.AddParameter("idBoat", idBoat);
            QueryBuilder.AddParameter("activeOwners", idsPerson);
            QueryBuilder.AddParameter("isActive", false);
            QueryBuilder.AddCondition("IdBoat = @idBoat AND EndDate IS NULL AND IdPerson NOT IN @activeOwners");

            base.ExecuteAndGetRowsAffected(context);

            var existingBoatOwners = this.GetBoatOwners(new BoatOwnerSearchFilter()
            {
                Boat = new Boat() { Id = idBoat },
                IsActive = true
            }, null, null, context).Results;

            foreach (var newBoatOwner in newBoatOwners)
            {
                if (existingBoatOwners.FirstOrDefault(x => x.Person.Id == newBoatOwner.Person.Id) != null)
                    continue;

                sql = @"INSERT INTO [Boat_Owner]
                            ( IdBoat, IdPerson, StartDate, EndDate, IsActive )
                        VALUES
                            ( @idBoat, @idOwner, @startDate, NULL, @isActive )";

                QueryBuilder.Clear();
                QueryBuilder.AddCommand(sql);
                QueryBuilder.AddParameter("idBoat", newBoatOwner.Boat.Id);
                QueryBuilder.AddParameter("idOwner", newBoatOwner.Person.Id);
                QueryBuilder.AddParameter("startDate", newBoatOwner.StartDate);
                QueryBuilder.AddParameter("isActive", true);

                QueryBuilder.AddReturnLastInsertedId();

                newBoatOwner.Id = base.Execute<int>(context);
            }
        }

        #endregion
    }
}

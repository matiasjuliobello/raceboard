using Dapper;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data.Helpers.Interfaces;
using RaceBoard.Data.Repositories.Base.Abstract;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;
using System.Text;

namespace RaceBoard.Data.Repositories
{
    public class CoachOrganizationRepository : AbstractRepository, ICoachOrganizationRepository
    {
        #region Private Members

        private readonly Dictionary<string, string> _columnsMapping = new()
        {
            { "Id", "[Coach_Organization].Id" },
            { "StartDate", "[Coach_Organization].StartDate" },
            { "EndDate", "[Coach_Organization].EndDate" },
            { "IsActive", "[Coach_Organization].IsActive" },
            { "Coach.Id", "[Coach].Id" },
            { "Person.Id", "[Person].Id" },
            { "Person.Firstname", "[Person].Firstname" },
            { "Person.Lastname", "[Person].Lastname" },
            { "Organization.Id", "[Organization].Id" },
            { "Organization.Name", "[Organization].Name" }
        };

        #endregion

        #region Constructors

        public CoachOrganizationRepository(IContextResolver contextResolver, IQueryBuilder queryBuilder) : base(contextResolver, queryBuilder)
        {
        }

        #endregion

        #region ICoachOrganizationRepository implementation

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
            return base.Exists(id, "Coach_Organization", "Id", context);
        }

        public bool ExistsDuplicate(CoachOrganization coachOrganization, ITransactionalContext? context = null)
        {
            var sb = new StringBuilder();
            sb.AppendLine("[IdCoach] = @idCoach AND ");
            sb.AppendLine("[IdOrganization] = @idOrganization AND ");
            sb.AppendLine("[StartDate] = @startDate");

            string existsQuery = base.GetExistsDuplicateQuery("[Coach_Organization]", sb.ToString(), "Id", "@id");

            QueryBuilder.AddCommand(existsQuery);
            QueryBuilder.AddParameter("idCoach", coachOrganization.Coach.Id);
            QueryBuilder.AddParameter("idOrganization", coachOrganization.Organization.Id);
            QueryBuilder.AddParameter("startDate", coachOrganization.StartDate);
            QueryBuilder.AddParameter("id", coachOrganization.Id);

            return base.Execute<bool>(context);
        }

        public PaginatedResult<CoachOrganization> Get(CoachOrganizationSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            return this.GetCoachOrganizations(searchFilter, paginationFilter, sorting, context);
        }

        public CoachOrganization? Get(int id, ITransactionalContext? context = null)
        {
            var searchFilter = new CoachOrganizationSearchFilter()
            {
                Ids = new int[] { id }
            };

            return this.GetCoachOrganizations(searchFilter, paginationFilter: null, sorting: null, context).Results.FirstOrDefault();
        }

        public void Create(CoachOrganization coachOrganization, ITransactionalContext? context = null)
        {
            this.CreateCoachOrganization(coachOrganization, context);
        }

        public void Update(CoachOrganization coachOrganization, ITransactionalContext? context = null)
        {
            this.UpdateCoachOrganization(coachOrganization, context);
        }

        #endregion

        #region Private Methods

        private PaginatedResult<CoachOrganization> GetCoachOrganizations(CoachOrganizationSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            string sql = $@"SELECT
                                [Coach_Organization].Id [Id],
                                [Coach_Organization].StartDate [StartDate],
                                [Coach_Organization].EndDate [EndDate],
                                [Coach_Organization].IsActive [IsActive],
                                [Coach].Id [Id],
                                [Person].Id [Id],
                                [Person].Firstname [Firstname],
                                [Person].Lastname [Lastname],
                                [Organization].Id [Id],
                                [Organization].Name [Name]
                            FROM [Coach_Organization] [Coach_Organization]
                            INNER JOIN [Coach] [Coach] ON [Coach].Id = [Coach_Organization].IdCoach
                            INNER JOIN [Person] [Person] ON [Person].Id = [Coach].IdPerson
                            INNER JOIN [Organization] [Organization] ON [Organization].Id = [Coach_Organization].IdOrganization";

            QueryBuilder.AddCommand(sql);

            ProcessSearchFilter(searchFilter);

            QueryBuilder.AddSorting(sorting, _columnsMapping);
            QueryBuilder.AddPagination(paginationFilter);

            var coachOrganizations = new List<CoachOrganization>();

            var items = base.GetPaginatedResults<CoachOrganization>
                (
                    (reader) =>
                    {
                        return reader.Read<CoachOrganization, Coach, Person, Organization, CoachOrganization>
                        (
                            (coachOrganization, coach, person, organization) =>
                            {
                                coach.Person = person;
                                
                                coachOrganization.Organization = organization;
                                coachOrganization.Coach  = coach;

                                coachOrganizations.Add(coachOrganization);

                                return coachOrganization;
                            },
                            splitOn: "Id, Id, Id, Id"
                        ).AsList();
                    },
                    context
                );

            items.Results = coachOrganizations;

            return items;
        }

        private void ProcessSearchFilter(CoachOrganizationSearchFilter? searchFilter = null)
        {
            if (searchFilter == null)
                return;

            base.AddFilterCriteria(ConditionType.In, "[Coach_Organization]", "Id", "ids", searchFilter.Ids);
            base.AddFilterCriteria(ConditionType.GreaterOrEqualThan, "[Coach_Organization]", "StartDate", "startDate", searchFilter.StartDate);
            base.AddFilterCriteria(ConditionType.LessOrEqualThan, "[Coach_Organization]", "EndDate", "endDate", searchFilter.EndDate);
            base.AddFilterCriteria(ConditionType.Equal, "[Coach_Organization]", "IsActive", "isActive", searchFilter.IsActive);
            base.AddFilterCriteria(ConditionType.Equal, "[Coach]", "Id", "idCoach", searchFilter.Coach?.Id);
            base.AddFilterCriteria(ConditionType.Equal, "[Person]", "Id", "idPerson", searchFilter.Coach?.Person?.Id);
            base.AddFilterCriteria(ConditionType.Equal, "[Organization]", "Id", "idOrganization", searchFilter.Organization?.Id);
        }

        private void CreateCoachOrganization(CoachOrganization coachOrganization, ITransactionalContext? context = null)
        {
            string sql = @" INSERT INTO [Coach_Organization]
                                ( IdCoach, IdOrganization, StartDate, EndDate, IsActive )
                            VALUES
                                ( @idCoach, @idOrganization, @startDate, @endDate, @isActive )";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("idCoach", coachOrganization.Coach.Id);
            QueryBuilder.AddParameter("idOrganization", coachOrganization.Organization.Id);
            QueryBuilder.AddParameter("startDate", coachOrganization.StartDate);
            QueryBuilder.AddParameter("endDate", coachOrganization.EndDate);
            QueryBuilder.AddParameter("isActive", coachOrganization.IsActive);

            QueryBuilder.AddReturnLastInsertedId();

            coachOrganization.Id = base.Execute<int>(context);
        }

        private void UpdateCoachOrganization(CoachOrganization coachOrganization, ITransactionalContext? context = null)
        {
            string sql = @" UPDATE [Coach_Organization] SET EndDate = @endDate, IsActive = 0";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("endDate", coachOrganization.EndDate);
            QueryBuilder.AddParameter("id", coachOrganization.Id);
            QueryBuilder.AddCondition("Id = @id");

            base.ExecuteAndGetRowsAffected(context);
        }

        #endregion
    }
}
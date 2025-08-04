using Dapper;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data.Helpers.Interfaces;
using RaceBoard.Data.Repositories.Base.Abstract;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;
using System.Text;

namespace RaceBoard.Data.Repositories
{
    public class CoachRepository : AbstractRepository, ICoachRepository
    {
        #region Private Members

        private readonly Dictionary<string, string> _columnsMapping = new()
        {
            { "Id", "[Coach].Id" },
            { "Person.Id", "[Person].Id" },
            { "Person.Firstname", "[Person].Firstname" },
            { "Person.Lastname", "[Person].Lastname" }
        };

        #endregion

        #region Constructors

        public CoachRepository(IContextResolver contextResolver, IQueryBuilder queryBuilder) : base(contextResolver, queryBuilder)
        {
        }

        #endregion

        #region ICoachRepository implementation

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
            return base.Exists(id, "Coach", "Id", context);
        }

        public bool ExistsDuplicate(Coach coach, ITransactionalContext? context = null)
        {
            var sb = new StringBuilder();
            sb.AppendLine("[IdPerson] = @idPerson");

            string existsQuery = base.GetExistsDuplicateQuery("[Coach]", sb.ToString(), "Id", "@id");

            QueryBuilder.AddCommand(existsQuery);
            QueryBuilder.AddParameter("idPerson", coach.Person.Id);
            QueryBuilder.AddParameter("id", coach.Id);

            return base.Execute<bool>(context);
        }

        public PaginatedResult<Coach> Get(CoachSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            return this.GetCoaches(searchFilter, paginationFilter, sorting, context);
        }

        public Coach? Get(int id, ITransactionalContext? context = null)
        {
            var searchFilter = new CoachSearchFilter()
            {
                Ids = new int[] { id }
            };

            return this.GetCoaches(searchFilter, paginationFilter: null, sorting: null, context).Results.FirstOrDefault();
        }

        public void Create(Coach coachOrganization, ITransactionalContext? context = null)
        {
            this.CreateCoach(coachOrganization, context);
        }

        public void Delete(int id, ITransactionalContext? context = null)
        {
            base.Delete("[Coach]", id, "Id", context);
        }

        #endregion

        #region Private Methods

        private PaginatedResult<Coach> GetCoaches(CoachSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            string sql = $@"SELECT
                                [Coach].Id [Id],
                                [Person].Id [Id],
                                [Person].Firstname [Firstname],
                                [Person].Lastname [Lastname]
                            FROM [Coach] [Coach]
                            INNER JOIN [Person] [Person] ON [Person].Id = [Coach].IdPerson";

            QueryBuilder.AddCommand(sql);

            ProcessSearchFilter(searchFilter);

            QueryBuilder.AddSorting(sorting, _columnsMapping);
            QueryBuilder.AddPagination(paginationFilter);

            var coaches = new List<Coach>();

            var items = base.GetPaginatedResults<Coach>
                (
                    (reader) =>
                    {
                        return reader.Read<Coach, Person, Coach>
                        (
                            (coach, person) =>
                            {
                                coach.Person = person;

                                coaches.Add(coach);

                                return coach;
                            },
                            splitOn: "Id, Id"
                        ).AsList();
                    },
                    context
                );

            items.Results = coaches;

            return items;
        }

        private void ProcessSearchFilter(CoachSearchFilter? searchFilter = null)
        {
            if (searchFilter == null)
                return;

            base.AddFilterCriteria(ConditionType.In, "[Coach]", "Id", "ids", searchFilter.Ids);
            base.AddFilterCriteria(ConditionType.Equal, "[Person]", "Id", "idPerson", searchFilter.Person?.Id);
        }

        private void CreateCoach(Coach coach, ITransactionalContext? context = null)
        {
            string sql = @" INSERT INTO [Coach]
                                ( IdPerson )
                            VALUES
                                ( @idPerson )";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("idPerson", coach.Person.Id);

            QueryBuilder.AddReturnLastInsertedId();

            coach.Id = base.Execute<int>(context);
        }

        #endregion
    }
}
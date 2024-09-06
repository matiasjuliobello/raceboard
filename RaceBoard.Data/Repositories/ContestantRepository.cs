using Dapper;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data.Helpers.Interfaces;
using RaceBoard.Data.Repositories.Base.Abstract;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;

namespace RaceBoard.Data.Repositories
{
    public class ContestantRepository : AbstractRepository, IContestantRepository
    {
        #region Private Members

        private readonly Dictionary<string, string> _columnsMapping = new()
        {
            { "Id", "[Contestant].Id" },
            { "Person.Id", "[Person].Id" },
            { "Person.FirstName", "[Person].Firstname" },
            { "Person.Lastname", "[Person].Lastname" }
        };

        #endregion

        #region Constructors

        public ContestantRepository(IContextResolver contextResolver, IQueryBuilder queryBuilder) : base(contextResolver, queryBuilder)
        {
        }

        #endregion

        #region IContestantRepository implementation

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
            string existsQuery = base.GetExistsQuery("[Contestant]", "[Id] = @id");

            QueryBuilder.AddCommand(existsQuery);
            QueryBuilder.AddParameter("id", id);

            return base.Execute<bool>(context);
        }

        public bool ExistsDuplicate(Contestant contestant, ITransactionalContext? context = null)
        {
            string existsQuery = base.GetExistsDuplicateQuery("[Contestant]", "[IdPerson] = @idPerson", "Id", "@id");

            QueryBuilder.AddCommand(existsQuery);
            QueryBuilder.AddParameter("idPerson", contestant.Person.Id);
            QueryBuilder.AddParameter("id", contestant.Id);

            return base.Execute<bool>(context);
        }

        public PaginatedResult<Contestant> Get(ContestantSearchFilter searchFilter, PaginationFilter paginationFilter, Sorting sorting, ITransactionalContext? context = null)
        {
            return this.GetContestants(searchFilter, paginationFilter, sorting, context);
        }

        public void Create(Contestant contestant, ITransactionalContext? context = null)
        {
            this.CreateContestant(contestant, context);
        }

        public void Update(Contestant contestant, ITransactionalContext? context = null)
        {
            this.UpdateContestant(contestant, context);
        }

        public int Delete(int id, ITransactionalContext? context = null)
        {
            return base.Delete("[Contestant]", id, "Id", context);
        }

        #endregion

        #region Private Methods

        private PaginatedResult<Contestant> GetContestants(ContestantSearchFilter searchFilter, PaginationFilter paginationFilter, Sorting sorting, ITransactionalContext? context = null)
        {
            string sql = $@"SELECT
                                [Contestant].Id [Id],
                                [Person].Id [Id],
                                [Person].Firstname [Firstname],
                                [Person].Lastname [Lastname]
                            FROM [Contestant] [Contestant]
                            INNER JOIN [Person] [Person] ON [Person].Id = [Contestant].IdPerson";

            QueryBuilder.AddCommand(sql);

            ProcessSearchFilter(searchFilter);

            QueryBuilder.AddSorting(sorting, _columnsMapping);
            QueryBuilder.AddPagination(paginationFilter);

            var contestants = new List<Contestant>();

            PaginatedResult<Contestant> items = base.GetPaginatedResults<Contestant>
                (
                    (reader) =>
                    {
                        return reader.Read<Contestant, Person, Contestant>
                        (
                            (contestant, person) =>
                            {
                                contestant.Person = person;

                                contestants.Add(contestant);

                                return contestant;
                            },
                            splitOn: "Id, Id"
                        ).AsList();
                    },
                    context
                );

            items.Results = contestants;

            return items;
        }

        private void ProcessSearchFilter(ContestantSearchFilter searchFilter)
        {
            if (searchFilter.Ids != null && searchFilter.Ids.Length > 0)
            {
                QueryBuilder.AddCondition($"[Contestant].Id IN @ids");
                QueryBuilder.AddParameter("ids", searchFilter.Ids);
            }

            if (searchFilter.Person != null && searchFilter.Person.Id > 0)
            {
                QueryBuilder.AddCondition($"[Person].Id = @idPerson");
                QueryBuilder.AddParameter("idPerson", searchFilter.Person.Id);
            }
        }

        private void CreateContestant(Contestant contestant, ITransactionalContext? context = null)
        {
            string sql = @" INSERT INTO [Contestant]
                                ( IdPerson )
                            VALUES
                                ( @idPerson )";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("idPerson", contestant.Person.Id);

            QueryBuilder.AddReturnLastInsertedId();

            contestant.Id = base.Execute<int>(context);
        }

        private void UpdateContestant(Contestant contestant, ITransactionalContext? context = null)
        {
            string sql = @" UPDATE [Contestant] SET
                                IdPerson = @idPerson";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("idCity", contestant.Person.Id);

            QueryBuilder.AddParameter("id", contestant.Id);
            QueryBuilder.AddCondition("Id = @id");

            base.ExecuteAndGetRowsAffected(context);
        }

        #endregion
    }
}
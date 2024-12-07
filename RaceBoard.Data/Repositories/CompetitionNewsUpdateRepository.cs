using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data.Helpers.Interfaces;
using RaceBoard.Data.Repositories.Base.Abstract;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;

namespace RaceBoard.Data.Repositories
{
    public class CompetitionNewsUpdateRepository : AbstractRepository, ICompetitionNewsUpdateRepository
    {
        #region Private Members

        private readonly Dictionary<string, string> _columnsMapping = new()
        {
            { "Id", "[Competition_NewsUpdate].Id" },
            { "Message", "[Competition_NewsUpdate].Message" },
            { "Timestamp", "[Competition_NewsUpdate].Timestamp" }
        };

        #endregion

        #region Constructors

        public CompetitionNewsUpdateRepository(IContextResolver contextResolver, IQueryBuilder queryBuilder) : base(contextResolver, queryBuilder)
        {
        }

        #endregion

        #region ICompetitionNewsUpdateRepository implementation

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
            return base.Exists(id, "Competition_NewsUpdate", "Id", context);
        }

        public bool ExistsDuplicate(CompetitionNewsUpdate competitionNewsUpdate, ITransactionalContext? context = null)
        {
            return false;
        }

        public PaginatedResult<CompetitionNewsUpdate> Get(CompetitionNewsUpdateSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            return this.GetCompetitionNewsUpdates(searchFilter, paginationFilter, sorting, context);
        }

        public void Create(CompetitionNewsUpdate competitionNewsUpdate, ITransactionalContext? context = null)
        {
            string sql = @" INSERT INTO [Competition_NewsUpdate]
                                ( IdCompetition, Message, Timestamp )
                            VALUES
                                ( @idCompetition, @message, @timestamp )";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("idCompetition", competitionNewsUpdate.Competition.Id);
            QueryBuilder.AddParameter("message", competitionNewsUpdate.Message);
            QueryBuilder.AddParameter("timestamp", competitionNewsUpdate.Timestamp);

            QueryBuilder.AddReturnLastInsertedId();

            competitionNewsUpdate.Id = base.Execute<int>(context);
        }

        #endregion

        #region Private Methods

        private PaginatedResult<CompetitionNewsUpdate> GetCompetitionNewsUpdates(CompetitionNewsUpdateSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            string sql = $@"SELECT
                                [Competition_NewsUpdate].Id [Id],
                                [Competition_NewsUpdate].Message [Message],
                                [Competition_NewsUpdate].Timestamp [Timestamp]
                            FROM [Competition_NewsUpdate] [Competition_NewsUpdate]";

            QueryBuilder.AddCommand(sql);

            ProcessSearchFilter(searchFilter);

            QueryBuilder.AddSorting(sorting, _columnsMapping);
            QueryBuilder.AddPagination(paginationFilter);

            return base.GetMultipleResultsWithPagination<CompetitionNewsUpdate>(context);
        }

        private void ProcessSearchFilter(CompetitionNewsUpdateSearchFilter? searchFilter = null)
        {
            if (searchFilter == null)
                return;

            base.AddFilterCriteria(ConditionType.In, "Competition_NewsUpdate", "Id", "id", searchFilter.Ids);
            base.AddFilterCriteria(ConditionType.Equal, "Competition_NewsUpdate", "IdCompetition", "idCompetition", searchFilter.Competition.Id);
        }

        #endregion
    }
}

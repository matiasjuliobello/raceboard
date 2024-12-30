using RaceBoard.Data.Helpers.Interfaces;
using RaceBoard.Data.Repositories.Base.Abstract;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;

namespace RaceBoard.Data.Repositories
{
    public class RaceProtestRepository : AbstractRepository, IRaceProtestRepository
    {
        #region Private Members

        private readonly Dictionary<string, string> _columnsMapping = new()
        {
            { "Id", "[Race].Id" },
            { "Schedule", "[Race].Schedule" },
            { "RaceClass.Id", "[RaceClass].Id" },
            { "RaceClass.Name", "[RaceClass].Name"},
            { "Championship.Id", "[Championship].Id" },
            { "Championship.Name", "[Championship].Name"},
            { "Championship.StartDate", "[Championship].StartDate"},
            { "Championship.EndDate", "[Championship].EndDate"}
        };

        #endregion

        #region Constructors

        public RaceProtestRepository(IContextResolver contextResolver, IQueryBuilder queryBuilder) : base(contextResolver, queryBuilder)
        {
        }

        #endregion

        #region IRaceProtestRepository implementation

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

        public void Create(RaceProtest raceProtest, ITransactionalContext? context = null)
        {
            this.CreateRaceProtest(raceProtest, context);
        }

        #endregion

        #region Private Methods

        private void CreateRaceProtest(RaceProtest raceProtest, ITransactionalContext? context = null)
        {
            string sql = @" INSERT INTO [Race_Protest]
                                ( IdRace, IdTeamMember, SubmissionDate )
                            VALUES
                                ( @idRace, @idTeamMember, @submissionDate )";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("idRace", raceProtest.Race.Id);
            QueryBuilder.AddParameter("idTeamMember", raceProtest.TeamMember.Id);
            QueryBuilder.AddParameter("submissionDate", raceProtest.Submission);

            QueryBuilder.AddReturnLastInsertedId();

            raceProtest.Id = base.Execute<int>(context);
        }

        #endregion
    }
}
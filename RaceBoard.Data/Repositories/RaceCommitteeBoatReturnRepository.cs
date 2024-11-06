using RaceBoard.Data.Helpers.Interfaces;
using RaceBoard.Data.Repositories.Base.Abstract;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;

namespace RaceBoard.Data.Repositories
{
    public class RaceCommitteeBoatReturnRepository : AbstractRepository, IRaceCommitteeBoatReturnRepository
    {
        #region Private Members

        private readonly Dictionary<string, string> _columnsMapping = new()
        {
            { "Id", "[Race].Id" },
            { "Schedule", "[Race].Schedule" },
            { "RaceClass.Id", "[RaceClass].Id" },
            { "RaceClass.Name", "[RaceClass].Name"},
            { "Competition.Id", "[Competition].Id" },
            { "Competition.Name", "[Competition].Name"},
            { "Competition.StartDate", "[Competition].StartDate"},
            { "Competition.EndDate", "[Competition].EndDate"}
        };

        #endregion

        #region Constructors

        public RaceCommitteeBoatReturnRepository(IContextResolver contextResolver, IQueryBuilder queryBuilder) : base(contextResolver, queryBuilder)
        {
        }

        #endregion

        #region IRaceCommitteeBoatReturnRepository implementation

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

        public void Create(RaceCommitteeBoatReturn raceCommitteeBoatReturn, ITransactionalContext? context = null)
        {
            this.CreateRaceCommitteeBoatReturn(raceCommitteeBoatReturn, context);
        }

        #endregion

        #region Private Methods

        private void CreateRaceCommitteeBoatReturn(RaceCommitteeBoatReturn raceCommitteeBoatReturn, ITransactionalContext? context = null)
        {
            string sql = @" INSERT INTO [Race_CommitteeBoatReturn]
                                ( IdRace, [Return], [Name] )
                            VALUES
                                ( @idRace, @return, @name )";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("idRace", raceCommitteeBoatReturn.Race.Id);
            QueryBuilder.AddParameter("return", raceCommitteeBoatReturn.Return);
            QueryBuilder.AddParameter("name", raceCommitteeBoatReturn.Name);

            QueryBuilder.AddReturnLastInsertedId();

            raceCommitteeBoatReturn.Id = base.Execute<int>(context);
        }

        #endregion
    }
}
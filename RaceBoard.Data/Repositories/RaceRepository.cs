using RaceBoard.Data.Helpers.Interfaces;
using RaceBoard.Data.Repositories.Base.Abstract;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;

namespace RaceBoard.Data.Repositories
{
    public class RaceRepository : AbstractRepository, IRaceRepository
    {
        #region Private Members

        #endregion

        #region Constructors

        public RaceRepository(IContextResolver contextResolver, IQueryBuilder queryBuilder) : base(contextResolver, queryBuilder)
        {
        }

        #endregion

        #region IRaceRepository implementation

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

        public void Create(Race race, ITransactionalContext? context = null)
        {
            this.CreateRace(race, context);
        }

        public void Update(Race race, ITransactionalContext? context = null)
        {
            this.UpdateRace(race, context);
        }

        public int Delete(int id, ITransactionalContext? context = null)
        {
            return base.Delete("[Race]", id, "Id", context);
        }

        #endregion

        #region Private Methods

        private void CreateRace(Race race, ITransactionalContext? context = null)
        {
            string sql = @" INSERT INTO [Race]
                                ( IdRaceClass, IdCompetition )
                            VALUES
                                ( @idRaceClass, @idCompetition )";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("idRaceClass", race.RaceClass.Id);
            QueryBuilder.AddParameter("idCompetition", race.Competition.Id);

            QueryBuilder.AddReturnLastInsertedId();

            race.Id = base.Execute<int>(context);
        }

        private void UpdateRace(Race race, ITransactionalContext? context = null)
        {
            string sql = @" UPDATE [Race] SET
                                IdRaceClass = @idRaceClass,
                                IdCompetition = @idCompetition";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("idRaceClass", race.RaceClass.Id);
            QueryBuilder.AddParameter("idCompetition", race.Competition.Id);

            QueryBuilder.AddParameter("id", race.Id);
            QueryBuilder.AddCondition("Id = @id");

            base.ExecuteAndGetRowsAffected(context);
        }

        #endregion
    }
}
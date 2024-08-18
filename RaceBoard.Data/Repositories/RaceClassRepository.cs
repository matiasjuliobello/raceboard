using RaceBoard.Data.Helpers.Interfaces;
using RaceBoard.Data.Repositories.Base.Abstract;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;

namespace RaceBoard.Data.Repositories
{
    public class RaceClassRepository : AbstractRepository, IRaceClassRepository
    {
        #region Private Members

        #endregion

        #region Constructors

        public RaceClassRepository(IContextResolver contextResolver, IQueryBuilder queryBuilder) : base(contextResolver, queryBuilder)
        {
        }

        #endregion

        #region IRaceClassRepository implementation

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

        public void Create(RaceClass raceClass, ITransactionalContext? context = null)
        {
            this.CreateRaceClass(raceClass, context);
        }

        public void Update(RaceClass raceClass, ITransactionalContext? context = null)
        {
            this.UpdateRaceClass(raceClass, context);
        }

        public int Delete(int id, ITransactionalContext? context = null)
        {
            return base.Delete("[RaceClass]", id, "Id", context);
        }

        #endregion

        #region Private Methods

        private void CreateRaceClass(RaceClass raceClass, ITransactionalContext? context = null)
        {
            string sql = @" INSERT INTO [RaceClass]
                                ( Name, Description )
                            VALUES
                                ( @name, @description )";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("name", raceClass.Name);
            QueryBuilder.AddParameter("description", raceClass.Description);

            QueryBuilder.AddReturnLastInsertedId();

            raceClass.Id = base.Execute<int>(context);
        }

        private void UpdateRaceClass(RaceClass raceClass, ITransactionalContext? context = null)
        {
            string sql = @" UPDATE [RaceClass] SET
                                Name = @name,
                                Description = @description";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("name", raceClass.Name);
            QueryBuilder.AddParameter("description", raceClass.Description);

            QueryBuilder.AddParameter("id", raceClass.Id);
            QueryBuilder.AddCondition("Id = @id");

            base.ExecuteAndGetRowsAffected(context);
        }

        #endregion
    }
}
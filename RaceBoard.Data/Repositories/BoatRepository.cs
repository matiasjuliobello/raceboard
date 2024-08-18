using RaceBoard.Data.Helpers.Interfaces;
using RaceBoard.Data.Repositories.Base.Abstract;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;

namespace RaceBoard.Data.Repositories
{
    public class BoatRepository : AbstractRepository, IBoatRepository
    {
        #region Private Members

        #endregion

        #region Constructors

        public BoatRepository(IContextResolver contextResolver, IQueryBuilder queryBuilder) : base(contextResolver, queryBuilder)
        {
        }

        #endregion

        #region IBoatRepository implementation

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

        public void Create(Boat boat, ITransactionalContext? context = null)
        {
            this.CreateBoat(boat, context);
        }

        public void Update(Boat boat, ITransactionalContext? context = null)
        {
            this.UpdateBoat(boat, context);
        }

        public int Delete(int id, ITransactionalContext? context = null)
        {
            return base.Delete("[Boat]", id, "Id", context);
        }

        #endregion

        #region Private Methods

        private void CreateBoat(Boat boat, ITransactionalContext? context = null)
        {
            string sql = @" INSERT INTO [Boat]
                                ( Name, SailNumber )
                            VALUES
                                ( @name, @sailNumber )";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("name", boat.Name);
            QueryBuilder.AddParameter("sailNumber", boat.SailNumber);

            QueryBuilder.AddReturnLastInsertedId();

            boat.Id = base.Execute<int>(context);
        }

        private void UpdateBoat(Boat boat, ITransactionalContext? context = null)
        {
            string sql = @" UPDATE [Boat] SET
                                Name = @name,
                                SailNumber = @sailNumber";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("name", boat.Name);
            QueryBuilder.AddParameter("sailNumber", boat.SailNumber);

            QueryBuilder.AddParameter("id", boat.Id);
            QueryBuilder.AddCondition("Id = @id");

            base.ExecuteAndGetRowsAffected(context);
        }

        #endregion
    }
}
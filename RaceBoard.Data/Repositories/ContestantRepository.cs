using RaceBoard.Data.Helpers.Interfaces;
using RaceBoard.Data.Repositories.Base.Abstract;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;

namespace RaceBoard.Data.Repositories
{
    public class ContestantRepository : AbstractRepository, IContestantRepository
    {
        #region Private Members

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
using RaceBoard.Data.Helpers.Interfaces;
using RaceBoard.Data.Repositories.Base.Abstract;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;

namespace RaceBoard.Data.Repositories
{
    public class ContestantRoleRepository : AbstractRepository, IContestantRoleRepository
    {
        #region Private Members

        #endregion

        #region Constructors

        public ContestantRoleRepository(IContextResolver contextResolver, IQueryBuilder queryBuilder) : base(contextResolver, queryBuilder)
        {
        }

        #endregion

        #region IContestantRoleRepository implementation

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

        public void Create(ContestantRole contestantRole, ITransactionalContext? context = null)
        {
            this.CreateContestantRole(contestantRole, context);
        }

        public void Update(ContestantRole contestantRole, ITransactionalContext? context = null)
        {
            this.UpdateContestantRole(contestantRole, context);
        }

        public int Delete(int id, ITransactionalContext? context = null)
        {
            return base.Delete("[ContestantRole]", id, "Id", context);
        }

        #endregion

        #region Private Methods

        private void CreateContestantRole(ContestantRole contestantRole, ITransactionalContext? context = null)
        {
            string sql = @" INSERT INTO [ContestantRole]
                                ( Name )
                            VALUES
                                ( @name )";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("name", contestantRole.Name);

            QueryBuilder.AddReturnLastInsertedId();

            contestantRole.Id = base.Execute<int>(context);
        }

        private void UpdateContestantRole(ContestantRole contestantRole, ITransactionalContext? context = null)
        {
            string sql = @" UPDATE [ContestantRole] SET
                                Name = @name";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("name", contestantRole.Name);

            QueryBuilder.AddParameter("id", contestantRole.Id);
            QueryBuilder.AddCondition("Id = @id");

            base.ExecuteAndGetRowsAffected(context);
        }

        #endregion
    }
}
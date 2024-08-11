using System.Data;

namespace RaceBoard.Data
{
    public class TransactionalContext : ITransactionalContext
    {
        private readonly Action<IDbTransaction, bool> _onComplete;

        private IDbTransaction _transaction { get; set; }
        private TransactionContextScope _scope { get; set; }

        public TransactionalContext(IDbTransaction transaction, TransactionContextScope scope, Action<IDbTransaction, bool> onComplete)
        {
            _transaction = transaction;
            _scope = scope;
            _onComplete = onComplete;
        }

        #region IITransactionalContext implementation

        public IDbTransaction Transaction
        {
            get { return _transaction; }
        }

        public TransactionContextScope Scope
        {
            get { return _scope; }
        }

        public void Confirm()
        {
            _onComplete(_transaction, true);
        }

        public void Cancel()
        {
            _onComplete(_transaction, false);
        }

        #endregion
    }
}

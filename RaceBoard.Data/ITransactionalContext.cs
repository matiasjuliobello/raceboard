using System.Data;

namespace RaceBoard.Data
{
    public enum TransactionContextScope
    {
        Internal = 1,
        External = 2
    }

    public interface ITransactionalContext
    {
        IDbTransaction Transaction { get; }
        TransactionContextScope Scope { get; }
        void Confirm();
        void Cancel();
    }
}

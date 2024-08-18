using RaceBoard.Domain;

namespace RaceBoard.Data.Repositories.Interfaces
{
    public interface IRaceClassRepository
    {
        ITransactionalContext GetTransactionalContext(TransactionContextScope scope = TransactionContextScope.Internal);
        void ConfirmTransactionalContext(ITransactionalContext context);
        void CancelTransactionalContext(ITransactionalContext context);

        void Create(RaceClass raceClass, ITransactionalContext? context = null);
        void Update(RaceClass raceClass, ITransactionalContext? context = null);
        int Delete(int id, ITransactionalContext? context = null);
    }
}

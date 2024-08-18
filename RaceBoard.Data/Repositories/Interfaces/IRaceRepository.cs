using RaceBoard.Domain;

namespace RaceBoard.Data.Repositories.Interfaces
{
    public interface IRaceRepository
    {
        ITransactionalContext GetTransactionalContext(TransactionContextScope scope = TransactionContextScope.Internal);
        void ConfirmTransactionalContext(ITransactionalContext context);
        void CancelTransactionalContext(ITransactionalContext context);

        void Create(Race race, ITransactionalContext? context = null);
        void Update(Race race, ITransactionalContext? context = null);
        int Delete(int id, ITransactionalContext? context = null);
    }
}

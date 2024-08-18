using RaceBoard.Domain;

namespace RaceBoard.Data.Repositories.Interfaces
{
    public interface IBoatRepository
    {
        ITransactionalContext GetTransactionalContext(TransactionContextScope scope = TransactionContextScope.Internal);
        void ConfirmTransactionalContext(ITransactionalContext context);
        void CancelTransactionalContext(ITransactionalContext context);

        void Create(Boat boat, ITransactionalContext? context = null);
        void Update(Boat boat, ITransactionalContext? context = null);
        int Delete(int id, ITransactionalContext? context = null);
    }
}

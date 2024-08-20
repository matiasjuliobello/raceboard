using RaceBoard.Domain;

namespace RaceBoard.Data.Repositories.Interfaces
{
    public interface IContestantRepository
    {
        ITransactionalContext GetTransactionalContext(TransactionContextScope scope = TransactionContextScope.Internal);
        void ConfirmTransactionalContext(ITransactionalContext context);
        void CancelTransactionalContext(ITransactionalContext context);

        void Create(Contestant contestant, ITransactionalContext? context = null);
        void Update(Contestant contestant, ITransactionalContext? context = null);
        int Delete(int id, ITransactionalContext? context = null);
    }
}

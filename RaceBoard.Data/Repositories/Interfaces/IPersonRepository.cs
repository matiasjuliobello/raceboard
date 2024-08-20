using RaceBoard.Domain;

namespace RaceBoard.Data.Repositories.Interfaces
{
    public interface IPersonRepository
    {
        ITransactionalContext GetTransactionalContext(TransactionContextScope scope = TransactionContextScope.Internal);
        void ConfirmTransactionalContext(ITransactionalContext context);
        void CancelTransactionalContext(ITransactionalContext context);

        void Create(Person person, ITransactionalContext? context = null);
        void Update(Person person, ITransactionalContext? context = null);
        int Delete(int id, ITransactionalContext? context = null);
    }
}

using RaceBoard.Domain;

namespace RaceBoard.Data.Repositories.Interfaces
{
    public interface IContestantRoleRepository
    {
        ITransactionalContext GetTransactionalContext(TransactionContextScope scope = TransactionContextScope.Internal);
        void ConfirmTransactionalContext(ITransactionalContext context);
        void CancelTransactionalContext(ITransactionalContext context);

        void Create(ContestantRole contestantRole, ITransactionalContext? context = null);
        void Update(ContestantRole contestantRole, ITransactionalContext? context = null);
        int Delete(int id, ITransactionalContext? context = null);
    }
}

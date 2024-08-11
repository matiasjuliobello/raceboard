using RaceBoard.Domain;

namespace RaceBoard.Data.Repositories.Interfaces
{
    public interface IUserPasswordResetRepository
    {
        ITransactionalContext GetTransactionalContext(TransactionContextScope scope = TransactionContextScope.Internal);

        UserPasswordReset GetByToken(string token, bool? isUsed = null, bool? isActive = null, ITransactionalContext? context = null);
        void Create(UserPasswordReset userPasswordReset, ITransactionalContext? context = null);
        void Update(UserPasswordReset userPasswordReset, ITransactionalContext? context = null);
    }
}

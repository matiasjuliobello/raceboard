
using RaceBoard.Domain;

namespace RaceBoard.Data.Repositories.Interfaces
{
    public interface IDeviceRepository
    {
        ITransactionalContext GetTransactionalContext(TransactionContextScope scope = TransactionContextScope.Internal);
        void ConfirmTransactionalContext(ITransactionalContext context);
        void CancelTransactionalContext(ITransactionalContext context);

        bool Exists(string token, ITransactionalContext? context = null);
        Device Get(string token, ITransactionalContext? context = null);
        int Create(Device device, ITransactionalContext? context = null);
        void Update(Device device, ITransactionalContext? context = null);
    }
}

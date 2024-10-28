using RaceBoard.Domain;

namespace RaceBoard.Data.Repositories.Interfaces
{
    public interface IDeviceSubscriptionRepository
    {
        ITransactionalContext GetTransactionalContext(TransactionContextScope scope = TransactionContextScope.Internal);
        void ConfirmTransactionalContext(ITransactionalContext context);
        void CancelTransactionalContext(ITransactionalContext context);

        bool Exists(DeviceSubscription deviceSubscription, ITransactionalContext? context = null);

        DeviceSubscription Get(int idDevice, ITransactionalContext? context = null);
        void Create(DeviceSubscription deviceSubscription, ITransactionalContext? context = null);
        void Update(DeviceSubscription deviceSubscription, ITransactionalContext? context = null);
        int Remove(int idDevice, ITransactionalContext? context = null);
    }
}

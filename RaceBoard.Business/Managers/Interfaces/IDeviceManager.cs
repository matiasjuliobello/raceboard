using RaceBoard.Data;
using RaceBoard.Domain;

namespace RaceBoard.Business.Managers.Interfaces
{
    public interface IDeviceManager
    {
        int Register(Device userDevice, ITransactionalContext? context = null);

        DeviceSubscription GetSubscription(Device device);
        void CreateSubscription(DeviceSubscription deviceSubscription, ITransactionalContext? context = null);
        void RemoveSubscription(int idDevice, ITransactionalContext? context = null);
    }
}

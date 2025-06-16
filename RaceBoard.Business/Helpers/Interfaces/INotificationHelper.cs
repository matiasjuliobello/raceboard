
namespace RaceBoard.Business.Helpers.Interfaces
{
    public interface INotificationHelper
    {
        void SendNotification(Notification.Enums.NotificationType notificationType, object data);
    }
}

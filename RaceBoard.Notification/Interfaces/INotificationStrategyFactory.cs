using RaceBoard.Notification.Enums;

namespace RaceBoard.Notification.Interfaces
{
    public interface INotificationStrategyFactory
    {
        IEnumerable<INotificationStrategy> ResolveStrategy(NotificationType notificationType);
    }
}

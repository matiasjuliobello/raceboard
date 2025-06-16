using RaceBoard.Notification.Enums;

namespace RaceBoard.Notification.Interfaces
{
    public interface INotification
    {
        NotificationMedia Media { get; set; }
        INotificationSettings Settings { get; set; }
        INotificationData Data { get; set; }
    }
}

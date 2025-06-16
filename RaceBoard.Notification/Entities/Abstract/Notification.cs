using RaceBoard.Notification.Enums;
using RaceBoard.Notification.Interfaces;

namespace RaceBoard.Notification.Entities.Abstract
{
    public abstract class Notification : INotification
    {
        public NotificationMedia Media { get; set; }
        public INotificationSettings Settings { get; set; }
        public INotificationData Data { get; set; }
    }
}

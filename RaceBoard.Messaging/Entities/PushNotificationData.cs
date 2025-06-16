using RaceBoard.Notification.Interfaces;
using RaceBoard.PushMessaging.Enums;

namespace RaceBoard.PushMessaging.Entities
{
    public class PushNotificationData : INotificationData
    {
        public PushNotificationType NotificationType { get; set; }
        public string IdTarget { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string ImageFileUrl { get; set; }
        public int IdChampionship { get; set; }
        public int[] IdsRaceClasses { get; set; }
    }
}
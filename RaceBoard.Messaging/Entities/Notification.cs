using RaceBoard.Messaging.Interfaces;
using RaceBoard.Messaging.Providers;

namespace RaceBoard.Messaging.Entities
{
    public class Notification : IMessagingNotification
    {
        public NotificationType NotificationType { get; set; }
        public string IdTarget { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string ImageFileUrl { get; set; }
    }
}

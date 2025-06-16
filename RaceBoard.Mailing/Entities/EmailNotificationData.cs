using RaceBoard.Notification.Interfaces;

namespace RaceBoard.Mailing.Entities
{
    public class EmailNotificationData : INotificationData
    {
        public string Subject { get; set; }
        public string Body { get; set; }
        public string EmailAddress { get; set; }
        public string FullName { get; set; }
    }
}

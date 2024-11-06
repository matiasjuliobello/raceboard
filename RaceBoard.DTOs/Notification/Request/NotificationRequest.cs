namespace RaceBoard.DTOs.Notification.Request
{
    public class NotificationRequest
    {
        public int IdNotificationType {  get; set; }
        public string IdTarget { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string ImageFileUrl { get; set; }
    }
}

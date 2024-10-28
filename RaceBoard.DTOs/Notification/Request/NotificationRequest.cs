namespace RaceBoard.DTOs.Notification.Request
{
    public class NotificationRequest
    {
        public string DeviceToken { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string ImageFileUrl { get; set; }
    }
}

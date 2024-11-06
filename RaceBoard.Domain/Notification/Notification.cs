namespace RaceBoard.Domain.Notification
{
    public class Notification
    {
        public NotificationType NotificationType { get; set; }
        public string IdTarget { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string ImageFileUrl { get; set; }
    }
}

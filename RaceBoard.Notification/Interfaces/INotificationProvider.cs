namespace RaceBoard.Notification.Interfaces
{
    public interface INotificationProvider
    {
        Task Send(INotification notification);
    }
}

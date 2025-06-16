namespace RaceBoard.Notification.Interfaces
{
    public interface INotificationStrategy
    {
        INotification Produce(object invitation);
    }
}

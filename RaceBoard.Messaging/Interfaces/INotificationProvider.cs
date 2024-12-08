using RestSharp;

namespace RaceBoard.Messaging.Interfaces
{
    public interface INotificationProvider
    {
        Task<RestResponse> SendNotification(IMessagingNotification messagingNotification);
    }
}

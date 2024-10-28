using RaceBoard.Domain.Notification;
using RestSharp;

namespace RaceBoard.Business.Managers.Interfaces
{
    public interface INotificationManager
    {
        string GetJwtToken();

        Task<RestResponse> SendNotification(Notification notification);
    }
}
using RaceBoard.Domain;

namespace RaceBoard.Business.Managers.Interfaces
{
    public interface INotificationManager
    {
        Task SendNotifications(string title, string message, int idChampionship, int[] idsRaceClasses);
    }
}

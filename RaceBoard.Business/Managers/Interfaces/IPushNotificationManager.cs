namespace RaceBoard.Business.Managers.Interfaces
{
    public interface IPushNotificationManager
    {
        Task Send(string title, string message, int idChampionship, int[] idsRaceClasses);
    }
}

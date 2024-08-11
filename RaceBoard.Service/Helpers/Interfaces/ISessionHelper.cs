using RaceBoard.Domain;

namespace RaceBoard.Service.Helpers.Interfaces
{
    public interface ISessionHelper
    {
        User GetUser(string username);
        UserSettings GetUserSettings(string username);
    }
}
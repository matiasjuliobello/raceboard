using RaceBoard.Data;
using RaceBoard.Domain;

namespace RaceBoard.Business.Managers.Interfaces
{
    public interface IUserSettingsManager
    {
        UserSettings Get(int idUser, ITransactionalContext? context = null);
        List<UserSettings> Get(int[] idsUser, ITransactionalContext? context = null);
        UserSettings Get(string username, ITransactionalContext? context = null);
        void Create(UserSettings userSettings, ITransactionalContext? context = null);
        void Update(UserSettings userSettings, ITransactionalContext? context = null);
        UserSettings Delete(int id, ITransactionalContext? context = null);
    }
}

using RaceBoard.Domain;

namespace RaceBoard.Data.Repositories.Interfaces
{
    public interface IUserSettingsRepository
    {
        UserSettings GetById(int id, ITransactionalContext? context = null);
        UserSettings GetByIdUser(int idUser, ITransactionalContext? context = null);
        List<UserSettings> GetByIdsUser(int[] idsUser, ITransactionalContext? context = null);
        UserSettings GetByUsername(string username, ITransactionalContext? context = null);
        void Create(UserSettings userSettings, ITransactionalContext? context = null);
        void Update(UserSettings userSettings, ITransactionalContext? context = null);
        int Delete(int id, ITransactionalContext? context = null);
    }
}

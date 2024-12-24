using RaceBoard.Domain;

namespace RaceBoard.Data.Repositories.Interfaces
{
    public interface IUserAccessRepository
    {
        UserAccess Get(int idUser, int idCompetition, ITransactionalContext? context = null);
        void Create(UserAccess userAccess, ITransactionalContext? context = null);
        int Delete(int id, ITransactionalContext? context = null);
    }
}
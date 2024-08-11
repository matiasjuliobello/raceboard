using RaceBoard.Domain;

namespace RaceBoard.Data.Repositories.Interfaces
{
    public interface IAuthenticationRepository
    {
        UserPassword Login(UserLogin userLogin, ITransactionalContext? context = null);
    }
}

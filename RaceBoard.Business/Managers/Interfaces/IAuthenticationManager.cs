using RaceBoard.Data;
using RaceBoard.Domain;

namespace RaceBoard.Business.Managers.Interfaces
{
    public interface IAuthenticationManager
    {
        void Login(UserLogin userLogin, ITransactionalContext? context = null);
    }
}
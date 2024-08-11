using RaceBoard.Data;

namespace RaceBoard.Business.Managers.Interfaces
{
    public interface IUserPasswordResetManager
    {
        void Create(string userEmailAddress, ITransactionalContext? context = null);
        void Update(string token, string password, ITransactionalContext? context = null);
    }
}

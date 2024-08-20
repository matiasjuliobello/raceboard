using RaceBoard.Data;
using RaceBoard.Domain;

namespace RaceBoard.Business.Managers.Interfaces
{
    public interface IContestantRoleManager
    {
        void Create(ContestantRole contestantRole, ITransactionalContext? context = null);
        void Update(ContestantRole contestantRole, ITransactionalContext? context = null);
        void Delete(int id, ITransactionalContext? context = null);
    }
}

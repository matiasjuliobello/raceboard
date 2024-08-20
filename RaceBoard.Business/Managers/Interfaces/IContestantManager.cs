using RaceBoard.Data;
using RaceBoard.Domain;

namespace RaceBoard.Business.Managers.Interfaces
{
    public interface IContestantManager
    {
        void Create(Contestant contestant, ITransactionalContext? context = null);
        void Update(Contestant contestant, ITransactionalContext? context = null);
        void Delete(int id, ITransactionalContext? context = null);
    }
}

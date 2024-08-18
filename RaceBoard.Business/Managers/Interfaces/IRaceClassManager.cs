using RaceBoard.Data;
using RaceBoard.Domain;

namespace RaceBoard.Business.Managers.Interfaces
{
    public interface IRaceClassManager
    {
        void Create(RaceClass raceClass, ITransactionalContext? context = null);
        void Update(RaceClass raceClass, ITransactionalContext? context = null);
        void Delete(int id, ITransactionalContext? context = null);
    }
}

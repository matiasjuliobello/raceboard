using RaceBoard.Data;
using RaceBoard.Domain;

namespace RaceBoard.Business.Managers.Interfaces
{
    public interface IRaceManager
    {
        void Create(Race race, ITransactionalContext? context = null);
        void Update(Race race, ITransactionalContext? context = null);
        void Delete(int id, ITransactionalContext? context = null);
    }
}

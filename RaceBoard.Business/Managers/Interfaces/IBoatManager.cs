using RaceBoard.Data;
using RaceBoard.Domain;

namespace RaceBoard.Business.Managers.Interfaces
{
    public interface IBoatManager
    {
        void Create(Boat boat, ITransactionalContext? context = null);
        void Update(Boat boat, ITransactionalContext? context = null);
        void Delete(int id, ITransactionalContext? context = null);
    }
}

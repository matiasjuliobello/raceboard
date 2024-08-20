using RaceBoard.Data;
using RaceBoard.Domain;

namespace RaceBoard.Business.Managers.Interfaces
{
    public interface IPersonManager
    {
        void Create(Person person, ITransactionalContext? context = null);
        void Update(Person person, ITransactionalContext? context = null);
        void Delete(int id, ITransactionalContext? context = null);
    }
}

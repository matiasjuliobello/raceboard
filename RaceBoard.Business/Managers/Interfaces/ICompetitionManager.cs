using RaceBoard.Data;
using RaceBoard.Domain;

namespace RaceBoard.Business.Managers.Interfaces
{
    public interface ICompetitionManager
    {
        void Create(Competition competition, ITransactionalContext? context = null);
        void Update(Competition competition, ITransactionalContext? context = null);
        void Delete(int id, ITransactionalContext? context = null);
    }
}

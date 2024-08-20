using RaceBoard.Data;
using RaceBoard.Domain;

namespace RaceBoard.Business.Managers.Interfaces
{
    public interface ITeamManager
    {
        void Create(Team team, ITransactionalContext? context = null);
        void Update(Team team, ITransactionalContext? context = null);
        void Delete(int id, ITransactionalContext? context = null);

        void SetBoat(TeamBoat teamBoat, ITransactionalContext? context = null);
        void SetContestants(List<TeamContestant> teamContestants, ITransactionalContext? context = null);
    }
}
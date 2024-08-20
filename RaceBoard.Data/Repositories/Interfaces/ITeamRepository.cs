using RaceBoard.Domain;

namespace RaceBoard.Data.Repositories.Interfaces
{
    public interface ITeamRepository
    {
        ITransactionalContext GetTransactionalContext(TransactionContextScope scope = TransactionContextScope.Internal);
        void ConfirmTransactionalContext(ITransactionalContext context);
        void CancelTransactionalContext(ITransactionalContext context);

        void Create(Team team, ITransactionalContext? context = null);
        void Update(Team team, ITransactionalContext? context = null);
        int Delete(int id, ITransactionalContext? context = null);

        void SetBoat(TeamBoat teamBoat, ITransactionalContext? context = null);
        void SetContestants(List<TeamContestant> teamContestants, ITransactionalContext? context = null);
    }
}

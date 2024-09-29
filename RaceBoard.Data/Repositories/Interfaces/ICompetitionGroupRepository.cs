using RaceBoard.Domain;

namespace RaceBoard.Data.Repositories.Interfaces
{
    public interface ICompetitionGroupRepository
    {
        ITransactionalContext GetTransactionalContext(TransactionContextScope scope = TransactionContextScope.Internal);
        void ConfirmTransactionalContext(ITransactionalContext context);
        void CancelTransactionalContext(ITransactionalContext context);

        bool Exists(int id, ITransactionalContext? context = null);
        bool ExistsDuplicate(CompetitionGroup competitionGroup, ITransactionalContext? context = null);

        CompetitionGroup GetById(int id, ITransactionalContext? context = null);
        List<CompetitionGroup> Get(int idCompetition, ITransactionalContext? context = null);

        void Create(CompetitionGroup competitionGroup, ITransactionalContext? context = null);
        void Update(CompetitionGroup competitionGroup, ITransactionalContext? context = null);
        int Delete(int id, ITransactionalContext? context = null);

        void CreateRaceClasses(int idCompetitionGroup, List<RaceClass> raceClasses, ITransactionalContext? context = null);
        int DeleteRaceClasses(int id, ITransactionalContext? context = null);
    }
}

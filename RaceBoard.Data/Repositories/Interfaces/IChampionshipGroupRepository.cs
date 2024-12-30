using RaceBoard.Domain;

namespace RaceBoard.Data.Repositories.Interfaces
{
    public interface IChampionshipGroupRepository
    {
        ITransactionalContext GetTransactionalContext(TransactionContextScope scope = TransactionContextScope.Internal);
        void ConfirmTransactionalContext(ITransactionalContext context);
        void CancelTransactionalContext(ITransactionalContext context);

        bool Exists(int id, ITransactionalContext? context = null);
        bool ExistsDuplicate(ChampionshipGroup championshipGroup, ITransactionalContext? context = null);

        ChampionshipGroup GetById(int id, ITransactionalContext? context = null);
        List<ChampionshipGroup> Get(int idChampionship, ITransactionalContext? context = null);

        void Create(ChampionshipGroup championshipGroup, ITransactionalContext? context = null);
        void Update(ChampionshipGroup championshipGroup, ITransactionalContext? context = null);
        int Delete(int id, ITransactionalContext? context = null);

        void CreateRaceClasses(int idChampionshipGroup, List<RaceClass> raceClasses, ITransactionalContext? context = null);
        int DeleteRaceClasses(int id, ITransactionalContext? context = null);
    }
}

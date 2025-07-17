using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Domain;

namespace RaceBoard.Data.Repositories.Interfaces
{
    public interface IChampionshipCommitteeBoatReturnRepository
	{
		ITransactionalContext GetTransactionalContext(TransactionContextScope scope = TransactionContextScope.Internal);
		void ConfirmTransactionalContext(ITransactionalContext context);
		void CancelTransactionalContext(ITransactionalContext context);

        PaginatedResult<ChampionshipCommitteeBoatReturn> Get(ChampionshipCommitteeBoatReturnSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null);
        void Create(ChampionshipCommitteeBoatReturn committeeBoatReturn, ITransactionalContext? context = null);
        void Update(ChampionshipCommitteeBoatReturn raceCommitteeBoatReturn, ITransactionalContext? context = null);
        void AssociateRaceClasses(ChampionshipCommitteeBoatReturn raceCommitteeBoatReturn, ITransactionalContext? context = null);
        int Delete(int id, ITransactionalContext? context = null);
        int DeleteRaceClasses(int id, ITransactionalContext? context = null);

    }
}
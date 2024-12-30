using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data.Helpers.SqlBulkHelper;
using RaceBoard.Domain;
using static RaceBoard.Data.Repositories.ChampionshipCommitteeBoatReturnRepository;

namespace RaceBoard.Data.Repositories.Interfaces
{
    public interface IChampionshipCommitteeBoatReturnRepository
	{
		ITransactionalContext GetTransactionalContext(TransactionContextScope scope = TransactionContextScope.Internal);
		void ConfirmTransactionalContext(ITransactionalContext context);
		void CancelTransactionalContext(ITransactionalContext context);

        PaginatedResult<ChampionshipBoatReturn> Get(ChampionshipBoatReturnSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null);
        void Create(ChampionshipBoatReturn committeeBoatReturn, ITransactionalContext? context = null);
        void AssociateRaceClasses(ChampionshipBoatReturn raceCommitteeBoatReturn, ITransactionalContext? context = null);
        int Delete(int id, ITransactionalContext? context = null);
        int DeleteRaceClasses(int id, ITransactionalContext? context = null);

    }
}
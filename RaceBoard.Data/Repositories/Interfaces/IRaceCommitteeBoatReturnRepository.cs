using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Domain;

namespace RaceBoard.Data.Repositories.Interfaces
{
	public interface IRaceCommitteeBoatReturnRepository
	{
		ITransactionalContext GetTransactionalContext(TransactionContextScope scope = TransactionContextScope.Internal);
		void ConfirmTransactionalContext(ITransactionalContext context);
		void CancelTransactionalContext(ITransactionalContext context);

		//PaginatedResult<RaceCommitteeBoatReturn> Get(RaceCommitteeBoatReturnSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null);
		//RaceCommitteeBoatReturn? Get(int id, ITransactionalContext? context = null);
		void Create(RaceCommitteeBoatReturn raceCommitteeBoatReturn, ITransactionalContext? context = null);
        //void Update(RaceCommitteeBoatReturn raceCommitteeBoatReturn, ITransactionalContext? context = null);
        //int Delete(int id, ITransactionalContext? context = null);
    }
}
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Domain;

namespace RaceBoard.Data.Repositories.Interfaces
{
    public interface ICommitteeBoatReturnRepository
	{
		ITransactionalContext GetTransactionalContext(TransactionContextScope scope = TransactionContextScope.Internal);
		void ConfirmTransactionalContext(ITransactionalContext context);
		void CancelTransactionalContext(ITransactionalContext context);

        PaginatedResult<CommitteeBoatReturn> Get(CommitteeBoatReturnSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null);
        void Create(CommitteeBoatReturn committeeBoatReturn, ITransactionalContext? context = null);
        void AssociateRaceClasses(CommitteeBoatReturn raceCommitteeBoatReturn, ITransactionalContext? context = null);
        int Delete(int id, ITransactionalContext? context = null);
        int DeleteRaceClasses(int id, ITransactionalContext? context = null);

    }
}
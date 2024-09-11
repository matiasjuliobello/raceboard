using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Domain;

namespace RaceBoard.Data.Repositories.Interfaces
{
    public interface ITeamBoatRepository
    {
        ITransactionalContext GetTransactionalContext(TransactionContextScope scope = TransactionContextScope.Internal);
        void ConfirmTransactionalContext(ITransactionalContext context);
        void CancelTransactionalContext(ITransactionalContext context);
        bool Exists(int id, ITransactionalContext? context = null);
        bool ExistsDuplicate(TeamBoat teamBoat, ITransactionalContext? context = null);
        PaginatedResult<TeamBoat> Get(TeamBoatSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null);
        TeamBoat? Get(int id, ITransactionalContext? context = null);
        void Create(TeamBoat teamBoat, ITransactionalContext? context = null);
        void Update(TeamBoat teamBoat, ITransactionalContext? context = null);
        void Delete(TeamBoat teamBoat, ITransactionalContext? context = null);
    }
}
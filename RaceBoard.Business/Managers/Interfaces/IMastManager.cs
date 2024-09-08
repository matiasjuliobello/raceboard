using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data;
using RaceBoard.Domain;

namespace RaceBoard.Business.Managers.Interfaces
{
    public interface IMastManager
    {
        PaginatedResult<Mast> Get(MastSearchFilter searchFilter, PaginationFilter paginationFilter, Sorting sorting, ITransactionalContext? context = null);
        Mast Get(int id, ITransactionalContext? context = null);
        void Create(Mast mast, ITransactionalContext? context = null);

        public PaginatedResult<MastFlag> GetFlags(MastFlagSearchFilter searchFilter, PaginationFilter paginationFilter, Sorting sorting, ITransactionalContext? context = null);
        void RaiseFlag(MastFlag mastFlag, ITransactionalContext? context = null);
        void LowerFlag(MastFlag mastFlag, ITransactionalContext? context = null);
    }
}
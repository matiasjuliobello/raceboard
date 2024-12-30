using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Domain;

namespace RaceBoard.Data.Repositories.Interfaces
{
    public interface IChampionshipRepository
    {
        ITransactionalContext GetTransactionalContext(TransactionContextScope scope = TransactionContextScope.Internal);
        void ConfirmTransactionalContext(ITransactionalContext context);
        void CancelTransactionalContext(ITransactionalContext context);
        bool Exists(int id, ITransactionalContext? context = null);
        bool ExistsDuplicate(Championship championship, ITransactionalContext? context = null);
        PaginatedResult<Championship> Get(ChampionshipSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null);
        Championship? Get(int id, ITransactionalContext? context = null);
        void Create(Championship championship, ITransactionalContext? context = null);
        void Update(Championship championship, ITransactionalContext? context = null);
        int Delete(int id, ITransactionalContext? context = null);

        void SetOrganizations(int idChampionship, List<Organization> organizations, ITransactionalContext? context = null);
        int DeleteOrganizations(int idChampionship, ITransactionalContext? context = null);
    }
}
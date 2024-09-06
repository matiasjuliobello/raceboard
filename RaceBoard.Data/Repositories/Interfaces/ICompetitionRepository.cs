using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Domain;

namespace RaceBoard.Data.Repositories.Interfaces
{
    public interface ICompetitionRepository
    {
        ITransactionalContext GetTransactionalContext(TransactionContextScope scope = TransactionContextScope.Internal);
        void ConfirmTransactionalContext(ITransactionalContext context);
        void CancelTransactionalContext(ITransactionalContext context);

        bool Exists(int id, ITransactionalContext? context = null);
        bool ExistsDuplicate(Competition competition, ITransactionalContext? context = null);
        PaginatedResult<Competition> Get(CompetitionSearchFilter searchFilter, PaginationFilter paginationFilter, Sorting sorting, ITransactionalContext? context = null);
        void Create(Competition competition, ITransactionalContext? context = null);
        void Update(Competition competition, ITransactionalContext? context = null);
        int Delete(int id, ITransactionalContext? context = null);
        void SetOrganizations(int idCompetition, List<Organization> organizations, ITransactionalContext? context = null);
        void DeleteOrganizations(int idCompetition, ITransactionalContext? context = null);
    }
}

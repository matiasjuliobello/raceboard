using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Domain;

namespace RaceBoard.Data.Repositories.Interfaces
{
    public interface ICompetitionFileRepository
    {
        ITransactionalContext GetTransactionalContext(TransactionContextScope scope = TransactionContextScope.Internal);
        void ConfirmTransactionalContext(ITransactionalContext context);
        void CancelTransactionalContext(ITransactionalContext context);
        bool Exists(int id, ITransactionalContext? context = null);
        bool ExistsDuplicate(CompetitionFile competitionFile, ITransactionalContext? context = null);
        PaginatedResult<CompetitionFile> Get(CompetitionFileSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null);
        void Create(CompetitionFile competitionFile, ITransactionalContext? context = null);
        void AssociateRaceClasses(CompetitionFile competitionFile, ITransactionalContext? context = null);
        int Delete(int id, ITransactionalContext? context = null);
        int DeleteRaceClasses(int id, ITransactionalContext? context = null);
    }
}

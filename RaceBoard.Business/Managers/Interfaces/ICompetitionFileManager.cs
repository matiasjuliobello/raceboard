using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data;
using RaceBoard.Domain;

namespace RaceBoard.Business.Managers.Interfaces
{
    public interface ICompetitionFileManager
    {
        PaginatedResult<CompetitionFile> Get(CompetitionFileSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null);
        void Create(CompetitionFile competitionFileUpload, ITransactionalContext? context = null);
        void Delete(int id, ITransactionalContext? context = null);
    }
}

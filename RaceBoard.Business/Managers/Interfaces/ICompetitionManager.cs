using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data;
using RaceBoard.Domain;

namespace RaceBoard.Business.Managers.Interfaces
{
    public interface ICompetitionManager
    {
        PaginatedResult<Competition> Get(CompetitionSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null);
        Competition Get(int id, ITransactionalContext? context = null);
        void Create(Competition competition, ITransactionalContext? context = null);
        void Update(Competition competition, ITransactionalContext? context = null);
        void Delete(int id, ITransactionalContext? context = null);

        List<CompetitionRaceClass> GetRaceClasses(int idCompetition, ITransactionalContext? context = null);
        void SetRaceClasses(List<CompetitionRaceClass> competitionRaceClasses, ITransactionalContext? context = null);

        List<CompetitionTerm> GetRegistrationTerms(int idCompetition, ITransactionalContext? context = null);
        void SetRegistrationTerms(List<CompetitionRegistrationTerm> registrationTerms, ITransactionalContext? context = null);

        List<CompetitionTerm> GetAccreditationTerms(int idCompetition, ITransactionalContext? context = null);
        void SetAccreditationTerms(List<CompetitionAccreditationTerm> accreditationTerms, ITransactionalContext? context = null);
    }
}

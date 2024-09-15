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
        PaginatedResult<Competition> Get(CompetitionSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null);
        void Create(Competition competition, ITransactionalContext? context = null);
        void Update(Competition competition, ITransactionalContext? context = null);
        int Delete(int id, ITransactionalContext? context = null);

        void SetOrganizations(int idCompetition, List<Organization> organizations, ITransactionalContext? context = null);
        int DeleteOrganizations(int idCompetition, ITransactionalContext? context = null);

        List<CompetitionRaceClass> GetRaceClasses(int idCompetition, ITransactionalContext? context = null);
        void AddRaceClasses(int idCompetition, List<RaceClass> raceClasses, ITransactionalContext? context = null);
        int RemoveRaceClasses(int idCompetition, ITransactionalContext? context = null);

        List<CompetitionTerm> GetRegistrationTerms(int idCompetition, ITransactionalContext? context = null);
        void AddRegistrationTerms(List<CompetitionRegistrationTerm> registrationTerms, ITransactionalContext? context = null);
        int RemoveRegistrationTerms(int idCompetition, ITransactionalContext? context = null);

        List<CompetitionTerm> GetAccreditationTerms(int idCompetition, ITransactionalContext? context = null);
        void AddAccreditationTerms(List<CompetitionAccreditationTerm> accreditationTerms, ITransactionalContext? context = null);
        int RemoveAccreditationTerms(int idCompetition, ITransactionalContext? context = null);
    }
}
using Language = RaceBoard.Domain.Language;

namespace RaceBoard.Data.Repositories.Interfaces
{
    public interface ILanguageRepository
    {
        List<Language> Get(ITransactionalContext? context = null);
    }
}

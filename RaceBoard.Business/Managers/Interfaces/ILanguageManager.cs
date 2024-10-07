using RaceBoard.Data;
using Language = RaceBoard.Domain.Language;

namespace RaceBoard.Business.Managers.Interfaces
{
    public interface ILanguageManager
    {
        List<Language> Get(ITransactionalContext? context = null);
    }
}
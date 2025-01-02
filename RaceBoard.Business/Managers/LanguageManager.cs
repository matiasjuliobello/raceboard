using RaceBoard.Business.Managers.Abstract;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Data;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Translations.Interfaces;
using ILanguageManager = RaceBoard.Business.Managers.Interfaces.ILanguageManager;
using Language = RaceBoard.Domain.Language;

namespace RaceBoard.Business.Managers
{
    public class LanguageManager : AbstractManager, ILanguageManager
    {
        private readonly ILanguageRepository _languageRepository;

        #region Constructors

        public LanguageManager
            (
                ILanguageRepository languageRepository,
                IRequestContextManager requestContextManager,
                ITranslator translator
            ) : base(requestContextManager, translator)
        {
            _languageRepository = languageRepository;
        }

        #endregion

        #region ILanguageManager implementation

        public List<Language> Get(ITransactionalContext? context = null)
        {
            return _languageRepository.Get(context);
        }

        #endregion

        #region Private Methods

        #endregion
    }
}
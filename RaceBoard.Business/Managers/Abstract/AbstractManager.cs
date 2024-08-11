using RaceBoard.Data;
using RaceBoard.Translations.Interfaces;

namespace RaceBoard.Business.Managers.Abstract
{
    public class AbstractManager
    {
        private readonly ITranslator _translator;

        public AbstractManager(ITranslator translator)
        {
            _translator = translator;
        }

        protected string Translate(string text, params object[] arguments)
        {
            if (_translator == null)
                return text;

            return _translator.Get(text, arguments);
        }

    }
}

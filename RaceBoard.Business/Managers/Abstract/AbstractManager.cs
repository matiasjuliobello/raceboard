using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Common.Enums;
using RaceBoard.Common.Exceptions;
using RaceBoard.Common.Helpers.Interfaces;
using RaceBoard.Domain;
using RaceBoard.Translations.Interfaces;

namespace RaceBoard.Business.Managers.Abstract
{
    public class AbstractManager
    {
        private readonly ITranslator _translator;
        private readonly IRequestContextManager _requestContextManager;


        public AbstractManager(IRequestContextManager requestContextManager, ITranslator translator)
        {
            _requestContextManager = requestContextManager;
            _translator = translator;
        }

        protected string Translate(string text, params object[] arguments)
        {
            if (_translator == null)
                return text;

            return _translator.Get(text, arguments);
        }

        public User GetContextUser()
        {
            return _requestContextManager.GetUser();
        }

        public void ValidateRecordNotFound(object? value)
        {
            if (value == null)
                throw new FunctionalException(ErrorType.NotFound, this.Translate("RecordNotFound"));
        }
    }
}
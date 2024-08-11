using FluentValidation;
using RaceBoard.Business.Validators.Interfaces;
using RaceBoard.Common.Enums;
using RaceBoard.Data;
using RaceBoard.Translations.Interfaces;

namespace RaceBoard.Business.Validators.Abstract
{
    public abstract class AbstractCustomValidator<T> : AbstractValidator<T>, ICustomValidator<T>
    {
        #region Private Members

        private Scenario _scenario;
        private ITranslator _translator;
        private ITransactionalContext _transactionalContext;
        private object _context;

        #endregion

        #region Protected Properties

        protected Scenario Scenario
        {
            get { return _scenario; }
        }

        protected Object Context
        {
            get { return _context; }
        }

        protected ITransactionalContext TransactionalContext
        {
            get { return _transactionalContext; }
        }

        #endregion

        #region Public Properties

        public List<string> Errors { get; private set; }

        public System.Action OnValidate { get; set; }

        #endregion

        #region Constructors

        public AbstractCustomValidator(ITranslator translator)
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            _translator = translator;

            Errors = new List<string>();
        }

        #endregion

        #region Public Methods

        public void SetRules(System.Action onValidate)
        {
            //OnValidate = onValidate;
            onValidate.Invoke();
        }

        public bool IsValid(T item, Scenario scenario, bool? persist = false, string? identifier = null)
        {
            _scenario = scenario;

            //OnValidate.Invoke();

            //var result = this.Validate(item, options => options.IncludeRuleSets(scenario.ToString());
            var result = this.Validate(item);

            //Errors = result.Errors.Select(x => x.ErrorMessage).ToList();

            identifier = !string.IsNullOrEmpty(identifier) ? identifier : "";

            var errors = result.Errors.Select(x => $"{x.ErrorMessage}{identifier}");

            if (persist.HasValue && persist.Value)
            {
                Errors.AddRange(errors);
            }
            else
            {
                Errors.Clear();
                Errors.AddRange(errors);
            }

            return result.IsValid;
        }

        public void SetContext(object context)
        {
            _context = context;
        }

        public void SetTransactionalContext(ITransactionalContext transactionalContext)
        {
            _transactionalContext = transactionalContext;
        }

        #endregion

        #region Protected Methods

        protected string Translate(string text, params object[] arguments)
        {
            if (_translator == null)
                return text;

            return _translator.Get(text, arguments);
        }

        #endregion
    }
}

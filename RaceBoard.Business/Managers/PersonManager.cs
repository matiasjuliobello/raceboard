using RaceBoard.Business.Managers.Abstract;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Data;
using RaceBoard.Translations.Interfaces;
using RaceBoard.Domain;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Business.Validators.Interfaces;
using RaceBoard.Common.Enums;
using RaceBoard.Common.Exceptions;

namespace RaceBoard.Business.Managers
{
    public class PersonManager : AbstractManager, IPersonManager
    {
        private readonly IPersonRepository _personRepository;
        private readonly ICustomValidator<Person> _personValidator;


        #region Constructors

        public PersonManager
            (
                IPersonRepository personRepository,
                ICustomValidator<Person> personValidator,
                ITranslator translator
            ) : base(translator)
        {
            _personRepository = personRepository;
            _personValidator = personValidator;
        }

        #endregion

        #region IPersonManager implementation


        public PaginatedResult<Person> Get(PersonSearchFilter searchFilter, PaginationFilter paginationFilter, Sorting sorting, ITransactionalContext? context = null)
        {
            return _personRepository.Get(searchFilter, paginationFilter, sorting, context);
        }

        public void Create(Person person, ITransactionalContext? context = null)
        {
            _personValidator.SetTransactionalContext(context);

            if (!_personValidator.IsValid(person, Scenario.Create))
                throw new FunctionalException(ErrorType.ValidationError, _personValidator.Errors);

            if (context == null)
                context = _personRepository.GetTransactionalContext(TransactionContextScope.Internal);

            try
            {
                _personRepository.Create(person, context);
                _personRepository.SetUserAssociation(person, context);

                _personRepository.ConfirmTransactionalContext(context);
            }
            catch (Exception)
            {
                _personRepository.CancelTransactionalContext(context);
                throw;
            }
        }

        public void Update(Person person, ITransactionalContext? context = null)
        {
            _personValidator.SetTransactionalContext(context);

            if (!_personValidator.IsValid(person, Scenario.Create))
                throw new FunctionalException(ErrorType.ValidationError, _personValidator.Errors);

            if (context == null)
                context = _personRepository.GetTransactionalContext(TransactionContextScope.Internal);

            try
            {
                _personRepository.Update(person, context);
                _personRepository.SetUserAssociation(person, context);

                _personRepository.ConfirmTransactionalContext(context);

            }
            catch (Exception)
            {
                _personRepository.CancelTransactionalContext(context);
                throw;
            }
        }

        public void Delete(int id, ITransactionalContext? context = null)
        {
            //_personValidator.SetTransactionalContext(context);
            //if (!_personValidator.IsValid(person, Scenario.Create))
            //    throw new FunctionalException(ErrorType.ValidationError, _personValidator.Errors);

            if (context == null)
                context = _personRepository.GetTransactionalContext(TransactionContextScope.Internal);

            try
            {
                _personRepository.Delete(id, context);

                _personRepository.ConfirmTransactionalContext(context);

            }
            catch (Exception)
            {
                _personRepository.CancelTransactionalContext(context);
                throw;
            }
        }

        #endregion
    }
}
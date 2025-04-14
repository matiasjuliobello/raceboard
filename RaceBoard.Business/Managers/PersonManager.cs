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
using System;

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
                IRequestContextManager requestContextManager,
                ITranslator translator
            ) : base(requestContextManager, translator)
        {
            _personRepository = personRepository;
            _personValidator = personValidator;
        }

        #endregion

        #region IPersonManager implementation

        public PaginatedResult<Person> Search(string searchTerm, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            return _personRepository.Search(searchTerm, paginationFilter, sorting, context);
        }

        public PaginatedResult<Person> Get(PersonSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            return _personRepository.Get(searchFilter, paginationFilter, sorting, context);
        }

        public Person Get(int id, ITransactionalContext? context = null)
        {
            var person = _personRepository.Get(id, context);
            if (person == null)
                throw new FunctionalException(ErrorType.NotFound, this.Translate("RecordNotFound"));

            return person;
        }

        public Person GetByIdUser(int idUser, ITransactionalContext? context = null)
        {
            var person = _personRepository.GetByIdUser(idUser, context);
            //if (person == null)
            //    throw new FunctionalException(ErrorType.NotFound, this.Translate("RecordNotFound"));

            return person;
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
            var contextUser = base.GetContextUser();
            if (contextUser.Id != person.User.Id)
                throw new FunctionalException(ErrorType.Forbidden, base.Translate("NeedPermissions"));


            _personValidator.SetTransactionalContext(context);

            if (!_personValidator.IsValid(person, Scenario.Update))
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
            var person = this.Get(id, context);

            var contextUser = base.GetContextUser();
            if (contextUser.Id != person.User.Id)
                throw new FunctionalException(ErrorType.Forbidden, base.Translate("NeedPermissions"));

            _personValidator.SetTransactionalContext(context);

            if (!_personValidator.IsValid(person, Scenario.Delete))
                throw new FunctionalException(ErrorType.ValidationError, _personValidator.Errors);

            if (context == null)
                context = _personRepository.GetTransactionalContext(TransactionContextScope.Internal);

            try
            {
                int affectedRecords = _personRepository.Delete(id, context);
                if (affectedRecords == 0)
                    throw new FunctionalException(ErrorType.ValidationError, base.Translate("DeleteFailed"));

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
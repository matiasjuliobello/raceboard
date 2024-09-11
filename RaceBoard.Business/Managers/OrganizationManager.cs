using RaceBoard.Business.Managers.Abstract;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Data;
using RaceBoard.Translations.Interfaces;
using RaceBoard.Domain;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Common.Enums;
using RaceBoard.Common.Exceptions;
using RaceBoard.Business.Validators.Interfaces;

namespace RaceBoard.Business.Managers
{
    public class OrganizationManager : AbstractManager, IOrganizationManager
    {
        private readonly IOrganizationRepository _organizationRepository;
        private readonly ICustomValidator<Organization> _organizationValidator;

        #region Constructors

        public OrganizationManager
            (
                IOrganizationRepository organizationRepository,
                ICustomValidator<Organization> organizationValidator,
                ITranslator translator
            ) : base(translator)
        {
            _organizationRepository = organizationRepository;
            _organizationValidator = organizationValidator;
        }

        #endregion

        #region IOrganizationManager implementation

        public PaginatedResult<Organization> Get(OrganizationSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            return _organizationRepository.Get(searchFilter, paginationFilter, sorting, context);
        }

        public Organization Get(int id, ITransactionalContext? context = null)
        {
            var person = _organizationRepository.Get(id, context);
            if (person == null)
                throw new FunctionalException(ErrorType.NotFound, this.Translate("RecordNotFound"));

            return person;
        }

        public void Create(Organization organization, ITransactionalContext? context = null)
        {
            _organizationValidator.SetTransactionalContext(context);

            if (!_organizationValidator.IsValid(organization, Scenario.Create))
                throw new FunctionalException(ErrorType.ValidationError, _organizationValidator.Errors);

            if (context == null)
                context = _organizationRepository.GetTransactionalContext(TransactionContextScope.Internal);

            try
            {
                _organizationRepository.Create(organization, context);

                _organizationRepository.ConfirmTransactionalContext(context);

            }
            catch (Exception)
            {
                _organizationRepository.CancelTransactionalContext(context);
                throw;
            }
        }

        public void Update(Organization organization, ITransactionalContext? context = null)
        {
            _organizationValidator.SetTransactionalContext(context);

            if (!_organizationValidator.IsValid(organization, Scenario.Update))
                throw new FunctionalException(ErrorType.ValidationError, _organizationValidator.Errors);

            if (context == null)
                context = _organizationRepository.GetTransactionalContext(TransactionContextScope.Internal);

            try
            {
                _organizationRepository.Update(organization, context);

                _organizationRepository.ConfirmTransactionalContext(context);

            }
            catch (Exception)
            {
                _organizationRepository.CancelTransactionalContext(context);
                throw;
            }
        }

        public void Delete(int id, ITransactionalContext? context = null)
        {
            var organization = this.Get(id, context);

            _organizationValidator.SetTransactionalContext(context);

            if (!_organizationValidator.IsValid(organization, Scenario.Delete))
                throw new FunctionalException(ErrorType.ValidationError, _organizationValidator.Errors);

            if (context == null)
                context = _organizationRepository.GetTransactionalContext(TransactionContextScope.Internal);

            try
            {
                _organizationRepository.Delete(id, context);

                _organizationRepository.ConfirmTransactionalContext(context);

            }
            catch (Exception)
            {
                _organizationRepository.CancelTransactionalContext(context);
                throw;
            }
        }

        #endregion
    }
}
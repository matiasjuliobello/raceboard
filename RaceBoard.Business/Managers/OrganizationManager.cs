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
using RaceBoard.Common.Helpers.Interfaces;
using Enums = RaceBoard.Domain.Enums;

namespace RaceBoard.Business.Managers
{
    public class OrganizationManager : AbstractManager, IOrganizationManager
    {
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IOrganizationMemberRepository _organizationMemberRepository;
        private readonly ICustomValidator<Organization> _organizationValidator;
        private readonly IDateTimeHelper _dateTimeHelper;

        #region Constructors

        public OrganizationManager
            (
                IOrganizationRepository organizationRepository,
                IOrganizationMemberRepository organizationMemberRepository,
                ICustomValidator<Organization> organizationValidator,
                IDateTimeHelper dateTimeHelper,
                ITranslator translator
            ) : base(translator)
        {
            _organizationRepository = organizationRepository;
            _organizationMemberRepository = organizationMemberRepository;
            _organizationValidator = organizationValidator;
            _dateTimeHelper = dateTimeHelper;
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

            var currentDate = _dateTimeHelper.GetCurrentTimestamp();

            try
            {
                _organizationRepository.Create(organization, context);

                var organizationMember = new OrganizationMember()
                {
                    IsActive = true,
                    JoinDate = currentDate,
                    Organization = organization,
                    User = organization.CreationUser,
                    Role = new Role() { Id = (int)Enums.UserRole.Manager }
                };
                _organizationMemberRepository.Add(organizationMember, context);

                context.Confirm();
            }
            catch (Exception)
            {
                if (context != null)
                    context.Cancel();

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
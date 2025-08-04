using RaceBoard.Business.Managers.Abstract;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Business.Validators.Interfaces;
using RaceBoard.Common.Enums;
using RaceBoard.Common.Exceptions;
using RaceBoard.Common.Helpers.Interfaces;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;
using RaceBoard.Translations.Interfaces;

namespace RaceBoard.Business.Managers
{
    public class BoatOrganizationManager : AbstractManager, IBoatOrganizationManager
    {
        private readonly IBoatOrganizationRepository _boatOrganizationRepository;
        private readonly ICustomValidator<BoatOrganization> _boatOrganizationValidator;
        private readonly IDateTimeHelper _dateTimeHelper;

        #region Constructors

        public BoatOrganizationManager
            (
                IBoatOrganizationRepository boatOrganizationRepository,
                ICustomValidator<BoatOrganization> boatOrganizationValidator,
                IRequestContextManager requestContextManager,
                IDateTimeHelper dateTimeHelper,
                ITranslator translator
            ) : base(requestContextManager, translator)
        {
            _boatOrganizationRepository = boatOrganizationRepository;
            _boatOrganizationValidator = boatOrganizationValidator;
            _dateTimeHelper = dateTimeHelper;
        }

        #endregion

        #region IBoatOrganizationManager implementation
        public PaginatedResult<BoatOrganization> Get(BoatOrganizationSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            return _boatOrganizationRepository.Get(searchFilter, paginationFilter, sorting, context);
        }

        public void Set(List<BoatOrganization> boatOrganizations, ITransactionalContext? context = null)
        {
            var startDate = _dateTimeHelper.GetCurrentTimestamp();

            boatOrganizations.ForEach(x => x.StartDate = startDate);

            _boatOrganizationValidator.SetTransactionalContext(context);

            foreach (var boatOrganization in boatOrganizations)
            {
                if (!_boatOrganizationValidator.IsValid(boatOrganization, Scenario.Create))
                    throw new FunctionalException(ErrorType.ValidationError, _boatOrganizationValidator.Errors);
            }

            if (context == null)
                context = _boatOrganizationRepository.GetTransactionalContext(TransactionContextScope.Internal);

            try
            {
                _boatOrganizationRepository.Set(boatOrganizations, context);

                _boatOrganizationRepository.ConfirmTransactionalContext(context);
            }
            catch (Exception)
            {
                _boatOrganizationRepository.CancelTransactionalContext(context);
                throw;
            }
        }

        #endregion
    }
}

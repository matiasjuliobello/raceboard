using RaceBoard.Business.Managers.Abstract;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Business.Validators.Interfaces;
using RaceBoard.Data;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;
using RaceBoard.Translations.Interfaces;
using RaceBoard.Common.Exceptions;
using RaceBoard.Common.Enums;
using RaceBoard.Common.Helpers.Pagination;

namespace RaceBoard.Business.Managers
{
    public class BoatOrganizationManager : AbstractManager, IBoatOrganizationManager
    {
        private readonly IBoatOrganizationRepository _boatOrganizationRepository;
        private readonly ICustomValidator<BoatOrganization> _boatOrganizationValidator;

        #region Constructors

        public BoatOrganizationManager
            (
                IBoatOrganizationRepository boatOrganizationRepository,
                ICustomValidator<BoatOrganization> boatOrganizationValidator,
                IRequestContextManager requestContextManager,
                ITranslator translator
            ) : base(requestContextManager, translator)
        {
            _boatOrganizationRepository = boatOrganizationRepository;
            _boatOrganizationValidator = boatOrganizationValidator;
        }

        #endregion

        #region IBoatOrganizationManager implementation
        public PaginatedResult<BoatOrganization> Get(BoatOrganizationSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            return _boatOrganizationRepository.Get(searchFilter, paginationFilter, sorting, context);
        }

        public BoatOrganization Get(int id, ITransactionalContext? context = null)
        {
            var searchFilter = new BoatOrganizationSearchFilter()
            {
                Ids = new int[] { id } 
            };

            var boatOrganizations = _boatOrganizationRepository.Get(searchFilter: searchFilter, paginationFilter: null, sorting: null, context);

            var boatOrganization = boatOrganizations.Results.FirstOrDefault();
            if (boatOrganization == null)
                throw new FunctionalException(ErrorType.NotFound, this.Translate("RecordNotFound"));

            return boatOrganization;
        }

        public void Create(BoatOrganization boatOrganization, ITransactionalContext? context = null)
        {
            _boatOrganizationValidator.SetTransactionalContext(context);

            if (!_boatOrganizationValidator.IsValid(boatOrganization, Scenario.Create))
                throw new FunctionalException(ErrorType.ValidationError, _boatOrganizationValidator.Errors);

            if (context == null)
                context = _boatOrganizationRepository.GetTransactionalContext(TransactionContextScope.Internal);

            try
            {
                _boatOrganizationRepository.Create(boatOrganization, context);

                _boatOrganizationRepository.ConfirmTransactionalContext(context);
            }
            catch (Exception)
            {
                _boatOrganizationRepository.CancelTransactionalContext(context);
                throw;
            }
        }

        public void Update(BoatOrganization boatOrganization, ITransactionalContext? context = null)
        {
            _boatOrganizationValidator.SetTransactionalContext(context);

            if (!_boatOrganizationValidator.IsValid(boatOrganization, Scenario.Update))
                throw new FunctionalException(ErrorType.ValidationError, _boatOrganizationValidator.Errors);

            if (context == null)
                context = _boatOrganizationRepository.GetTransactionalContext(TransactionContextScope.Internal);

            try
            {
                _boatOrganizationRepository.Update(boatOrganization, context);

                _boatOrganizationRepository.ConfirmTransactionalContext(context);
            }
            catch (Exception)
            {
                _boatOrganizationRepository.CancelTransactionalContext(context);
                throw;
            }
        }

        public void Delete(int id, ITransactionalContext? context = null)
        {
            var boat = this.Get(id, context);

            _boatOrganizationValidator.SetTransactionalContext(context);

            if (!_boatOrganizationValidator.IsValid(boat, Scenario.Delete))
                throw new FunctionalException(ErrorType.ValidationError, _boatOrganizationValidator.Errors);

            if (context == null)
                context = _boatOrganizationRepository.GetTransactionalContext(TransactionContextScope.Internal);

            try
            {
                _boatOrganizationRepository.Delete(id, context);

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

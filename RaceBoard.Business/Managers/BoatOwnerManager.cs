
using RaceBoard.Business.Managers.Abstract;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Business.Validators.Interfaces;
using RaceBoard.Data;
using RaceBoard.Domain;
using RaceBoard.Translations.Interfaces;
using RaceBoard.Common.Exceptions;
using RaceBoard.Common.Enums;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Common.Helpers.Interfaces;
using RaceBoard.Data.Repositories.Interfaces;

namespace RaceBoard.Business.Managers
{
    public class BoatOwnerManager : AbstractManager, IBoatOwnerManager
    {
        private readonly IBoatOwnerRepository _boatOwnerRepository;
        private readonly ICustomValidator<BoatOwner> _boatOwnerValidator;
        private readonly IDateTimeHelper _dateTimeHelper;

        #region Constructors

        public BoatOwnerManager
            (
                IBoatOwnerRepository boatOwnerRepository,
                ICustomValidator<BoatOwner> boatOwnerValidator,
                IRequestContextManager requestContextManager,
                IDateTimeHelper dateTimeHelper,
                ITranslator translator
            ) : base(requestContextManager, translator)
        {
            _boatOwnerRepository = boatOwnerRepository;
            _boatOwnerValidator = boatOwnerValidator;
            _dateTimeHelper = dateTimeHelper;
        }

        #endregion

        #region IBoatOwnerManager implementation


        public PaginatedResult<BoatOwner> Get(BoatOwnerSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            return _boatOwnerRepository.Get(searchFilter, paginationFilter, sorting, context);
        }

        public void Set(List<BoatOwner> boatOwners, ITransactionalContext? context = null)
        {
            var startDate = _dateTimeHelper.GetCurrentTimestamp();

            boatOwners.ForEach(x => x.StartDate = startDate);

            _boatOwnerValidator.SetTransactionalContext(context);

            foreach (var boatOwner in boatOwners)
            {
                if (!_boatOwnerValidator.IsValid(boatOwner, Scenario.Create))
                    throw new FunctionalException(ErrorType.ValidationError, _boatOwnerValidator.Errors);
            }

            if (context == null)
                context = _boatOwnerRepository.GetTransactionalContext(TransactionContextScope.Internal);

            try
            {
                _boatOwnerRepository.Set(boatOwners, context);

                _boatOwnerRepository.ConfirmTransactionalContext(context);
            }
            catch (Exception)
            {
                _boatOwnerRepository.CancelTransactionalContext(context);
                throw;
            }
        }

        #endregion
    }
}

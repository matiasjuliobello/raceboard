using RaceBoard.Business.Managers.Abstract;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Data;
using RaceBoard.Translations.Interfaces;
using RaceBoard.Domain;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Business.Validators.Interfaces;
using RaceBoard.Common.Exceptions;
using RaceBoard.Common.Enums;

namespace RaceBoard.Business.Managers
{
    public class BoatManager : AbstractManager, IBoatManager
    {
        private readonly IBoatRepository _boatRepository;
        private readonly ICustomValidator<Boat> _boatValidator;

        #region Constructors

        public BoatManager
            (
                IBoatRepository boatRepository,
                ICustomValidator<Boat> boatValidator,
                IRequestContextManager requestContextManager,
                ITranslator translator
            ) : base(requestContextManager, translator)
        {
            _boatRepository = boatRepository;
            _boatValidator = boatValidator;
        }

        #endregion

        #region IBoatManager implementation

        public PaginatedResult<Boat> Search(string searchTerm, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            return _boatRepository.Search(searchTerm, paginationFilter, sorting, context);
        }

        public PaginatedResult<Boat> Get(BoatSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            return _boatRepository.Get(searchFilter, paginationFilter, sorting, context);
        }

        public Boat Get(int id, ITransactionalContext? context = null)
        {
            var searchFilter = new BoatSearchFilter() { Ids = new int[] { id } };

            var boats = _boatRepository.Get(searchFilter: searchFilter, paginationFilter: null, sorting: null, context);

            var boat = boats.Results.FirstOrDefault();
            if (boat == null)
                throw new FunctionalException(ErrorType.NotFound, this.Translate("RecordNotFound"));

            return boat;
        }

        public void Create(Boat boat, ITransactionalContext? context = null)
        {
            _boatValidator.SetTransactionalContext(context);

            if (!_boatValidator.IsValid(boat, Scenario.Create))
                throw new FunctionalException(ErrorType.ValidationError, _boatValidator.Errors);

            if (context == null)
                context = _boatRepository.GetTransactionalContext(TransactionContextScope.Internal);

            try
            {
                _boatRepository.Create(boat, context);

                _boatRepository.ConfirmTransactionalContext(context);

            }
            catch (Exception)
            {
                _boatRepository.CancelTransactionalContext(context);
                throw;
            }
        }

        public void Update(Boat boat, ITransactionalContext? context = null)
        {
            _boatValidator.SetTransactionalContext(context);

            if (!_boatValidator.IsValid(boat, Scenario.Update))
                throw new FunctionalException(ErrorType.ValidationError, _boatValidator.Errors);

            if (context == null)
                context = _boatRepository.GetTransactionalContext(TransactionContextScope.Internal);

            try
            {
                _boatRepository.Update(boat, context);

                _boatRepository.ConfirmTransactionalContext(context);

            }
            catch (Exception)
            {
                _boatRepository.CancelTransactionalContext(context);
                throw;
            }
        }

        public void Delete(int id, ITransactionalContext? context = null)
        {
            var boat = this.Get(id, context);

            _boatValidator.SetTransactionalContext(context);

            if (!_boatValidator.IsValid(boat, Scenario.Delete))
                throw new FunctionalException(ErrorType.ValidationError, _boatValidator.Errors);

            if (context == null)
                context = _boatRepository.GetTransactionalContext(TransactionContextScope.Internal);

            try
            {
                _boatRepository.Delete(id, context);

                _boatRepository.ConfirmTransactionalContext(context);

            }
            catch (Exception)
            {
                _boatRepository.CancelTransactionalContext(context);
                throw;
            }
        }

        #endregion
    }
}
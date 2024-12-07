using RaceBoard.Business.Managers.Abstract;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Business.Validators;
using RaceBoard.Business.Validators.Interfaces;
using RaceBoard.Common.Enums;
using RaceBoard.Common.Exceptions;
using RaceBoard.Common.Helpers.Interfaces;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data;
using RaceBoard.Data.Repositories;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;
using RaceBoard.Translations.Interfaces;
using System;

namespace RaceBoard.Business.Managers
{
    public class MastManager : AbstractManager, IMastManager
    {
        private readonly IMastRepository _mastRepository;
        private readonly IMastFlagRepository _mastFlagRepository;
        private readonly ICustomValidator<Mast> _mastValidator;
        private readonly ICustomValidator<MastFlag> _mastFlagValidator;

        #region Constructors

        public MastManager
            (
                IMastRepository mastRepository,
                IMastFlagRepository mastFlagRepository,
                ICustomValidator<Mast> mastValidator,
                ICustomValidator<MastFlag> mastFlagValidator,
                ITranslator translator
            ) : base(translator)
        {
            _mastRepository = mastRepository;
            _mastFlagRepository = mastFlagRepository;
            _mastValidator = mastValidator;
            _mastFlagValidator = mastFlagValidator;
        }

        #endregion

        #region IMastManager implementation

        public PaginatedResult<Mast> Get(MastSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            return _mastRepository.Get(searchFilter, paginationFilter, sorting, context);
        }

        public Mast Get(int id, ITransactionalContext? context = null)
        {
            var searchFilter = new MastSearchFilter() { Ids = new int[] { id } };

            var masts = _mastRepository.Get(searchFilter: searchFilter, paginationFilter: null, sorting: null, context);

            var mast = masts.Results.FirstOrDefault();

            base.ValidateRecordNotFound(mast);

            return mast;
        }

        public void Create(Mast mast, ITransactionalContext? context = null)
        {
            _mastValidator.SetTransactionalContext(context);

            if (!_mastValidator.IsValid(mast, Scenario.Create))
                throw new FunctionalException(ErrorType.ValidationError, _mastValidator.Errors);

            if (context == null)
                context = _mastRepository.GetTransactionalContext(TransactionContextScope.Internal);

            try
            {
                _mastRepository.Create(mast, context);

                _mastRepository.ConfirmTransactionalContext(context);

            }
            catch (Exception)
            {
                _mastRepository.CancelTransactionalContext(context);
                throw;
            }
        }

        public PaginatedResult<MastFlag> GetFlags(MastFlagSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            return _mastFlagRepository.Get(searchFilter, paginationFilter, sorting, context);
        }

        public void RaiseFlag(MastFlag mastFlag, ITransactionalContext? context = null)
        {
            _mastFlagValidator.SetTransactionalContext(context);

            if (!_mastFlagValidator.IsValid(mastFlag, Scenario.Create))
                throw new FunctionalException(ErrorType.ValidationError, _mastFlagValidator.Errors);

            if (context == null)
                context = _mastFlagRepository.GetTransactionalContext(TransactionContextScope.Internal);

            try
            {
                _mastFlagRepository.Create(mastFlag, context);

                _mastFlagRepository.ConfirmTransactionalContext(context);

            }
            catch (Exception)
            {
                _mastFlagRepository.CancelTransactionalContext(context);
                throw;
            }
        }

        public void LowerFlag(MastFlag mastFlag, ITransactionalContext? context = null)
        {
            _mastFlagValidator.SetTransactionalContext(context);

            if (!_mastFlagValidator.IsValid(mastFlag, Scenario.Update))
                throw new FunctionalException(ErrorType.ValidationError, _mastFlagValidator.Errors);

            if (context == null)
                context = _mastFlagRepository.GetTransactionalContext(TransactionContextScope.Internal);

            try
            {
                _mastFlagRepository.Update(mastFlag, context);

                _mastFlagRepository.ConfirmTransactionalContext(context);

            }
            catch (Exception)
            {
                _mastFlagRepository.CancelTransactionalContext(context);
                throw;
            }
        }

        public void RemoveFlag(int id, ITransactionalContext? context = null)
        {
            var filter = new MastFlagSearchFilter()
            {
                Ids = new int[] { id }
            };
            var mastFlags = this.GetFlags(searchFilter: filter, context: context);

            if (mastFlags.Results.Count() == 0)
                throw new FunctionalException(ErrorType.NotFound, this.Translate("RecordNotFound"));

            //_competitionValidator.SetTransactionalContext(context);

            //if (!_competitionValidator.IsValid(competition, Scenario.Delete))
            //    throw new FunctionalException(ErrorType.ValidationError, _competitionValidator.Errors);
            
            if (context == null)
                context = _mastFlagRepository.GetTransactionalContext(TransactionContextScope.Internal);

            try
            {
                _mastFlagRepository.Delete(id, context);

                context.Confirm();
            }
            catch (Exception)
            {
                if (context != null)
                    context.Cancel();
                
                throw;
            }
        }

        #endregion

        #region Private Methods

        #endregion
    }
}

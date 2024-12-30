using Microsoft.Extensions.Configuration;
using RaceBoard.Business.Managers.Abstract;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Business.Validators;
using RaceBoard.Business.Validators.Interfaces;
using RaceBoard.Common.Enums;
using RaceBoard.Common.Exceptions;
using RaceBoard.Common.Extensions;
using RaceBoard.Common.Helpers;
using RaceBoard.Common.Helpers.Interfaces;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data;
using RaceBoard.Data.Repositories;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;
using RaceBoard.FileStorage.Interfaces;
using RaceBoard.Translations.Interfaces;


namespace RaceBoard.Business.Managers
{
    public class ChampionshipFileManager : AbstractManager, IChampionshipFileManager
    {
        private readonly IChampionshipFileRepository _championshipFileRepository;
        private readonly ICustomValidator<ChampionshipFile> _championshipFileValidator;
        private readonly IDateTimeHelper _dateTimeHelper;

        private readonly IFileHelper _fileHelper;
        private readonly IFileRepository _fileRepository;

        #region Constructors

        public ChampionshipFileManager
            (
                IChampionshipFileRepository championshipFileRepository,
                ICustomValidator<ChampionshipFile> championshipFileValidator,
                ITranslator translator,
                IDateTimeHelper dateTimeHelper,
                IFileHelper fileHelper,
                IFileStorageProvider fileStorageProvider,
                IFileRepository fileRepository,
                IConfiguration configuration
            ) : base(translator)
        {
            _championshipFileRepository = championshipFileRepository;
            _championshipFileValidator = championshipFileValidator;
            _dateTimeHelper = dateTimeHelper;
            _fileHelper = fileHelper;
            _fileRepository = fileRepository;
        }

        #endregion

        #region IChampionshipFileManager implementation

        public PaginatedResult<ChampionshipFile> Get(ChampionshipFileSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            return _championshipFileRepository.Get(searchFilter, paginationFilter, sorting, context);
        }

        public ChampionshipFile Get(int id, ITransactionalContext? context = null)
        {
            var searchFilter = new ChampionshipFileSearchFilter() { Ids = new int[] { id } };

            var championshipFiles = _championshipFileRepository.Get(searchFilter: searchFilter, paginationFilter: null, sorting: null, context);

            var competitonFile = championshipFiles.Results.FirstOrDefault();
            if (competitonFile == null)
                throw new FunctionalException(ErrorType.NotFound, this.Translate("RecordNotFound"));

            return competitonFile;
        }

        public void Create(ChampionshipFile championshipFile, ITransactionalContext? context = null)
        {
            if (context == null)
                context = _championshipFileRepository.GetTransactionalContext(TransactionContextScope.Internal);

            //_championshipValidator.SetTransactionalContext(context);
            //if (!_championshipValidator.IsValid(championship, Scenario.Create))
            //    throw new FunctionalException(ErrorType.ValidationError, _championshipValidator.Errors);

            try
            {
                championshipFile.File.Path = _fileHelper.SaveFile(Common.CommonValues.Directories.Files, championshipFile.Championship.Id.ToString(), championshipFile.File.Name, championshipFile.File.Content);

                _fileRepository.Create(championshipFile.File, context);

                _championshipFileRepository.Create(championshipFile, context);
                _championshipFileRepository.AssociateRaceClasses(championshipFile, context);

                context.Confirm();
            }
            catch (Exception)
            {
                if (context != null)
                    context.Cancel();

                if (!string.IsNullOrEmpty(championshipFile.File.Path))
                    _fileHelper.DeleteFile(Path.Combine(championshipFile.File.Path, championshipFile.File.Name));

                throw;
            }
        }

        public void Delete(int id, ITransactionalContext? context = null)
        {
            var championshipFile = this.Get(id, context);

            //_championshipValidator.SetTransactionalContext(context);

            //if (!_championshipValidator.IsValid(championshipFile, Scenario.Delete))
            //    throw new FunctionalException(ErrorType.ValidationError, _championshipValidator.Errors);

            if (context == null)
                context = _championshipFileRepository.GetTransactionalContext(TransactionContextScope.Internal);

            try
            {
                _championshipFileRepository.DeleteRaceClasses(championshipFile.Id, context);
                _championshipFileRepository.Delete(championshipFile.Id, context);

                _fileRepository.Delete(championshipFile.File.Id, context);

                _fileHelper.DeleteFile(Common.CommonValues.Directories.Files, championshipFile.Championship.Id.ToString(), championshipFile.File.Name);

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
    }
}

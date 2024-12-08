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
    public class CompetitionFileManager : AbstractManager, ICompetitionFileManager
    {
        private readonly ICompetitionFileRepository _competitionFileRepository;
        private readonly ICustomValidator<CompetitionFile> _competitionFileValidator;
        private readonly IDateTimeHelper _dateTimeHelper;

        private readonly IFileHelper _fileHelper;
        //private readonly IFileStorageProvider _fileStorageProvider;
        private readonly IFileRepository _fileRepository;

        #region Constructors

        public CompetitionFileManager
            (
                ICompetitionFileRepository competitionFileRepository,
                ICustomValidator<CompetitionFile> competitionFileValidator,
                ITranslator translator,
                IDateTimeHelper dateTimeHelper,
                IFileHelper fileHelper,
                IFileStorageProvider fileStorageProvider,
                IFileRepository fileRepository,
                IConfiguration configuration
            ) : base(translator)
        {
            _competitionFileRepository = competitionFileRepository;
            _competitionFileValidator = competitionFileValidator;
            _dateTimeHelper = dateTimeHelper;
            _fileHelper = fileHelper;
            //_fileStorageProvider = fileStorageProvider;
            _fileRepository = fileRepository;

            //string currentWorkingPath = AppDomain.CurrentDomain.BaseDirectory; // Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            //_fileStorageProvider.SetCurrentDirectory(currentWorkingPath);
        }

        #endregion

        #region ICompetitionFileManager implementation

        public PaginatedResult<CompetitionFile> Get(CompetitionFileSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            return _competitionFileRepository.Get(searchFilter, paginationFilter, sorting, context);
        }

        public CompetitionFile Get(int id, ITransactionalContext? context = null)
        {
            var searchFilter = new CompetitionFileSearchFilter() { Ids = new int[] { id } };

            var competitionFiles = _competitionFileRepository.Get(searchFilter: searchFilter, paginationFilter: null, sorting: null, context);

            var competitonFile = competitionFiles.Results.FirstOrDefault();
            if (competitonFile == null)
                throw new FunctionalException(ErrorType.NotFound, this.Translate("RecordNotFound"));

            return competitonFile;
        }

        public void Create(CompetitionFile competitionFile, ITransactionalContext? context = null)
        {
            if (context == null)
                context = _competitionFileRepository.GetTransactionalContext(TransactionContextScope.Internal);

            //_competitionValidator.SetTransactionalContext(context);
            //if (!_competitionValidator.IsValid(competition, Scenario.Create))
            //    throw new FunctionalException(ErrorType.ValidationError, _competitionValidator.Errors);

            try
            {
                competitionFile.File.Path = _fileHelper.SaveFile(Common.CommonValues.Directories.Files, competitionFile.Competition.Id.ToString(), competitionFile.File.Name, competitionFile.File.Content);

                _fileRepository.Create(competitionFile.File, context);

                _competitionFileRepository.Create(competitionFile, context);
                _competitionFileRepository.AssociateRaceClasses(competitionFile, context);

                context.Confirm();
            }
            catch (Exception)
            {
                if (context != null)
                    context.Cancel();

                if (!string.IsNullOrEmpty(competitionFile.File.Path))
                    _fileHelper.DeleteFile(Path.Combine(competitionFile.File.Path, competitionFile.File.Name));

                throw;
            }
        }

        public void Delete(int id, ITransactionalContext? context = null)
        {
            var competitionFile = this.Get(id, context);

            //_competitionValidator.SetTransactionalContext(context);

            //if (!_competitionValidator.IsValid(competitionFile, Scenario.Delete))
            //    throw new FunctionalException(ErrorType.ValidationError, _competitionValidator.Errors);

            if (context == null)
                context = _competitionFileRepository.GetTransactionalContext(TransactionContextScope.Internal);

            try
            {
                _competitionFileRepository.DeleteRaceClasses(competitionFile.Id, context);
                _competitionFileRepository.Delete(competitionFile.Id, context);

                _fileRepository.Delete(competitionFile.File.Id, context);

                _fileHelper.DeleteFile(Common.CommonValues.Directories.Files, competitionFile.Competition.Id.ToString(), competitionFile.File.Name);

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

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
using RaceBoard.Data.Repositories;

namespace RaceBoard.Business.Managers
{
    public class CompetitionManager : AbstractManager, ICompetitionManager
    {
        private readonly ICompetitionRepository _competitionRepository;
        private readonly ICompetitionGroupRepository _competitionGroupRepository;
        private readonly IFileRepository _fileRepository;

        private readonly ICommitteeBoatReturnRepository _committeeBoatReturnRepository;
        private readonly ICustomValidator<CommitteeBoatReturn> _committeeBoatReturnValidator;

        private readonly ICustomValidator<Competition> _competitionValidator;
        private readonly ICustomValidator<CompetitionGroup> _competitionGroupValidator;

        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly IFileHelper _fileHelper;

        #region Constructors

        public CompetitionManager
            (
                ICompetitionRepository competitionRepository,
                ICompetitionGroupRepository competitionGrpupRepository,
                IFileRepository fileRepository,
                ICustomValidator<Competition> competitionValidator,
                ICustomValidator<CompetitionGroup> competitionGroupValidator,
                ICommitteeBoatReturnRepository committeeBoatReturnRepository,
                ICustomValidator<CommitteeBoatReturn> committeeBoatReturnValidator,
                IDateTimeHelper dateTimeHelper,
                IFileHelper fileHelper,
                ITranslator translator
            ) : base(translator)
        {
            _competitionRepository = competitionRepository;
            _fileRepository = fileRepository;
            _competitionGroupRepository = competitionGrpupRepository;
            _committeeBoatReturnRepository = committeeBoatReturnRepository;
            _competitionValidator = competitionValidator;
            _competitionGroupValidator = competitionGroupValidator;
            _committeeBoatReturnValidator = committeeBoatReturnValidator;
            _dateTimeHelper = dateTimeHelper;
            _fileHelper = fileHelper;
        }

        #endregion

        #region ICompetitionManager implementation

        public PaginatedResult<Competition> Get(CompetitionSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            return _competitionRepository.Get(searchFilter, paginationFilter, sorting, context);
        }

        public Competition Get(int id, ITransactionalContext? context = null)
        {
            var searchFilter = new CompetitionSearchFilter() { Ids = new int[] { id } };

            var competitions = _competitionRepository.Get(searchFilter: searchFilter, paginationFilter: null, sorting: null, context);

            var competiton = competitions.Results.FirstOrDefault();
            if (competiton == null)
                throw new FunctionalException(ErrorType.NotFound, this.Translate("RecordNotFound"));

            return competiton;
        }

        public void Create(Competition competition, ITransactionalContext? context = null)
        {
            if (context == null)
                context = _competitionRepository.GetTransactionalContext(TransactionContextScope.Internal);

            _competitionValidator.SetTransactionalContext(context);

            if (!_competitionValidator.IsValid(competition, Scenario.Create))
                throw new FunctionalException(ErrorType.ValidationError, _competitionValidator.Errors);

            try
            {
                _competitionRepository.Create(competition, context);
                _competitionRepository.SetOrganizations(competition.Id, competition.Organizations, context);

                if (competition.ImageFile != null)
                {
                    competition.ImageFile.Path = _fileHelper.SaveFile(Common.CommonValues.Directories.Files, competition.Id.ToString(), competition.ImageFile.Name, competition.ImageFile.Content);
                    _fileRepository.Create(competition.ImageFile, context);
                    _competitionRepository.Update(competition, context);
                }

                context.Confirm();
            }
            catch (Exception)
            {
                if (context != null)
                    context.Cancel();

                if (!string.IsNullOrEmpty(competition.ImageFile?.Path))
                    _fileHelper.DeleteFile(Path.Combine(competition.ImageFile.Path, competition.ImageFile.Name));

                throw;
            }
        }

        public void Update(Competition competition, ITransactionalContext? context = null)
        {
            if (context == null)
                context = _competitionRepository.GetTransactionalContext(TransactionContextScope.Internal);

            _competitionValidator.SetTransactionalContext(context);

            if (!_competitionValidator.IsValid(competition, Scenario.Update))
                throw new FunctionalException(ErrorType.ValidationError, _competitionValidator.Errors);

            try
            {
                _competitionRepository.Update(competition, context);
                _competitionRepository.SetOrganizations(competition.Id, competition.Organizations, context);

                if (competition.ImageFile != null)
                {
                    competition.ImageFile.Path = _fileHelper.SaveFile(Common.CommonValues.Directories.Files, competition.Id.ToString(), competition.ImageFile.Name, competition.ImageFile.Content);
                    _fileRepository.Create(competition.ImageFile, context);
                    _competitionRepository.Update(competition, context);
                }

                _competitionRepository.ConfirmTransactionalContext(context);

            }
            catch (Exception)
            {
                _competitionRepository.CancelTransactionalContext(context);
                throw;
            }
        }

        public void Delete(int id, ITransactionalContext? context = null)
        {
            var competition = this.Get(id, context);

            _competitionValidator.SetTransactionalContext(context);

            if (!_competitionValidator.IsValid(competition, Scenario.Delete))
                throw new FunctionalException(ErrorType.ValidationError, _competitionValidator.Errors);

            if (context == null)
                context = _competitionRepository.GetTransactionalContext(TransactionContextScope.Internal);

            try
            {
                _competitionRepository.DeleteOrganizations(id, context);
                _competitionRepository.Delete(id, context);

                _competitionRepository.ConfirmTransactionalContext(context);

            }
            catch (Exception)
            {
                _competitionRepository.CancelTransactionalContext(context);
                throw;
            }
        }

        public List<CompetitionGroup> GetGroups(int idCompetition, ITransactionalContext? context = null)
        {
            return _competitionGroupRepository.Get(idCompetition, context);
        }

        public CompetitionGroup GetGroup(int id, ITransactionalContext? context = null)
        {
            return _competitionGroupRepository.GetById(id, context);
        }

        public void CreateGroup(CompetitionGroup competitionGroup, ITransactionalContext? context = null)
        {
            if (context == null)
                context = _competitionGroupRepository.GetTransactionalContext(TransactionContextScope.Internal);

            _competitionGroupValidator.SetTransactionalContext(context);

            if (!_competitionGroupValidator.IsValid(competitionGroup, Scenario.Create))
                throw new FunctionalException(ErrorType.ValidationError, _competitionGroupValidator.Errors);

            try
            {
                _competitionGroupRepository.Create(competitionGroup, context);
                _competitionGroupRepository.DeleteRaceClasses(competitionGroup.Id, context);
                _competitionGroupRepository.CreateRaceClasses(competitionGroup.Id, competitionGroup.RaceClasses, context);

                _competitionGroupRepository.ConfirmTransactionalContext(context);
            }
            catch (Exception)
            {
                _competitionGroupRepository.CancelTransactionalContext(context);
                throw;
            }
        }

        public void UpdateGroup(CompetitionGroup competitionGroup, ITransactionalContext? context = null)
        {
            if (context == null)
                context = _competitionGroupRepository.GetTransactionalContext(TransactionContextScope.Internal);

            _competitionGroupValidator.SetTransactionalContext(context);

            if (!_competitionGroupValidator.IsValid(competitionGroup, Scenario.Update))
                throw new FunctionalException(ErrorType.ValidationError, _competitionGroupValidator.Errors);

            try
            {
                _competitionGroupRepository.Update(competitionGroup, context);
                _competitionGroupRepository.DeleteRaceClasses(competitionGroup.Id, context);
                _competitionGroupRepository.CreateRaceClasses(competitionGroup.Id, competitionGroup.RaceClasses, context);

                _competitionGroupRepository.ConfirmTransactionalContext(context);
            }
            catch (Exception)
            {
                _competitionGroupRepository.CancelTransactionalContext(context);
                throw;
            }
        }

        public void DeleteGroup(int idCompetitionGroup, ITransactionalContext? context = null)
        {
            var competitionGroup = this.GetGroup(idCompetitionGroup, context);

            _competitionGroupValidator.SetTransactionalContext(context);

            if (!_competitionGroupValidator.IsValid(competitionGroup, Scenario.Delete))
                throw new FunctionalException(ErrorType.ValidationError, _competitionGroupValidator.Errors);

            if (context == null)
                context = _competitionRepository.GetTransactionalContext(TransactionContextScope.Internal);

            try
            {
                _competitionGroupRepository.DeleteRaceClasses(idCompetitionGroup, context);
                _competitionGroupRepository.Delete(idCompetitionGroup, context);

                _competitionRepository.ConfirmTransactionalContext(context);
            }
            catch (Exception)
            {
                _competitionRepository.CancelTransactionalContext(context);
                throw;
            }
        }

        public PaginatedResult<CommitteeBoatReturn> GetCommitteeBoatReturns(CommitteeBoatReturnSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            return _committeeBoatReturnRepository.Get(searchFilter, paginationFilter, sorting, context);
        }

        public void CreateCommitteeBoatReturn(CommitteeBoatReturn committeeBoatReturn, ITransactionalContext? context = null)
        {
            committeeBoatReturn.ReturnTime = _dateTimeHelper.GetCurrentTimestamp();

            _committeeBoatReturnValidator.SetTransactionalContext(context);

            if (!_committeeBoatReturnValidator.IsValid(committeeBoatReturn, Scenario.Create))
                throw new FunctionalException(ErrorType.ValidationError, _committeeBoatReturnValidator.Errors);

            if (context == null)
                context = _committeeBoatReturnRepository.GetTransactionalContext(TransactionContextScope.Internal);

            try
            {
                _committeeBoatReturnRepository.Create(committeeBoatReturn, context);
                _committeeBoatReturnRepository.AssociateRaceClasses(committeeBoatReturn, context);

                context.Confirm();
            }
            catch (Exception)
            {
                if (context != null)
                    context.Cancel();

                throw;
            }
        }

        public void DeleteCommitteeBoatReturn(int id, ITransactionalContext? context = null)
        {
            if (context == null)
                context = _committeeBoatReturnRepository.GetTransactionalContext(TransactionContextScope.Internal);

            var searchFilter = new CommitteeBoatReturnSearchFilter() { Ids = new int[] { id } };
            var committeeBoatReturn = this.GetCommitteeBoatReturns(searchFilter, paginationFilter: null, sorting: null, context);

            //_committeeBoatReturnValidator.SetTransactionalContext(context);

            //if (!_committeeBoatReturnValidator.IsValid(committeeBoatReturn, Scenario.Delete))
            //    throw new FunctionalException(ErrorType.ValidationError, _committeeBoatReturnValidator.Errors);

            if (context == null)
                context = _committeeBoatReturnRepository.GetTransactionalContext(TransactionContextScope.Internal);

            try
            {
                _committeeBoatReturnRepository.DeleteRaceClasses(id, context);
                _committeeBoatReturnRepository.Delete(id, context);

                context.Confirm();
            }
            catch (Exception)
            {
                if (context != null)
                    context.Cancel();

                throw;
            }
        }

        public void CreateProtest(RaceProtest raceProtest, ITransactionalContext? context = null)
        {
            //raceProtest.Submission = _dateTimeHelper.GetCurrentTimestamp();

            //_raceProtestValidator.SetTransactionalContext(context);

            //if (!_raceProtestValidator.IsValid(raceProtest, Scenario.Create))
            //    throw new FunctionalException(ErrorType.ValidationError, _raceProtestValidator.Errors);

            //if (context == null)
            //    context = _raceProtestRepository.GetTransactionalContext(TransactionContextScope.Internal);

            //try
            //{
            //    _raceProtestRepository.Create(raceProtest, context);

            //    _raceProtestRepository.ConfirmTransactionalContext(context);
            //}
            //catch (Exception)
            //{
            //    _raceProtestRepository.CancelTransactionalContext(context);
            //    throw;
            //}
            throw new NotImplementedException();
        }

        #endregion
    }
}
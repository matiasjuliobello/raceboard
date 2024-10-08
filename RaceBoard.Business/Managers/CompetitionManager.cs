﻿using RaceBoard.Business.Managers.Abstract;
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
    public class CompetitionManager : AbstractManager, ICompetitionManager
    {
        private readonly ICompetitionRepository _competitionRepository;
        private readonly ICompetitionGroupRepository _competitionGroupRepository;

        private readonly ICustomValidator<Competition> _competitionValidator;
        private readonly ICustomValidator<CompetitionGroup> _competitionGroupValidator;

        #region Constructors

        public CompetitionManager
            (
                ICompetitionRepository competitionRepository,
                ICompetitionGroupRepository competitionGrpupRepository,
                ICustomValidator<Competition> competitionValidator,
                ICustomValidator<CompetitionGroup> competitionGroupValidator,
                ITranslator translator
            ) : base(translator)
        {
            _competitionRepository = competitionRepository;
            _competitionGroupRepository = competitionGrpupRepository;
            _competitionValidator = competitionValidator;
            _competitionGroupValidator = competitionGroupValidator;
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

                _competitionRepository.ConfirmTransactionalContext(context);

            }
            catch (Exception)
            {
                _competitionRepository.CancelTransactionalContext(context);
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


        #endregion
    }
}
using RaceBoard.Business.Managers.Abstract;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Business.Validators.Interfaces;
using RaceBoard.Common.Enums;
using RaceBoard.Common.Exceptions;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;
using RaceBoard.Translations.Interfaces;

namespace RaceBoard.Business.Managers
{
    public class TeamMemberManager : AbstractManager, ITeamMemberManager
    {
        private readonly ITeamMemberRepository _teamMemberRepository;
        private readonly ICustomValidator<TeamMember> _teamMemberValidator;

        #region Constructors

        public TeamMemberManager
            (
                ITeamMemberRepository teamMemberRepository,
                ICustomValidator<TeamMember> teamMemberValidator,
                ITranslator translator
            ) : base(translator)
        {
            _teamMemberRepository = teamMemberRepository;
            _teamMemberValidator = teamMemberValidator;
        }

        #endregion

        #region ITeamMemberManager implementation

        public PaginatedResult<TeamMember> Get(TeamMemberSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            return _teamMemberRepository.Get(searchFilter, paginationFilter, sorting, context);
        }

        public TeamMember Get(int id, ITransactionalContext? context = null)
        {
            var teamMember = _teamMemberRepository.Get(id, context);
            if (teamMember == null)
                throw new FunctionalException(ErrorType.NotFound, this.Translate("RecordNotFound"));

            return teamMember;
        }

        public void Create(TeamMember teamMember, ITransactionalContext? context = null)
        {
            _teamMemberValidator.SetTransactionalContext(context);

            if (!_teamMemberValidator.IsValid(teamMember, Scenario.Create))
                throw new FunctionalException(ErrorType.ValidationError, _teamMemberValidator.Errors);

            if (context == null)
                context = _teamMemberRepository.GetTransactionalContext(TransactionContextScope.Internal);

            try
            {
                _teamMemberRepository.Create(teamMember, context);

                _teamMemberRepository.ConfirmTransactionalContext(context);
            }
            catch (Exception)
            {
                _teamMemberRepository.CancelTransactionalContext(context);
                throw;
            }
        }

        public void Update(TeamMember teamMember, ITransactionalContext? context = null)
        {
            _teamMemberValidator.SetTransactionalContext(context);

            if (!_teamMemberValidator.IsValid(teamMember, Scenario.Update))
                throw new FunctionalException(ErrorType.ValidationError, _teamMemberValidator.Errors);

            if (context == null)
                context = _teamMemberRepository.GetTransactionalContext(TransactionContextScope.Internal);

            try
            {
                _teamMemberRepository.Update(teamMember, context);

                _teamMemberRepository.ConfirmTransactionalContext(context);
            }
            catch (Exception)
            {
                _teamMemberRepository.CancelTransactionalContext(context);
                throw;
            }
        }

        public void Delete(int id, ITransactionalContext? context = null)
        {
            var teamMember = this.Get(id, context);

            _teamMemberValidator.SetTransactionalContext(context);

            if (!_teamMemberValidator.IsValid(teamMember, Scenario.Delete))
                throw new FunctionalException(ErrorType.ValidationError, _teamMemberValidator.Errors);

            if (context == null)
                context = _teamMemberRepository.GetTransactionalContext(TransactionContextScope.Internal);

            try
            {
                _teamMemberRepository.Delete(teamMember, context);

                _teamMemberRepository.ConfirmTransactionalContext(context);

            }
            catch (Exception)
            {
                _teamMemberRepository.CancelTransactionalContext(context);
                throw;
            }
        }

        #endregion
    }
}

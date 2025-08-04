using RaceBoard.Business.Managers.Abstract;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Data;
using RaceBoard.Translations.Interfaces;
using RaceBoard.Domain;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Common.Exceptions;
using RaceBoard.Business.Validators.Interfaces;
using RaceBoard.Common.Helpers.Interfaces;
using RaceBoard.Common.Enums;
using Enums = RaceBoard.Domain.Enums;
using RaceBoard.Business.Helpers.Interfaces;

namespace RaceBoard.Business.Managers
{
    public class TeamManager : AbstractManager, ITeamManager
    {
        private readonly ITeamRepository _teamRepository;
        private readonly ITeamMemberRepository _teamMemberRepository;
        private readonly ICustomValidator<Team> _teamValidator;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly INotificationHelper _notificationHelper;

        #region Constructors

        public TeamManager
            (
                ITeamRepository teamRepository,
                ITeamMemberRepository teamMemberRepository,
                ICustomValidator<Team> teamValidator,
                IDateTimeHelper dateTimeHelper,
                INotificationHelper notificationHelper,
                IRequestContextManager requestContextManager,
                ITranslator translator
            ) : base(requestContextManager, translator)
        {
            _teamRepository = teamRepository;
            _teamMemberRepository = teamMemberRepository;

            _teamValidator = teamValidator;

            _dateTimeHelper = dateTimeHelper;
            _notificationHelper = notificationHelper;
        }

        #endregion

        #region ITeamManager implementation

        public PaginatedResult<Team> Get(TeamSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            return _teamRepository.Get(searchFilter, paginationFilter, sorting, context);
        }

        public Team Get(int id, ITransactionalContext? context = null)
        {
            var team = _teamRepository.Get(id, context);
            if (team == null)
                throw new FunctionalException(ErrorType.NotFound, this.Translate("RecordNotFound"));

            return team;
        }

        public void Create(Team team, ITransactionalContext? context = null)
        {
            _teamValidator.SetTransactionalContext(context);

            var currentDate = _dateTimeHelper.GetCurrentTimestamp();
            
            team.CreationUser = base.GetContextUser();

            if (!_teamValidator.IsValid(team, Scenario.Create))
                throw new FunctionalException(ErrorType.ValidationError, _teamValidator.Errors);

            if (context == null)
                context = _teamRepository.GetTransactionalContext(TransactionContextScope.Internal);

            try
            {
                _teamRepository.Create(team, context);

                var teamMember = new TeamMember()
                {
                    IsActive = true,
                    JoinDate = currentDate,
                    Team = team,
                    User = team.CreationUser,
                    Role = new TeamMemberRole() { Id = (int)Enums.TeamMemberRole.Leader }
                };
                _teamMemberRepository.Add(teamMember, context);

                team.Members.Add(teamMember);

                context.Confirm();
            }
            catch (Exception)
            {
                if (context != null)
                    context.Cancel();

                throw;
            }

            _notificationHelper.SendNotification(Notification.Enums.NotificationType.Team_Creation, team);
        }

        public void Update(Team team, ITransactionalContext? context = null)
        {
            _teamValidator.SetTransactionalContext(context);

            if (!_teamValidator.IsValid(team, Scenario.Update))
                throw new FunctionalException(ErrorType.ValidationError, _teamValidator.Errors);

            if (context == null)
                context = _teamRepository.GetTransactionalContext(TransactionContextScope.Internal);

            try
            {
                _teamRepository.Update(team, context);

                _teamRepository.ConfirmTransactionalContext(context);
            }
            catch (Exception)
            {
                _teamRepository.CancelTransactionalContext(context);
                throw;
            }
        }

        public void Delete(int id, ITransactionalContext? context = null)
        {
            var team = this.Get(id, context);

            _teamValidator.SetTransactionalContext(context);

            if (!_teamValidator.IsValid(team, Scenario.Delete))
                throw new FunctionalException(ErrorType.ValidationError, _teamValidator.Errors);

            if (context == null)
                context = _teamRepository.GetTransactionalContext(TransactionContextScope.Internal);

            try
            {
                _teamRepository.Delete(team, context);

                _teamRepository.ConfirmTransactionalContext(context);
            }
            catch (Exception)
            {
                _teamRepository.CancelTransactionalContext(context);
                throw;
            }
        }

        #endregion
    }
}
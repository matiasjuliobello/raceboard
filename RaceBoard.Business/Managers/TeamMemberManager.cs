﻿using RaceBoard.Business.Helpers.Interfaces;
using RaceBoard.Business.Managers.Abstract;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Business.Validators;
using RaceBoard.Business.Validators.Interfaces;
using RaceBoard.Common.Enums;
using RaceBoard.Common.Exceptions;
using RaceBoard.Common.Helpers.Interfaces;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;
using RaceBoard.Translations.Interfaces;
using Enums = RaceBoard.Domain.Enums;

namespace RaceBoard.Business.Managers
{
    public class TeamMemberManager : AbstractManager, ITeamMemberManager
    {
        private readonly ITeamRepository _teamRepository;
        private readonly ITeamMemberRepository _teamMemberRepository;
        private readonly ITeamMemberRoleRepository _teamMemberRoleRepository;
        private readonly IPersonRepository _personRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICustomValidator<TeamMember> _teamMemberValidator;
        private readonly ICustomValidator<TeamMemberInvitation> _teamMemberInvitationValidator;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly IStringHelper _stringHelper;
        private readonly ICryptographyHelper _cryptographyHelper;
        private readonly IAuthorizationManager _authorizationManager;
        //private readonly IInvitationManager _invitationManager;
        private readonly INotificationHelper _notificationHelper;

        private const int _INVITATION_TOKEN_LENGTH = 32;

        #region Constructors

        public TeamMemberManager
            (
                ITeamRepository teamRepository,
                ITeamMemberRepository teamMemberRepository,
                ITeamMemberRoleRepository teamMembeRoleRepository,
                IPersonRepository personRepository,
                IUserRepository userRepository,
                IDateTimeHelper dateTimeHelper,
                IStringHelper stringHelper,
                ICryptographyHelper cryptographyHelper,
                //IMailManager mailManager,
                ICustomValidator<TeamMember> teamMemberValidator,
                ICustomValidator<TeamMemberInvitation> teamMemberInvitationValidator,
                IRequestContextManager requestContextManager,
                IAuthorizationManager authorizationManager,
                //IInvitationManager invitationManager,
                INotificationHelper notificationHelper,
                ITranslator translator

            ) : base(requestContextManager, translator)
        {
            _teamRepository = teamRepository;
            _teamMemberRepository = teamMemberRepository;
            _teamMemberRoleRepository = teamMembeRoleRepository;
            _personRepository = personRepository;
            _userRepository = userRepository;
            _dateTimeHelper = dateTimeHelper;
            _stringHelper = stringHelper;
            _cryptographyHelper = cryptographyHelper;
            _teamMemberValidator = teamMemberValidator;
            _teamMemberInvitationValidator = teamMemberInvitationValidator;
            _authorizationManager = authorizationManager;
            //_invitationManager = invitationManager;
            _notificationHelper = notificationHelper;
        }

        #endregion

        #region ITeamMemberManager implementation

        public PaginatedResult<TeamMember> Get(TeamMemberSearchFilter searchFilter, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            return _teamMemberRepository.Get(searchFilter, paginationFilter, sorting, context);
        }

        public TeamMember Get(int id, ITransactionalContext? context = null)
        {
            var searchFilter = new TeamMemberSearchFilter() { Ids = new int[] { id } };

            var teamMember = _teamMemberRepository.Get(searchFilter, paginationFilter: null, sorting: null, context).Results.FirstOrDefault();
            if (teamMember == null)
                throw new FunctionalException(ErrorType.NotFound, this.Translate("RecordNotFound"));

            return teamMember;
        }

        public PaginatedResult<TeamMemberInvitation> GetMemberInvitations(int idTeam, bool isPending, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            var searchFilter = new TeamMemberInvitationSearchFilter()
            {
                Team = new Team() { Id = idTeam },
                IsPending = isPending
            };

            return _teamMemberRepository.GetInvitations(searchFilter, paginationFilter, sorting, context);
        }

        public TeamMemberInvitation GetInvitation(int id, ITransactionalContext? context = null)
        {
            var searchFilter = new TeamMemberInvitationSearchFilter() { Ids = new int[] { id } };

            var teamMemberInvitation = _teamMemberRepository.GetInvitations(searchFilter, paginationFilter: null, sorting: null, context).Results.FirstOrDefault();
            if (teamMemberInvitation == null)
                throw new FunctionalException(ErrorType.NotFound, this.Translate("RecordNotFound"));

            return teamMemberInvitation;
        }

        public void CreateInvitation(TeamMemberInvitation teamMemberInvitation, ITransactionalContext? context = null)
        {
            var contextUser = base.GetContextUser();

            teamMemberInvitation.RequestUser = contextUser;

            _authorizationManager.ValidatePermission(contextUser.Id, Enums.Action.TeamMember_Create, teamMemberInvitation.Team.Id);

            if (context == null)
                context = _teamMemberRepository.GetTransactionalContext(TransactionContextScope.Internal);

            _teamMemberInvitationValidator.SetTransactionalContext(context);
            if (!_teamMemberInvitationValidator.IsValid(teamMemberInvitation, Scenario.Create))
                throw new FunctionalException(ErrorType.ValidationError, _teamMemberInvitationValidator.Errors);

            teamMemberInvitation.RequestDate = _dateTimeHelper.GetCurrentTimestamp();
            teamMemberInvitation.Invitation.Token = this.GenerateInvitationToken();

            if (teamMemberInvitation.User != null)
                teamMemberInvitation.Invitation.EmailAddress = _userRepository.GetById(teamMemberInvitation.User.Id).Email;

            try
            {
                _teamMemberRepository.CreateInvitation(teamMemberInvitation, context);

                //_invitationManager.SendTeamInvitation(teamMemberInvitation);

                context.Confirm();
            }
            catch (Exception)
            {
                if (context != null)
                    context.Cancel();

                throw;
            }

            _notificationHelper.SendNotification(Notification.Enums.NotificationType.Team_Member_Invitation, teamMemberInvitation);
        }

        public void UpdateInvitation(TeamMemberInvitation teamMemberInvitation, ITransactionalContext? context = null)
        {
            var contextUser = base.GetContextUser();

            teamMemberInvitation.User = contextUser;

            //_authorizationManager.ValidatePermission(Enums.Action.TeamMember_Update, teamMemberInvitation.Team.Id, contextUser.Id);

            var invitations = _teamMemberRepository.GetInvitations(new TeamMemberInvitationSearchFilter() { Token = teamMemberInvitation.Invitation.Token });
            if (invitations.Results.Count() == 0)
                throw new FunctionalException(ErrorType.NotFound, this.Translate("RecordNotFound"));

            var invitation = invitations.Results.First();
            if (invitation.Invitation.IsExpired)
                throw new FunctionalException(ErrorType.NotFound, this.Translate("InvitationExpired"));

            if (invitation.Invitation.EmailAddress != contextUser.Email)
                throw new FunctionalException(ErrorType.Forbidden, this.Translate("InvitationUnauthorized"));

            if (context == null)
                context = _teamMemberRepository.GetTransactionalContext(TransactionContextScope.Internal);

            _teamMemberInvitationValidator.SetTransactionalContext(context);
            if (!_teamMemberInvitationValidator.IsValid(teamMemberInvitation, Scenario.Update))
                throw new FunctionalException(ErrorType.ValidationError, _teamMemberInvitationValidator.Errors);

            var currentDate = _dateTimeHelper.GetCurrentTimestamp();

            try
            {
                var teamMember = new TeamMember()
                {
                    IsActive = true,
                    JoinDate = currentDate,
                    Team = invitation.Team,
                    Role = invitation.Role,
                    User = teamMemberInvitation.User!
                };
                _teamMemberRepository.Add(teamMember, context);

                invitation.User = teamMemberInvitation.User;
                _teamMemberRepository.UpdateInvitation(invitation, context);

                context.Confirm();
            }
            catch (Exception)
            {
                if (context != null)
                    context.Cancel();

                throw;
            }
        }

        public void Remove(int id, ITransactionalContext? context = null)
        {
            var teamMember = this.Get(id, context);

            var contextUser = base.GetContextUser();
            _authorizationManager.ValidatePermission(contextUser.Id, Enums.Action.TeamMember_Delete, teamMember.Team.Id);

            if (context == null)
                context = _teamMemberRepository.GetTransactionalContext(TransactionContextScope.Internal);

            _teamMemberValidator.SetTransactionalContext(context);

            if (!_teamMemberValidator.IsValid(teamMember, Scenario.Delete))
                throw new FunctionalException(ErrorType.ValidationError, _teamMemberValidator.Errors);

            try
            {
                _teamMemberRepository.Remove(teamMember.Id, context);

                var searchFilter = new TeamMemberInvitationSearchFilter()
                {
                    Team = teamMember.Team,
                    Role = teamMember.Role,
                    User = teamMember.User
                };
                var invitation = _teamMemberRepository.GetInvitations(searchFilter, paginationFilter: null, sorting: null, context).Results.FirstOrDefault();
                if (invitation != null)
                    _teamMemberRepository.RemoveInvitation(invitation, context);

                context.Confirm();
            }
            catch (Exception)
            {
                if (context != null)
                    context.Cancel();

                throw;
            }
        }

        public void RemoveInvitation(int id, ITransactionalContext? context = null)
        {
            var teamMemberInvitation = this.GetInvitation(id, context);

            var contextUser = base.GetContextUser();
            _authorizationManager.ValidatePermission(contextUser.Id, Enums.Action.TeamMember_Delete, teamMemberInvitation.Team.Id);

            if (context == null)
                context = _teamMemberRepository.GetTransactionalContext(TransactionContextScope.Internal);

            _teamMemberInvitationValidator.SetTransactionalContext(context);

            if (!_teamMemberInvitationValidator.IsValid(teamMemberInvitation, Scenario.Delete))
                throw new FunctionalException(ErrorType.ValidationError, _teamMemberInvitationValidator.Errors);

            try
            {
                _teamMemberRepository.RemoveInvitation(teamMemberInvitation, context);

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

        private string GenerateInvitationToken()
        {
            string randomString = _stringHelper.GenerateRandomString(_INVITATION_TOKEN_LENGTH);
            string randomStringHash = _cryptographyHelper.ComputeHash(randomString);

            return randomStringHash;
        }

        #endregion
    }
}

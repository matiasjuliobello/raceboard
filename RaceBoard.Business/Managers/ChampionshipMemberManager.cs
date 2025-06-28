using RaceBoard.Business.Helpers.Interfaces;
using RaceBoard.Business.Managers.Abstract;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Business.Validators.Interfaces;
using RaceBoard.Common;
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
    public class ChampionshipMemberManager : AbstractManager, IChampionshipMemberManager
    {
        private readonly IChampionshipMemberRepository _championshipMemberRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICustomValidator<ChampionshipMember> _championshipMemberValidator;
        private readonly ICustomValidator<ChampionshipMemberInvitation> _championshipMemberInvitationValidator;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly IStringHelper _stringHelper;
        private readonly ICryptographyHelper _cryptographyHelper;
        private readonly IAuthorizationManager _authorizationManager;
        private readonly INotificationHelper _notificationHelper;

        private const int _INVITATION_TOKEN_LENGTH = 32;

        #region Constructors

        public ChampionshipMemberManager
            (
                IChampionshipMemberRepository championshipMemberRepository,
                IUserRepository userRepository,
                ICustomValidator<ChampionshipMember> championshipMemberValidator,
                ICustomValidator<ChampionshipMemberInvitation> championshipMemberInvitationValidator,
                ITranslator translator,
                IDateTimeHelper dateTimeHelper,
                IStringHelper stringHelper,
                ICryptographyHelper cryptographyHelper,
                IRequestContextManager requestContextManager,
                IAuthorizationManager authorizationManager,
                INotificationHelper notificationHelper
            ) : base(requestContextManager, translator)
        {
            _championshipMemberRepository = championshipMemberRepository;
            _userRepository = userRepository;
            _championshipMemberValidator = championshipMemberValidator;
            _championshipMemberInvitationValidator = championshipMemberInvitationValidator;
            _dateTimeHelper = dateTimeHelper;
            _stringHelper = stringHelper;
            _cryptographyHelper = cryptographyHelper;
            _authorizationManager = authorizationManager;
            _notificationHelper = notificationHelper;
        }

        #endregion

        #region IChampionshipMemberManager implementation

        public PaginatedResult<ChampionshipMember> Get(ChampionshipMemberSearchFilter searchFilter, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            return _championshipMemberRepository.Get(searchFilter, paginationFilter, sorting, context);
        }

        public ChampionshipMember Get(int id, ITransactionalContext? context = null)
        {
            var searchFilter = new ChampionshipMemberSearchFilter() { Ids = new int[] { id } };

            var championshipMember = _championshipMemberRepository.Get(searchFilter, paginationFilter: null, sorting: null, context).Results.FirstOrDefault();
            if (championshipMember == null)
                throw new FunctionalException(ErrorType.NotFound, this.Translate("RecordNotFound"));

            return championshipMember;
        }

        public PaginatedResult<ChampionshipMemberInvitation> GetMemberInvitations(int idChampionship, bool isPending, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            var searchFilter = new ChampionshipMemberInvitationSearchFilter()
            {
                Championship = new Championship() { Id = idChampionship },
                IsPending = isPending
            };

            return _championshipMemberRepository.GetInvitations(searchFilter, paginationFilter, sorting, context);
        }

        public ChampionshipMemberInvitation GetInvitation(int id, ITransactionalContext? context = null)
        {
            var searchFilter = new ChampionshipMemberInvitationSearchFilter() { Ids = new int[] { id } };

            var championshipMemberInvitation = _championshipMemberRepository.GetInvitations(searchFilter, paginationFilter: null, sorting: null, context).Results.FirstOrDefault();
            if (championshipMemberInvitation == null)
                throw new FunctionalException(ErrorType.NotFound, this.Translate("RecordNotFound"));

            return championshipMemberInvitation;
        }

        public void CreateInvitation(ChampionshipMemberInvitation championshipMemberInvitation, ITransactionalContext? context = null)
        {
            var contextUser = base.GetContextUser();

            championshipMemberInvitation.RequestUser = contextUser;

            _authorizationManager.ValidatePermission(contextUser.Id, Enums.Action.ChampionshipMember_Create, championshipMemberInvitation.Championship.Id);

            if (context == null)
                context = _championshipMemberRepository.GetTransactionalContext(TransactionContextScope.Internal);

            _championshipMemberInvitationValidator.SetTransactionalContext(context);
            if (!_championshipMemberInvitationValidator.IsValid(championshipMemberInvitation, Scenario.Create))
                throw new FunctionalException(ErrorType.ValidationError, _championshipMemberInvitationValidator.Errors);

            championshipMemberInvitation.RequestDate = _dateTimeHelper.GetCurrentTimestamp();
            championshipMemberInvitation.Invitation.Token = this.GenerateInvitationToken();

            if (championshipMemberInvitation.User != null)
                championshipMemberInvitation.Invitation.EmailAddress = _userRepository.GetById(championshipMemberInvitation.User.Id).Email;

            try
            {
                _championshipMemberRepository.CreateInvitation(championshipMemberInvitation, context);

                _notificationHelper.SendNotification(Notification.Enums.NotificationType.Championship_Member_Invitation, championshipMemberInvitation);

                context.Confirm();
            }
            catch (Exception)
            {
                if (context != null)
                    context.Cancel();

                throw;
            }
        }

        public void UpdateInvitation(ChampionshipMemberInvitation championshipMemberInvitation, ITransactionalContext? context = null)
        {
            var contextUser = base.GetContextUser();

            championshipMemberInvitation.User = contextUser;
            //_authorizationManager.ValidatePermission(Enums.Action.ChampionshipMember_Update, championshipMemberInvitation.Championship.Id, contextUser.Id);

            var invitations = _championshipMemberRepository.GetInvitations(new ChampionshipMemberInvitationSearchFilter() { Token = championshipMemberInvitation.Invitation.Token });
            if (invitations.Results.Count() == 0)
                throw new FunctionalException(ErrorType.NotFound, this.Translate("RecordNotFound"));

            var invitation = invitations.Results.First();
            if (invitation.Invitation.IsExpired)
                throw new FunctionalException(ErrorType.NotFound, this.Translate("InvitationExpired"));

            if (invitation.Invitation.EmailAddress != contextUser.Email)
                throw new FunctionalException(ErrorType.Forbidden, this.Translate("InvitationUnauthorized"));

            if (context == null)
                context = _championshipMemberRepository.GetTransactionalContext(TransactionContextScope.Internal);

            _championshipMemberInvitationValidator.SetTransactionalContext(context);
            if (!_championshipMemberInvitationValidator.IsValid(championshipMemberInvitation, Scenario.Update))
                throw new FunctionalException(ErrorType.ValidationError, _championshipMemberInvitationValidator.Errors);

            var currentDate = _dateTimeHelper.GetCurrentTimestamp();

            try
            {
                var championshipMember = new ChampionshipMember()
                {
                    IsActive = true,
                    JoinDate = currentDate,
                    Championship = invitation.Championship,
                    Role = invitation.Role,
                    User = championshipMemberInvitation.User!
                };
                _championshipMemberRepository.Add(championshipMember, context);

                invitation.User = championshipMemberInvitation.User;
                _championshipMemberRepository.UpdateInvitation(invitation, context);

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
            var championshipMember = this.Get(id, context);

            var contextUser = base.GetContextUser();
            _authorizationManager.ValidatePermission(contextUser.Id, Enums.Action.ChampionshipMember_Delete, championshipMember.Championship.Id);

            if (context == null)
                context = _championshipMemberRepository.GetTransactionalContext(TransactionContextScope.Internal);

            _championshipMemberValidator.SetTransactionalContext(context);

            if (!_championshipMemberValidator.IsValid(championshipMember, Scenario.Delete))
                throw new FunctionalException(ErrorType.ValidationError, _championshipMemberValidator.Errors);

            try
            {
                _championshipMemberRepository.Remove(championshipMember.Id, context);

                var searchFilter = new ChampionshipMemberInvitationSearchFilter()
                {
                    Championship = championshipMember.Championship,
                    Role = championshipMember.Role,
                    User = championshipMember.User
                };
                var invitation = _championshipMemberRepository.GetInvitations(searchFilter, paginationFilter: null, sorting: null, context).Results.FirstOrDefault();
                if (invitation != null)
                    _championshipMemberRepository.RemoveInvitation(invitation, context);

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
            var championshipMemberInvitation = this.GetInvitation(id, context);

            var contextUser = base.GetContextUser();
            _authorizationManager.ValidatePermission(contextUser.Id, Enums.Action.ChampionshipMember_Delete, championshipMemberInvitation.Championship.Id);

            if (context == null)
                context = _championshipMemberRepository.GetTransactionalContext(TransactionContextScope.Internal);

            _championshipMemberInvitationValidator.SetTransactionalContext(context);

            if (!_championshipMemberInvitationValidator.IsValid(championshipMemberInvitation, Scenario.Delete))
                throw new FunctionalException(ErrorType.ValidationError, _championshipMemberInvitationValidator.Errors);

            try
            {
                _championshipMemberRepository.RemoveInvitation(championshipMemberInvitation, context);

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

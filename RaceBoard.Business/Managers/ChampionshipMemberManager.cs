using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Configuration;
using RaceBoard.Business.Managers.Abstract;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Business.Validators;
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

namespace RaceBoard.Business.Managers
{
    public class ChampionshipMemberManager : AbstractManager, IChampionshipMemberManager
    {
        private readonly IChampionshipMemberRepository _championshipMemberRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPersonRepository _personRepository;
        private readonly IChampionshipRepository _championshipRepository;
        private readonly ICustomValidator<ChampionshipMember> _championshipMemberValidator;
        private readonly ICustomValidator<ChampionshipMemberInvitation> _championshipMemberInvitationValidator;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly IStringHelper _stringHelper;
        private readonly ICryptographyHelper _cryptographyHelper;
        private readonly IMailManager _mailManager;

        private const int _INVITATION_TOKEN_LENGTH = 32;

        #region Constructors

        public ChampionshipMemberManager
            (
                IChampionshipMemberRepository championshipMemberRepository,
                IUserRepository userRepository,
                IPersonRepository personRepository,
                IChampionshipRepository championshipRepository,
                ICustomValidator<ChampionshipMember> championshipMemberValidator,
                ICustomValidator<ChampionshipMemberInvitation> championshipMemberInvitationValidator,
                ITranslator translator,
                IDateTimeHelper dateTimeHelper,
                IStringHelper stringHelper,
                ICryptographyHelper cryptographyHelper,
                IMailManager mailManager
            ) : base(translator)
        {
            _championshipMemberRepository = championshipMemberRepository;
            _userRepository = userRepository;
            _personRepository = personRepository;
            _championshipRepository = championshipRepository;
            _championshipMemberValidator = championshipMemberValidator;
            _championshipMemberInvitationValidator = championshipMemberInvitationValidator;
            _dateTimeHelper = dateTimeHelper;
            _stringHelper = stringHelper;
            _cryptographyHelper = cryptographyHelper;
            _mailManager = mailManager;
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
                Championship = new Championship() {  Id = idChampionship },
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

        public void AddInvitation(ChampionshipMemberInvitation championshipMemberInvitation, ITransactionalContext? context = null)
        {
            if (context == null)
                context = _championshipMemberRepository.GetTransactionalContext(TransactionContextScope.Internal);

            _championshipMemberInvitationValidator.SetTransactionalContext(context);
            if (!_championshipMemberInvitationValidator.IsValid(championshipMemberInvitation, Scenario.Create))
                throw new FunctionalException(ErrorType.ValidationError, _championshipMemberInvitationValidator.Errors);

            championshipMemberInvitation.RequestDate = _dateTimeHelper.GetCurrentTimestamp();
            championshipMemberInvitation.Invitation.Token = this.GenerateInvitationToken();

            if (championshipMemberInvitation.User != null)
                championshipMemberInvitation.Invitation.EmailAddress = _userRepository.GetById(championshipMemberInvitation.User.Id).Email;

            var requestUser = _personRepository.GetByIdUser(championshipMemberInvitation.RequestUser.Id);
            var championship = _championshipRepository.Get(championshipMemberInvitation.Championship.Id)!;

            try
            {
                _championshipMemberRepository.CreateInvitation(championshipMemberInvitation, context);

                string emailSubject = $"You've invited to join '{championship.Name}'";
                string emailBody = $"You've invited by {requestUser.Fullname} to join '{championship.Name}', to perform as {championshipMemberInvitation.Role.Name}";
                string emailRecipientAddress = championshipMemberInvitation.Invitation.EmailAddress;
                string emailRecipientName = championshipMemberInvitation.Invitation.EmailAddress;

                _mailManager.SendMail(emailSubject, emailBody, emailRecipientAddress, emailRecipientName);

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
            var invitations = _championshipMemberRepository.GetInvitations(new ChampionshipMemberInvitationSearchFilter() { Token = championshipMemberInvitation.Invitation.Token });
            if (invitations.Results.Count() == 0)
                throw new FunctionalException(ErrorType.NotFound, this.Translate("RecordNotFound"));

            var invitation = invitations.Results.First();

            if (invitation.Invitation.IsExpired)
                throw new FunctionalException(ErrorType.NotFound, this.Translate("InvitationExpired"));

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
            if (context == null)
                context = _championshipMemberRepository.GetTransactionalContext(TransactionContextScope.Internal);

            var championshipMember = this.Get(id, context);

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
            if (context == null)
                context = _championshipMemberRepository.GetTransactionalContext(TransactionContextScope.Internal);

            var championshipMemberInvitation = this.GetInvitation(id, context);

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

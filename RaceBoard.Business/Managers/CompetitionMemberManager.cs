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
    public class CompetitionMemberManager : AbstractManager, ICompetitionMemberManager
    {
        private readonly ICompetitionMemberRepository _competitionMemberRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPersonRepository _personRepository;
        private readonly ICompetitionRepository _competitionRepository;
        private readonly ICustomValidator<CompetitionMember> _competitionMemberValidator;
        private readonly ICustomValidator<CompetitionMemberInvitation> _competitionMemberInvitationValidator;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly IStringHelper _stringHelper;
        private readonly ICryptographyHelper _cryptographyHelper;
        private readonly IMailManager _mailManager;

        private const int _INVITATION_TOKEN_LENGTH = 32;

        #region Constructors

        public CompetitionMemberManager
            (
                ICompetitionMemberRepository competitionMemberRepository,
                IUserRepository userRepository,
                IPersonRepository personRepository,
                ICompetitionRepository competitionRepository,
                ICustomValidator<CompetitionMember> competitionMemberValidator,
                ICustomValidator<CompetitionMemberInvitation> competitionMemberInvitationValidator,
                ITranslator translator,
                IDateTimeHelper dateTimeHelper,
                IStringHelper stringHelper,
                ICryptographyHelper cryptographyHelper,
                IMailManager mailManager
            ) : base(translator)
        {
            _competitionMemberRepository = competitionMemberRepository;
            _userRepository = userRepository;
            _personRepository = personRepository;
            _competitionRepository = competitionRepository;
            _competitionMemberValidator = competitionMemberValidator;
            _competitionMemberInvitationValidator = competitionMemberInvitationValidator;
            _dateTimeHelper = dateTimeHelper;
            _stringHelper = stringHelper;
            _cryptographyHelper = cryptographyHelper;
            _mailManager = mailManager;
        }

        #endregion

        #region ICompetitionMemberManager implementation

        public PaginatedResult<CompetitionMember> Get(CompetitionMemberSearchFilter searchFilter, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            return _competitionMemberRepository.Get(searchFilter, paginationFilter, sorting, context);
        }

        public CompetitionMember Get(int id, ITransactionalContext? context = null)
        {
            var searchFilter = new CompetitionMemberSearchFilter() { Ids = new int[] { id } };

            var competitionMember = _competitionMemberRepository.Get(searchFilter, paginationFilter: null, sorting: null, context).Results.FirstOrDefault();
            if (competitionMember == null)
                throw new FunctionalException(ErrorType.NotFound, this.Translate("RecordNotFound"));

            return competitionMember;
        }

        public PaginatedResult<CompetitionMemberInvitation> GetMemberInvitations(int idCompetition, bool isPending, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            var searchFilter = new CompetitionMemberInvitationSearchFilter() { IdCompetition = idCompetition, IsPending = isPending };

            return _competitionMemberRepository.GetInvitations(searchFilter, paginationFilter, sorting, context);
        }

        public CompetitionMemberInvitation GetInvitation(int id, ITransactionalContext? context = null)
        {
            var searchFilter = new CompetitionMemberInvitationSearchFilter() { Ids = new int[] { id } };

            var competitionMemberInvitation = _competitionMemberRepository.GetInvitations(searchFilter, paginationFilter: null, sorting: null, context).Results.FirstOrDefault();
            if (competitionMemberInvitation == null)
                throw new FunctionalException(ErrorType.NotFound, this.Translate("RecordNotFound"));

            return competitionMemberInvitation;
        }

        public void AddInvitation(CompetitionMemberInvitation competitionMemberInvitation, ITransactionalContext? context = null)
        {
            if (context == null)
                context = _competitionMemberRepository.GetTransactionalContext(TransactionContextScope.Internal);

            _competitionMemberInvitationValidator.SetTransactionalContext(context);
            if (!_competitionMemberInvitationValidator.IsValid(competitionMemberInvitation, Scenario.Create))
                throw new FunctionalException(ErrorType.ValidationError, _competitionMemberInvitationValidator.Errors);

            competitionMemberInvitation.RequestDate = _dateTimeHelper.GetCurrentTimestamp();
            competitionMemberInvitation.Invitation.Token = this.GenerateInvitationToken();

            if (competitionMemberInvitation.User != null)
                competitionMemberInvitation.Invitation.EmailAddress = _userRepository.GetById(competitionMemberInvitation.User.Id).Email;

            var requestUser = _personRepository.GetByIdUser(competitionMemberInvitation.RequestUser.Id);
            var competition = _competitionRepository.Get(competitionMemberInvitation.Competition.Id)!;

            try
            {
                _competitionMemberRepository.CreateInvitation(competitionMemberInvitation, context);

                string emailSubject = $"You've invited to join '{competition.Name}'";
                string emailBody = $"You've invited by {requestUser.Fullname} to join '{competition.Name}', to perform as {competitionMemberInvitation.Role.Name}";
                string emailRecipientAddress = competitionMemberInvitation.Invitation.EmailAddress;
                string emailRecipientName = competitionMemberInvitation.Invitation.EmailAddress;

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

        public void UpdateInvitation(CompetitionMemberInvitation competitionMemberInvitation, ITransactionalContext? context = null)
        {
            var invitations = _competitionMemberRepository.GetInvitations(new CompetitionMemberInvitationSearchFilter() { Token = competitionMemberInvitation.Invitation.Token });
            if (invitations.Results.Count() == 0)
                throw new FunctionalException(ErrorType.NotFound, this.Translate("RecordNotFound"));

            var invitation = invitations.Results.First();

            if (invitation.Invitation.IsExpired)
                throw new FunctionalException(ErrorType.NotFound, this.Translate("InvitationExpired"));

            if (context == null)
                context = _competitionMemberRepository.GetTransactionalContext(TransactionContextScope.Internal);

            _competitionMemberInvitationValidator.SetTransactionalContext(context);
            if (!_competitionMemberInvitationValidator.IsValid(competitionMemberInvitation, Scenario.Update))
                throw new FunctionalException(ErrorType.ValidationError, _competitionMemberInvitationValidator.Errors);

            var currentDate = _dateTimeHelper.GetCurrentTimestamp();

            try
            {
                var competitionMember = new CompetitionMember()
                {
                    IsActive = true,
                    JoinDate = currentDate,
                    Competition = invitation.Competition,
                    Role = invitation.Role,
                    User = competitionMemberInvitation.User!
                };
                _competitionMemberRepository.Add(competitionMember, context);

                invitation.User = competitionMemberInvitation.User;
                _competitionMemberRepository.UpdateInvitation(invitation, context);

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
                context = _competitionMemberRepository.GetTransactionalContext(TransactionContextScope.Internal);

            var competitionMember = this.Get(id, context);

            _competitionMemberValidator.SetTransactionalContext(context);

            if (!_competitionMemberValidator.IsValid(competitionMember, Scenario.Delete))
                throw new FunctionalException(ErrorType.ValidationError, _competitionMemberValidator.Errors);

            try
            {
                _competitionMemberRepository.Remove(competitionMember.Id, context);

                var searchFilter = new CompetitionMemberInvitationSearchFilter()
                {
                    IdCompetition = competitionMember.Competition.Id,
                    IdRole = competitionMember.Role.Id,
                    IdUser = competitionMember.User.Id
                };
                var invitation = _competitionMemberRepository.GetInvitations(searchFilter, paginationFilter: null, sorting: null, context).Results.FirstOrDefault();
                if (invitation != null)
                    _competitionMemberRepository.RemoveInvitation(invitation, context);

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
                context = _competitionMemberRepository.GetTransactionalContext(TransactionContextScope.Internal);

            var competitionMemberInvitation = this.GetInvitation(id, context);

            _competitionMemberInvitationValidator.SetTransactionalContext(context);

            if (!_competitionMemberInvitationValidator.IsValid(competitionMemberInvitation, Scenario.Delete))
                throw new FunctionalException(ErrorType.ValidationError, _competitionMemberInvitationValidator.Errors);

            try
            {
                _competitionMemberRepository.RemoveInvitation(competitionMemberInvitation, context);

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

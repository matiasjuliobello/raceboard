using RaceBoard.Business.Managers.Abstract;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Business.Validators.Interfaces;
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
    public class TeamMemberManager : AbstractManager, ITeamMemberManager
    {
        private readonly ITeamMemberRepository _teamMemberRepository;
        private readonly IPersonRepository _personRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICustomValidator<TeamMember> _teamMemberValidator;
        private readonly ICustomValidator<TeamMemberInvitation> _teamMemberInvitationValidator;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly IStringHelper _stringHelper;
        private readonly ICryptographyHelper _cryptographyHelper;
        private readonly IMailManager _mailManager;

        private const int _INVITATION_TOKEN_LENGTH = 32;

        #region Constructors

        public TeamMemberManager
            (
                ITeamMemberRepository teamMemberRepository,
                IPersonRepository personRepository,
                IUserRepository userRepository,
                IDateTimeHelper dateTimeHelper,
                IStringHelper stringHelper,
                ICryptographyHelper cryptographyHelper,
                IMailManager mailManager,
                ICustomValidator<TeamMember> teamMemberValidator,
                ICustomValidator<TeamMemberInvitation> teamMemberInvitationValidator,
                ITranslator translator
                
            ) : base(translator)
        {
            _teamMemberRepository = teamMemberRepository;
            _personRepository = personRepository;
            _userRepository = userRepository;
            _dateTimeHelper = dateTimeHelper;
            _stringHelper = stringHelper;
            _cryptographyHelper = cryptographyHelper;
            _mailManager = mailManager;
            _teamMemberValidator = teamMemberValidator;
            _teamMemberInvitationValidator = teamMemberInvitationValidator;
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

        //public void Create(TeamMember teamMember, ITransactionalContext? context = null)
        //{
        //    _teamMemberValidator.SetTransactionalContext(context);

        //    if (!_teamMemberValidator.IsValid(teamMember, Scenario.Create))
        //        throw new FunctionalException(ErrorType.ValidationError, _teamMemberValidator.Errors);

        //    if (context == null)
        //        context = _teamMemberRepository.GetTransactionalContext(TransactionContextScope.Internal);

        //    try
        //    {
        //        _teamMemberRepository.Create(teamMember, context);

        //        _teamMemberRepository.ConfirmTransactionalContext(context);
        //    }
        //    catch (Exception)
        //    {
        //        _teamMemberRepository.CancelTransactionalContext(context);
        //        throw;
        //    }
        //}
        public void AddInvitation(TeamMemberInvitation teamMemberInvitation, ITransactionalContext? context = null)
        {
            if (context == null)
                context = _teamMemberRepository.GetTransactionalContext(TransactionContextScope.Internal);

            _teamMemberInvitationValidator.SetTransactionalContext(context);
            if (!_teamMemberInvitationValidator.IsValid(teamMemberInvitation, Scenario.Create))
                throw new FunctionalException(ErrorType.ValidationError, _teamMemberInvitationValidator.Errors);

            teamMemberInvitation.RequestDate = _dateTimeHelper.GetCurrentTimestamp();
            teamMemberInvitation.Invitation.Token = this.GenerateInvitationToken();

            if (teamMemberInvitation.User != null)
                teamMemberInvitation.Invitation.EmailAddress = _userRepository.GetById(teamMemberInvitation.User.Id).Email;

            var requestUser = _personRepository.GetByIdUser(teamMemberInvitation.RequestUser.Id);

            try
            {
                _teamMemberRepository.CreateInvitation(teamMemberInvitation, context);

                string emailSubject = $"You've invited to join a team";
                string emailBody = $"You've invited by {requestUser.Fullname} to perform as {teamMemberInvitation.Role.Name} in a team";
                string emailRecipientAddress = teamMemberInvitation.Invitation.EmailAddress;
                string emailRecipientName = teamMemberInvitation.Invitation.EmailAddress;

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

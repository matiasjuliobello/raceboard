using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Configuration;
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

namespace RaceBoard.Business.Managers
{
    public class OrganizationMemberManager : AbstractManager, IOrganizationMemberManager
    {
        private readonly IOrganizationMemberRepository _organizationMemberRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPersonRepository _personRepository;
        private readonly IOrganizationRepository _organizationRepository;
        private readonly ICustomValidator<OrganizationMember> _organizationMemberValidator;
        private readonly ICustomValidator<OrganizationMemberInvitation> _organizationMemberInvitationValidator;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly IStringHelper _stringHelper;
        private readonly ICryptographyHelper _cryptographyHelper;
        private readonly IConfiguration _configuration;
        private readonly IMailManager _mailManager;

        private const int _INVITATION_TOKEN_LENGTH = 32;

        #region Constructors

        public OrganizationMemberManager
            (
                IOrganizationMemberRepository organizationMemberRepository,
                IUserRepository userRepository,
                IPersonRepository personRepository,
                IOrganizationRepository organizationRepository,
                ICustomValidator<OrganizationMember> organizationMemberValidator,
                ICustomValidator<OrganizationMemberInvitation> organizationMemberInvitationValidator,
                ITranslator translator,
                IDateTimeHelper dateTimeHelper,
                IStringHelper stringHelper,
                ICryptographyHelper cryptographyHelper,
                IConfiguration configuration,
                IMailManager mailManager
            ) : base(translator)
        {
            _organizationMemberRepository = organizationMemberRepository;
            _userRepository = userRepository;
            _personRepository = personRepository;
            _organizationRepository = organizationRepository;
            _organizationMemberValidator = organizationMemberValidator;
            _organizationMemberInvitationValidator = organizationMemberInvitationValidator;
            _dateTimeHelper = dateTimeHelper;
            _stringHelper = stringHelper;
            _cryptographyHelper = cryptographyHelper;
            _configuration = configuration;
            _mailManager = mailManager;
        }

        #endregion

        #region IOrganizationMemberManager implementation

        public PaginatedResult<OrganizationMember> Get(OrganizationMemberSearchFilter searchFilter, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            return _organizationMemberRepository.Get(searchFilter, paginationFilter, sorting, context);
        }

        public OrganizationMember Get(int id, ITransactionalContext? context = null)
        {
            var searchFilter = new OrganizationMemberSearchFilter() { Ids = new int[] { id } };

            var organizationMember = _organizationMemberRepository.Get(searchFilter, paginationFilter: null, sorting: null, context).Results.FirstOrDefault();
            if (organizationMember == null)
                throw new FunctionalException(ErrorType.NotFound, this.Translate("RecordNotFound"));

            return organizationMember;
        }

        public PaginatedResult<OrganizationMemberInvitation> GetMemberInvitations(int idOrganization, bool isPending, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            var searchFilter = new OrganizationMemberInvitationSearchFilter() { IdOrganization = idOrganization, IsPending = isPending };

            return _organizationMemberRepository.GetInvitations(searchFilter, paginationFilter, sorting, context);
        }

        public OrganizationMemberInvitation GetInvitation(int id, ITransactionalContext? context = null)
        {
            var searchFilter = new OrganizationMemberInvitationSearchFilter() { Ids = new int[] { id } };

            var organizationMemberInvitation = _organizationMemberRepository.GetInvitations(searchFilter, paginationFilter: null, sorting: null, context).Results.FirstOrDefault();
            if (organizationMemberInvitation == null)
                throw new FunctionalException(ErrorType.NotFound, this.Translate("RecordNotFound"));

            return organizationMemberInvitation;
        }

        public void AddInvitation(OrganizationMemberInvitation organizationMemberInvitation, ITransactionalContext? context = null)
        {
            if (context == null)
                context = _organizationMemberRepository.GetTransactionalContext(TransactionContextScope.Internal);

            _organizationMemberInvitationValidator.SetTransactionalContext(context);
            if (!_organizationMemberInvitationValidator.IsValid(organizationMemberInvitation, Scenario.Create))
                throw new FunctionalException(ErrorType.ValidationError, _organizationMemberInvitationValidator.Errors);

            organizationMemberInvitation.RequestDate = _dateTimeHelper.GetCurrentTimestamp();
            organizationMemberInvitation.Invitation.Token = this.GenerateInvitationToken();

            if (organizationMemberInvitation.User != null)
                organizationMemberInvitation.Invitation.EmailAddress = _userRepository.GetById(organizationMemberInvitation.User.Id).Email;

            var requestUser = _personRepository.GetByIdUser(organizationMemberInvitation.RequestUser.Id);
            var organization = _organizationRepository.Get(organizationMemberInvitation.Organization.Id)!;

            try
            {
                _organizationMemberRepository.CreateInvitation(organizationMemberInvitation, context);

                string emailSubject = $"You've invited to join '{organization.Name}'";
                string emailBody = $"You've invited by {requestUser.Fullname} to join '{organization.Name}', to perform as {organizationMemberInvitation.Role.Name}";
                string emailRecipientAddress = organizationMemberInvitation.Invitation.EmailAddress;
                string emailRecipientName = organizationMemberInvitation.Invitation.EmailAddress;

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

        public void UpdateInvitation(OrganizationMemberInvitation organizationMemberInvitation, ITransactionalContext? context = null)
        {
            var invitations = _organizationMemberRepository.GetInvitations(new OrganizationMemberInvitationSearchFilter() { Token = organizationMemberInvitation.Invitation.Token });
            if (invitations.Results.Count() == 0)
                throw new FunctionalException(ErrorType.NotFound, this.Translate("RecordNotFound"));

            var invitation = invitations.Results.First();

            if (invitation.Invitation.IsExpired)
                throw new FunctionalException(ErrorType.NotFound, this.Translate("InvitationExpired"));

            if (context == null)
                context = _organizationMemberRepository.GetTransactionalContext(TransactionContextScope.Internal);

            _organizationMemberInvitationValidator.SetTransactionalContext(context);
            if (!_organizationMemberInvitationValidator.IsValid(organizationMemberInvitation, Scenario.Update))
                throw new FunctionalException(ErrorType.ValidationError, _organizationMemberInvitationValidator.Errors);

            var currentDate = _dateTimeHelper.GetCurrentTimestamp();

            try
            {
                var organizationMember = new OrganizationMember()
                {
                    IsActive = true,
                    JoinDate = currentDate,
                    Organization = invitation.Organization,
                    Role = invitation.Role,
                    User = organizationMemberInvitation.User!
                };
                _organizationMemberRepository.Add(organizationMember, context);
                
                invitation.User = organizationMemberInvitation.User;
                _organizationMemberRepository.UpdateInvitation(invitation, context);

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
                context = _organizationMemberRepository.GetTransactionalContext(TransactionContextScope.Internal);

            var organizationMember = this.Get(id, context);

            _organizationMemberValidator.SetTransactionalContext(context);

            if (!_organizationMemberValidator.IsValid(organizationMember, Scenario.Delete))
                throw new FunctionalException(ErrorType.ValidationError, _organizationMemberValidator.Errors);

            try
            {
                _organizationMemberRepository.Remove(organizationMember.Id, context);

                var searchFilter = new OrganizationMemberInvitationSearchFilter() 
                {
                    IdOrganization = organizationMember.Organization.Id,
                    IdRole = organizationMember.Role.Id,
                    IdUser = organizationMember.User.Id
                };
                var invitation = _organizationMemberRepository.GetInvitations(searchFilter, paginationFilter: null, sorting: null, context).Results.FirstOrDefault();
                if (invitation != null)
                    _organizationMemberRepository.RemoveInvitation(invitation, context);

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
                context = _organizationMemberRepository.GetTransactionalContext(TransactionContextScope.Internal);

            var organizationMemberInvitation = this.GetInvitation(id, context);

            _organizationMemberInvitationValidator.SetTransactionalContext(context);

            if (!_organizationMemberInvitationValidator.IsValid(organizationMemberInvitation, Scenario.Delete))
                throw new FunctionalException(ErrorType.ValidationError, _organizationMemberInvitationValidator.Errors);

            try
            {
                _organizationMemberRepository.RemoveInvitation(organizationMemberInvitation, context);

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

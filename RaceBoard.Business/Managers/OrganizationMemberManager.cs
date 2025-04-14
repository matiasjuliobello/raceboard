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
using System.Text;
using Enums = RaceBoard.Domain.Enums;

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
        private readonly IAuthorizationManager _authorizationManager;
        private readonly IInvitationManager _invitationManager;

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
                IRequestContextManager requestContextManager,
                IAuthorizationManager authorizationManager,
                IInvitationManager invitationManager
            ) : base(requestContextManager, translator)
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
            _authorizationManager = authorizationManager;
            _invitationManager = invitationManager;
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
            var searchFilter = new OrganizationMemberInvitationSearchFilter()
            { 
                Organization = new Organization() {  Id = idOrganization },
                IsPending = isPending 
            };

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
            var contextUser = base.GetContextUser();
            _authorizationManager.ValidatePermission(Enums.Action.OrganizationMember_Create, organizationMemberInvitation.Organization.Id, contextUser.Id);

            if (context == null)
                context = _organizationMemberRepository.GetTransactionalContext(TransactionContextScope.Internal);

            _organizationMemberInvitationValidator.SetTransactionalContext(context);
            if (!_organizationMemberInvitationValidator.IsValid(organizationMemberInvitation, Scenario.Create))
                throw new FunctionalException(ErrorType.ValidationError, _organizationMemberInvitationValidator.Errors);

            organizationMemberInvitation.RequestDate = _dateTimeHelper.GetCurrentTimestamp();
            organizationMemberInvitation.Invitation.Token = this.GenerateInvitationToken();

            if (organizationMemberInvitation.User != null)
                organizationMemberInvitation.Invitation.EmailAddress = _userRepository.GetById(organizationMemberInvitation.User.Id).Email;

            try
            {
                _organizationMemberRepository.CreateInvitation(organizationMemberInvitation, context);

                _invitationManager.SendOrganizationInvitation(organizationMemberInvitation);

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
            //var contextUser = base.GetContextUser();
            //_authorizationManager.ValidatePermission(Enums.Action.OrganizationMember_Update, organizationMemberInvitation.Organization.Id, contextUser.Id);
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
            var organizationMember = this.Get(id);

            var contextUser = base.GetContextUser();
            _authorizationManager.ValidatePermission(Enums.Action.OrganizationMember_Delete, organizationMember.Organization.Id, contextUser.Id);

            if (context == null)
                context = _organizationMemberRepository.GetTransactionalContext(TransactionContextScope.Internal);

            _organizationMemberValidator.SetTransactionalContext(context);

            if (!_organizationMemberValidator.IsValid(organizationMember, Scenario.Delete))
                throw new FunctionalException(ErrorType.ValidationError, _organizationMemberValidator.Errors);

            try
            {
                _organizationMemberRepository.Remove(organizationMember.Id, context);

                var searchFilter = new OrganizationMemberInvitationSearchFilter() 
                {
                    Organization = organizationMember.Organization,
                    Role = organizationMember.Role,
                    User = organizationMember.User
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
            var contextUser = base.GetContextUser();
            _authorizationManager.ValidatePermission(Enums.Action.OrganizationMember_Delete, 0, contextUser.Id);

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

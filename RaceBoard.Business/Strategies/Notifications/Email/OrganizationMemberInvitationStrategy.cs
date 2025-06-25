using Microsoft.Extensions.Configuration;
using RaceBoard.Business.Strategies.Notifications.Abstract;
using RaceBoard.Common.Exceptions;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;
using RaceBoard.Mailing.Entities;
using RaceBoard.Notification.Interfaces;
using RaceBoard.Translations.Interfaces;

namespace RaceBoard.Business.Strategies.Notifications.Email
{
    public class OrganizationMemberInvitationStrategy : AbstractStrategy, INotificationStrategy
    {
        private readonly IPersonRepository _personRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IOrganizationRepository _organizationRepository;

        public OrganizationMemberInvitationStrategy
            (
                IConfiguration configuration,
                ITranslator translator,
                IPersonRepository personRepository,
                IRoleRepository roleRepository,
                IOrganizationRepository organizationRepository,
                IMemberRepository memberRepository
            ) : base(configuration, translator, memberRepository)
        {
            _personRepository = personRepository;
            _roleRepository = roleRepository;
            _organizationRepository = organizationRepository;
        }

        public INotification Produce(object data)
        {
            var notificationData = this.BuildNotificationData(data as OrganizationMemberInvitation);

            return new EmailNotification()
            {
                Media = Notification.Enums.NotificationMedia.Mail,
                //Settings = new EmailNotificationSettings(),
                Data = notificationData
            };
        }

        #region Private Methods

        private EmailNotificationData BuildNotificationData(OrganizationMemberInvitation organizationMemberInvitation)
        {
            var organization = _organizationRepository.Get(organizationMemberInvitation.Organization.Id);
            if (organization == null)
                throw new FunctionalException(Common.Enums.ErrorType.NotFound, base.Translate("RecordNotFound"));

            var role = _roleRepository.Get().Results.First(x => x.Id == organizationMemberInvitation.Role.Id);

            var requestUser = _personRepository.GetByIdUser(organizationMemberInvitation.RequestUser.Id);

            string subject = base.Translate("OrganizationMemberInvitationEmailSubject");
            string body = String.Format(base.Translate("OrganizationMemberInvitationEmailBody"), requestUser.Fullname, organization.Name, role.Name);
            string link = base.BuildInvitationLink("organization", organization!.Id, organizationMemberInvitation.Invitation);
            body = $"<br />{body}<br /><br /><br />{link}";

            string recipientAddress = organizationMemberInvitation.Invitation.EmailAddress;
            string recipientName = organizationMemberInvitation.Invitation.EmailAddress;

            return new EmailNotificationData()
            {
                Subject = subject,
                Body = body,
                EmailAddress = recipientAddress,
                FullName = recipientName
            };
        }

        #endregion
    }
}

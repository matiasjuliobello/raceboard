using Microsoft.Extensions.Configuration;
using RaceBoard.Business.Strategies.Notifications.Abstract;
using RaceBoard.Common.Exceptions;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;
using RaceBoard.Mailing.Entities;
using RaceBoard.Notification.Interfaces;
using RaceBoard.Translations.Interfaces;
using Enums = RaceBoard.Domain.Enums;

namespace RaceBoard.Business.Strategies.Notifications.Email
{
    public class OrganizationCreationStrategy : AbstractStrategy, INotificationStrategy
    {
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IOrganizationMemberRepository _organizationMemberRepository;

        public OrganizationCreationStrategy
            (
                IConfiguration configuration,
                ITranslator translator,
                IMemberRepository memberRepository,
                IOrganizationRepository organizationRepository,
                IOrganizationMemberRepository organizationMemberRepository
            ) : base(configuration, translator, memberRepository)
        {
            _organizationRepository = organizationRepository;
            _organizationMemberRepository = organizationMemberRepository;
        }

        public INotification Produce(object data)
        {
            var notificationData = this.BuildNotificationData(data as Organization);

            return new EmailNotification()
            {
                Media = Notification.Enums.NotificationMedia.Mail,
                //Settings = new EmailNotificationSettings(),
                Data = notificationData
            };
        }

        #region Private Methods

        private EmailNotificationData BuildNotificationData(Organization newOrganization)
        {
            var organization = _organizationRepository.Get(newOrganization.Id);
            if (organization == null)
                throw new FunctionalException(Common.Enums.ErrorType.NotFound, base.Translate("RecordNotFound"));

            var organizationMembers = _organizationMemberRepository.Get(new OrganizationMemberSearchFilter() { Organization = new Organization() { Id = organization.Id } }).Results;
            var organizationManager = organizationMembers.Where(x => x.Role.Id == (int)Enums.UserRole.Manager).FirstOrDefault();
            if (organizationManager == null)
                throw new FunctionalException(Common.Enums.ErrorType.NotFound, base.Translate("OrganizationLeaderNotFound"));

            //var championship = _championshipRepository.Get(organization.Championship.Id);
            //if (championship == null)
            //    throw new FunctionalException(Common.Enums.ErrorType.NotFound, base.Translate("RecordNotFound"));

            string subject = base.Translate("NewOrganizationCreated");
            string body = $"A new organization has been created<br />";
            body += $"Organization '<b>{organization.Name}</b>' has been created.";

            string link = base.BuildApplicationLink();
            string emailBody = $"<br />{body}<br /><br /><br />{link}";

            string recipientAddress = newOrganization.CreationUser.Email;
            string recipientName = organizationManager.Person.Fullname;

            return new EmailNotificationData()
            {
                Subject = subject,
                Body = emailBody,
                EmailAddress = recipientAddress,
                FullName = recipientName
            };
        }

        #endregion
    }
}

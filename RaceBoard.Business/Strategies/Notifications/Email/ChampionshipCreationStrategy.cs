using Microsoft.Extensions.Configuration;
using RaceBoard.Business.Strategies.Notifications.Abstract;
using RaceBoard.Common.Exceptions;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;
using RaceBoard.Mailing.Entities;
using RaceBoard.Notification.Interfaces;
using RaceBoard.Translations.Interfaces;
using static QuestPDF.Helpers.Colors;
using Enums = RaceBoard.Domain.Enums;

namespace RaceBoard.Business.Strategies.Notifications.Email
{
    public class ChampionshipCreationStrategy : AbstractStrategy, INotificationStrategy
    {
        private readonly IChampionshipRepository _championshipRepository;
        private readonly IChampionshipMemberRepository _championshipMemberRepository;

        public ChampionshipCreationStrategy
            (
                IConfiguration configuration,
                ITranslator translator,
                IMemberRepository memberRepository,
                IChampionshipRepository championshipRepository,
                IChampionshipMemberRepository championshipMemberRepository
            ) : base(configuration, translator, memberRepository)
        {
            _championshipRepository = championshipRepository;
            _championshipMemberRepository = championshipMemberRepository;
        }

        public INotification Produce(object data)
        {
            var notificationData = this.BuildNotificationData(data as Championship);

            return new EmailNotification()
            {
                Media = Notification.Enums.NotificationMedia.Mail,
                //Settings = new EmailNotificationSettings(),
                Data = notificationData
            };
        }

        #region Private Methods

        private EmailNotificationData BuildNotificationData(Championship newChampionship)
        {
            var championship = _championshipRepository.Get(newChampionship.Id);
            if (championship == null)
                throw new FunctionalException(Common.Enums.ErrorType.NotFound, base.Translate("RecordNotFound"));

            var championshipMembers = _championshipMemberRepository.Get(new ChampionshipMemberSearchFilter() { Championship = new Championship() { Id = championship.Id } }).Results;
            var championshipManager = championshipMembers.Where(x => x.Role.Id == (int)Enums.UserRole.Manager).FirstOrDefault();
            if (championshipManager == null)
                throw new FunctionalException(Common.Enums.ErrorType.NotFound, base.Translate("ChampionshipLeaderNotFound"));

            var organization = championship.Organizations.FirstOrDefault();
            if (organization == null)
                throw new FunctionalException(Common.Enums.ErrorType.NotFound, base.Translate("OrganizationNotFound"));

            string subject = base.Translate("NewChampionshipCreated");
            string body = $"A new championship has been created<br />";
            body += $"Championship '<b>{championship.Name}</b>' has been created, held by '<b>{organization.Name}</b>'";

            string link = base.BuildApplicationLink();
            string emailBody = $"<br />{body}<br /><br /><br />{link}";

            string recipientAddress = newChampionship.CreationUser.Email;
            string recipientName = championshipManager.Person.Fullname;

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

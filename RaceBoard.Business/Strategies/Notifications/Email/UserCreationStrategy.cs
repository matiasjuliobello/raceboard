using Microsoft.Extensions.Configuration;
using RaceBoard.Business.Strategies.Notifications.Abstract;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;
using RaceBoard.Mailing.Entities;
using RaceBoard.Notification.Interfaces;
using RaceBoard.Translations.Interfaces;

namespace RaceBoard.Business.Strategies.Notifications.Email
{
    public class UserCreationStrategy : AbstractStrategy, INotificationStrategy
    {
        private readonly string _appLink;

        public UserCreationStrategy
            (
                IConfiguration configuration,
                ITranslator translator,
                IMemberRepository memberRepository
            ) : base(configuration, translator, memberRepository)
        {
            _appLink = configuration["FrontEndUrl"];
        }

        public INotification Produce(object data)
        {
            var notificationData = this.BuildNotificationData(data as User);

            return new EmailNotification()
            {
                Media = Notification.Enums.NotificationMedia.Mail,
                //Settings = new EmailNotificationSettings(),
                Data = notificationData
            };
        }

        #region Private Methods

        private EmailNotificationData BuildNotificationData(User user)
        {
            string subject = base.Translate("NewAccountCreated");
            string body = $"Welcome on board!<br />We're glad to have you in.";

            string link = base.BuildApplicationLink();
            string emailBody = $"<br />{body}<br /><br /><br />{link}";

            string recipientAddress = user.Email;
            string recipientName = "New user";

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

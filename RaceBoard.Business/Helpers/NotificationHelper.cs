using RaceBoard.Business.Helpers.Interfaces;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Managers.Interfaces;
using RaceBoard.Notification.Interfaces;
using RaceBoard.Mailing.Entities;
using RaceBoard.PushMessaging.Entities;

namespace RaceBoard.Business.Helpers
{
    public class NotificationHelper : INotificationHelper
    {
        private readonly INotificationStrategyFactory _notificationStrategyFactory;
        private readonly IMailManager _mailManager;
        private readonly IPushNotificationManager _pushNotificationManager;

        public NotificationHelper
            (
                INotificationStrategyFactory notificationStrategyFactory,
                IMailManager mailManager,
                IPushNotificationManager pushNotificationManager
            )
        {
            _notificationStrategyFactory = notificationStrategyFactory;
            _mailManager = mailManager;
            _pushNotificationManager = pushNotificationManager;
        }

        #region INotificationHelper implementation

        public void SendNotification(Notification.Enums.NotificationType notificationType, object data)
        {
            var strategies = _notificationStrategyFactory.ResolveStrategy(notificationType);

            foreach (var strategy in strategies)
            {
                try
                {
                    INotification notification = strategy.Produce(data);

                    switch (notification.Media)
                    {
                        case Notification.Enums.NotificationMedia.Mail:
                            var emailNotification = notification as EmailNotification;
                            if (emailNotification == null)
                                continue;

                            var emailData = emailNotification.Data as EmailNotificationData;
                            if (emailData == null)
                                continue;

                            _mailManager.Send(emailData.Subject, emailData.Body, emailData.EmailAddress, emailData.FullName);

                            break;

                        case Notification.Enums.NotificationMedia.Push:
                            var pushNotification = notification as PushNotification;
                            if (pushNotification == null)
                                continue;

                            var pushData = pushNotification.Data as PushNotificationData;
                            if (pushData == null)
                                continue;

                            _pushNotificationManager.Send(pushData.Title, pushData.Message, pushData.IdChampionship, pushData.IdsRaceClasses);

                            break;
                    }
                }
                catch (Exception)
                {
                    // TODO: log here to understand why strategy failed to send mail|push notification                    
                }
            }
        }

        #endregion
    }
}

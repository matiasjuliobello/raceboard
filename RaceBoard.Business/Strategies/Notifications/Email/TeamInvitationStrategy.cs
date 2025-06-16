////using RaceBoard.Mailing.Interfaces;
//using RaceBoard.Notification.Interfaces;

//namespace RaceBoard.Business.Strategies.Notifications.Email
//{
//    public class TeamInvitationStrategy : INotificationStrategy
//    {
//        private readonly INotificationProvider _notificationProvider;

//        public TeamInvitationStrategy(INotificationProvider notificationProvider)
//        {
//            _notificationProvider = notificationProvider;
//        }

//        public async void SendNotificationAsync(INotification notification)
//        {
//            await _notificationProvider.SendNotification(notification); // .SendMail(subject: "", body: "", emailAddress: "", fullName: "");
//        }
//    }
//}

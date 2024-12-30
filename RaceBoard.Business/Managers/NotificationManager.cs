using Microsoft.Extensions.Configuration;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Messaging.Entities;
using RaceBoard.Messaging.Interfaces;
using RestSharp;

namespace RaceBoard.Business.Managers
{
    public class NotificationManager : INotificationManager
    {
        private readonly INotificationProvider _notificationProvider;
        private readonly IRaceClassManager _raceClassManager;

        private readonly bool _enabled;

        private const int _MESSAGE_MAX_LENGTH = 50;

        public NotificationManager
        (
            IConfiguration configuration,
            INotificationProvider notificationProvider,
            IRaceClassManager raceClassManager
        )
        {
            _notificationProvider = notificationProvider;
            _raceClassManager = raceClassManager;

            bool.TryParse(configuration["Messaging_Enabled"], out _enabled);
        }

        public async Task SendNotifications(string title, string message, int idChampionship, int[] idsRaceClasses)
        {
            if (!_enabled)
                return;

            List<Task<RestResponse>> tasks = new List<Task<RestResponse>>();

            int[] targetRaceClassIds = new int[] { };

            string idTarget = null;

            var allRaceClassIds = _raceClassManager.Get().Results.Select(x => x.Id).ToArray();
            if (allRaceClassIds.Length == idsRaceClasses.Length)
            {
                idTarget = $"{idChampionship}";

                targetRaceClassIds = allRaceClassIds;
            }
            else
            {
                targetRaceClassIds = idsRaceClasses;
            }

            if (message.Length > _MESSAGE_MAX_LENGTH)
                message = message.Substring(0, _MESSAGE_MAX_LENGTH) + "...";

            Parallel.ForEach(targetRaceClassIds, idsRaceClass =>
            {
                var notification = new Notification()
                {
                    NotificationType = Messaging.Providers.NotificationType.Topic,
                    IdTarget = idTarget != null ? idTarget : $"{idChampionship}_{idsRaceClass}",
                    Title = title,
                    Message = message,
                    ImageFileUrl = null
                };

                Task<RestResponse> response = _notificationProvider.SendNotification(notification);

                tasks.Add(response);
            });

            await Task.WhenAll(tasks);


            //List<int> items = new List<int>();
            //for(int i=0; i < 10; i++)
            //{
            //    items.Add(i);
            //    Thread.Sleep(1000);
            //}

            //int count = items.Count;
        }
    }
}

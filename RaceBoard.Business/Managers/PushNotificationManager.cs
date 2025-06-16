using Microsoft.Extensions.Configuration;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Messaging.Interfaces;
using RaceBoard.PushMessaging.Entities;
using RaceBoard.PushMessaging.Enums;

namespace RaceBoard.Business.Managers
{
    public class PushNotificationManager : IPushNotificationManager
    {
        private readonly IPushNotificationProvider _pushNotificationProvider;
        private readonly IRaceClassManager _raceClassManager;

        private readonly bool _enabled;

        private const int _MESSAGE_MAX_LENGTH = 50;

        public PushNotificationManager
        (
            IConfiguration configuration,
            IPushNotificationProvider pushNotificationProvider,
            IRaceClassManager raceClassManager
        )
        {
            _pushNotificationProvider = pushNotificationProvider;
            _raceClassManager = raceClassManager;

            bool.TryParse(configuration["Messaging_Enabled"], out _enabled);
        }

        public async Task Send(string title, string message, int idChampionship, int[] idsRaceClasses)
        {
            if (!_enabled)
                return;

            var tasks = new List<Task>();

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
                var notification = new PushNotification()
                {
                    Data = new PushNotificationData()
                    {
                        NotificationType = PushNotificationType.Topic,
                        IdTarget = idTarget != null ? idTarget : $"{idChampionship}_{idsRaceClass}",
                        Title = title,
                        Message = message,
                        ImageFileUrl = null
                    }
                };

                Task task = _pushNotificationProvider.Send(notification);

                tasks.Add(task);
            });

            await Task.WhenAll(tasks);


            List<int> items = new List<int>();
            for (int i = 0; i < 10; i++)
            {
                items.Add(i);
                Thread.Sleep(1000);
            }

            int count = items.Count;
        }
    }
}

using Microsoft.Extensions.Configuration;
using RaceBoard.Business.Strategies.Notifications.Abstract;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;
using RaceBoard.Notification.Interfaces;
using RaceBoard.PushMessaging.Entities;
using RaceBoard.Translations.Interfaces;

namespace RaceBoard.Business.Strategies.Notifications.Push
{
    public class ChampionshipFileUploadStrategy : AbstractStrategy, INotificationStrategy
    {
        private readonly IChampionshipRepository _championshipRepository;

        public ChampionshipFileUploadStrategy
            (
                IConfiguration configuration,
                ITranslator translator,
                IChampionshipRepository championshipRepository,
                IMemberRepository memberRepository
            ) : base(configuration, translator, memberRepository)
        {
            _championshipRepository = championshipRepository;
            //_memberRepository = memberRepository;
        }

        public INotification Produce(object data)
        {
            var notificationData = this.BuildNotificationData(data as ChampionshipFile);

            return new PushNotification()
            {
                Media = Notification.Enums.NotificationMedia.Push,
                //Settings = new EmailNotificationSettings(),
                Data = notificationData
            };
        }
        private PushNotificationData BuildNotificationData(ChampionshipFile championshipFile)
        {
            var members = base.CheckForTargetMembers(championshipFile.Championship, championshipFile.RaceClasses);

            var championship = _championshipRepository.Get(championshipFile.Championship.Id);

            string title = $"'{championshipFile.Championship.Name}': " + base.Translate("NewFileHasBeenUploaded");
            string message = championshipFile.File.Description;

            return new PushNotificationData()
            {
                IdChampionship = championshipFile.Championship.Id,
                IdsRaceClasses = championshipFile.RaceClasses.Select(x => x.Id).ToArray(),
                Title = base.Translate("NewFileHasBeenUploaded"),
                Message = message
            };
        }
    }
}

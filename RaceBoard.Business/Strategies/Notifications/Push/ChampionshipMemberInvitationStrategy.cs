using Microsoft.Extensions.Configuration;
using RaceBoard.Business.Strategies.Notifications.Abstract;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;
using RaceBoard.Notification.Interfaces;
using RaceBoard.PushMessaging.Entities;
using RaceBoard.Translations.Interfaces;

namespace RaceBoard.Business.Strategies.Notifications.Push
{
    public class ChampionshipMemberInvitationStrategy : AbstractStrategy, INotificationStrategy
    {
        private readonly IPersonRepository _personRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IChampionshipRepository _championshipRepository;
        private readonly IChampionshipGroupRepository _championshipGroupRepository;

        public ChampionshipMemberInvitationStrategy
            (
                IConfiguration configuration,
                ITranslator translator,
                IPersonRepository personRepository,
                IRoleRepository roleRepository,
                IChampionshipRepository championshipRepository,
                IChampionshipGroupRepository championshipGroupRepository,
                IMemberRepository memberRepository
            ) : base(configuration, translator, memberRepository)
        {
            _personRepository = personRepository;
            _roleRepository = roleRepository;
            _championshipRepository = championshipRepository;
            _championshipGroupRepository = championshipGroupRepository;
        }

        public INotification Produce(object data)
        {
            var notificationData = this.BuildNotificationData(data as ChampionshipMemberInvitation);

            return new PushNotification()
            {
                Media = Notification.Enums.NotificationMedia.Push,
                //Settings = new EmailNotificationSettings(),
                Data = notificationData
            };
        }

        private PushNotificationData BuildNotificationData(ChampionshipMemberInvitation championshipMemberInvitation)
        {
            var championship = _championshipRepository.Get(championshipMemberInvitation.Championship.Id);
            var championshipGroups = _championshipGroupRepository.Get(championship.Id);

            var raceClasses = championshipGroups.SelectMany(x => x.RaceClasses);

            base.CheckForTargetMembers(championship, raceClasses);

            var requestUser = _personRepository.GetByIdUser(championshipMemberInvitation.RequestUser.Id);

            var role = _roleRepository.Get().Results.First(x => x.Id == championshipMemberInvitation.Role.Id);
            championshipMemberInvitation.Role.Name = role.Name;

            string title = base.Translate("YouVeBeenInvitedToJoinChampionship");

            string message = $"You've been invited by <b>{requestUser.Fullname}</b> to join <b>'{championshipMemberInvitation.Championship.Name}'</b>, performing as <b>{championshipMemberInvitation.Role.Name}</b>";

            return new PushNotificationData()
            {
                IdChampionship = championship.Id,
                IdsRaceClasses = raceClasses.Select(x => x.Id).ToArray(),
                Title = title,
                Message = message
            };
        }
    }
}

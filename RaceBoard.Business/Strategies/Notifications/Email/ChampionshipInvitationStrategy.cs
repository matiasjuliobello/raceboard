using Microsoft.Extensions.Configuration;
using RaceBoard.Business.Strategies.Notifications.Abstract;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;
using RaceBoard.Mailing.Entities;
using RaceBoard.Notification.Interfaces;
using RaceBoard.Translations.Interfaces;

namespace RaceBoard.Business.Strategies.Notifications.Email
{
    public class ChampionshipInvitationStrategy : AbstractStrategy, INotificationStrategy
    {
        private readonly IPersonRepository _personRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IChampionshipRepository _championshipRepository;

        public ChampionshipInvitationStrategy
            (
                IConfiguration configuration,
                ITranslator translator,
                IPersonRepository personRepository, 
                IRoleRepository roleRepository, 
                IChampionshipRepository championshipRepository
            ) : base(configuration, translator)
        {
            _personRepository = personRepository;
            _roleRepository = roleRepository;
            _championshipRepository = championshipRepository;
        }

        public INotification Produce(object data)
        {
            var notificationData = this.BuildNotificationData(data as ChampionshipMemberInvitation);

            return new EmailNotification()
            {
                Media = Notification.Enums.NotificationMedia.Mail,
                //Settings = new EmailNotificationSettings(),
                Data = notificationData
            };
        }

        #region Private Methods

        private EmailNotificationData BuildNotificationData(ChampionshipMemberInvitation championshipMemberInvitation)
        {
            var requestUser = _personRepository.GetByIdUser(championshipMemberInvitation.RequestUser.Id);

            var role = _roleRepository.Get().Results.First(x => x.Id == championshipMemberInvitation.Role.Id);
            championshipMemberInvitation.Role.Name = role.Name;

            var championship = _championshipRepository.Get(championshipMemberInvitation.Championship.Id);

            string emailSubject = base.Translate("YouVeBeenInvitedToJoinChampionship"); //"You've been invited to join a championship"

            string link = base.BuildInvitationLink("championship", championship!.Id, championshipMemberInvitation.Invitation);

            string body = $"You've been invited by <b>{requestUser.Fullname}</b> to join <b>'{championship.Name}'</b>, performing as <b>{championshipMemberInvitation.Role.Name}</b>";
            
            string emailBody = $"<br />{body}<br /><br /><br />{link}";

            string emailRecipientAddress = championshipMemberInvitation.Invitation.EmailAddress;
            string emailRecipientName = championshipMemberInvitation.Invitation.EmailAddress;

            return new EmailNotificationData()
            {
                Subject = emailSubject,
                Body = emailBody,
                EmailAddress = emailRecipientAddress,
                FullName = emailRecipientName
            };
        }

        #endregion
    }
}

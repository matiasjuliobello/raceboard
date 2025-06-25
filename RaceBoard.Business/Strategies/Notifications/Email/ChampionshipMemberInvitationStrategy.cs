using Microsoft.Extensions.Configuration;
using RaceBoard.Business.Strategies.Notifications.Abstract;
using RaceBoard.Common.Exceptions;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;
using RaceBoard.Mailing.Entities;
using RaceBoard.Notification.Interfaces;
using RaceBoard.Translations.Interfaces;

namespace RaceBoard.Business.Strategies.Notifications.Email
{
    public class ChampionshipMemberInvitationStrategy : AbstractStrategy, INotificationStrategy
    {
        private readonly IPersonRepository _personRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IChampionshipRepository _championshipRepository;

        public ChampionshipMemberInvitationStrategy
            (
                IConfiguration configuration,
                ITranslator translator,
                IPersonRepository personRepository, 
                IRoleRepository roleRepository, 
                IChampionshipRepository championshipRepository,
                IMemberRepository memberRepository
            ) : base(configuration, translator, memberRepository)
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
            var championship = _championshipRepository.Get(championshipMemberInvitation.Championship.Id);
            if (championship == null)
                throw new FunctionalException(Common.Enums.ErrorType.NotFound, base.Translate("RecordNotFound"));

            var role = _roleRepository.Get().Results.First(x => x.Id == championshipMemberInvitation.Role.Id);

            var requestUser = _personRepository.GetByIdUser(championshipMemberInvitation.RequestUser.Id);

            string subject = base.Translate("ChampionshipMemberInvitationEmailSubject");
            string body = String.Format(base.Translate("ChampionshipMemberInvitationEmailBody"), requestUser.Fullname, championship.Name, role.Name);
            string link = base.BuildInvitationLink("championship", championship!.Id, championshipMemberInvitation.Invitation);
            body = $"<br />{body}<br /><br /><br />{link}";

            string recipientAddress = championshipMemberInvitation.Invitation.EmailAddress;
            string recipientName = championshipMemberInvitation.Invitation.EmailAddress;

            return new EmailNotificationData()
            {
                Subject = subject,
                Body = body,
                EmailAddress = recipientAddress,
                FullName = recipientName
            };
        }

        #endregion
    }
}

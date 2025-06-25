using Microsoft.Extensions.Configuration;
using RaceBoard.Business.Strategies.Notifications.Abstract;
using RaceBoard.Common.Exceptions;
using RaceBoard.Data.Repositories;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;
using RaceBoard.Mailing.Entities;
using RaceBoard.Notification.Interfaces;
using RaceBoard.Translations.Interfaces;

namespace RaceBoard.Business.Strategies.Notifications.Email
{
    public class TeamMemberInvitationStrategy : AbstractStrategy, INotificationStrategy
    {
        private readonly IPersonRepository _personRepository;
        private readonly ITeamMemberRoleRepository _teamMemberRoleRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly IChampionshipRepository _championshipRepository;

        public TeamMemberInvitationStrategy
            (
                IConfiguration configuration,
                ITranslator translator,
                IPersonRepository personRepository,
                ITeamMemberRoleRepository teamMemberRoleRepository,
                ITeamRepository teamRepository,
                IChampionshipRepository championshipRepository,
                IMemberRepository memberRepository
            ) : base(configuration, translator, memberRepository)
        {
            _personRepository = personRepository;
            _teamMemberRoleRepository = teamMemberRoleRepository;
            _teamRepository = teamRepository;
            _championshipRepository = championshipRepository;
        }

        public INotification Produce(object data)
        {
            var notificationData = this.BuildNotificationData(data as TeamMemberInvitation);

            return new EmailNotification()
            {
                Media = Notification.Enums.NotificationMedia.Mail,
                //Settings = new EmailNotificationSettings(),
                Data = notificationData
            };
        }

        #region Private Methods

        private EmailNotificationData BuildNotificationData(TeamMemberInvitation teamMemberInvitation)
        {
            var team = _teamRepository.Get(teamMemberInvitation.Team.Id);
            if (team == null)
                throw new FunctionalException(Common.Enums.ErrorType.NotFound, base.Translate("RecordNotFound"));

            var championship = _championshipRepository.Get(team.Championship.Id);
            if (championship == null)
                throw new FunctionalException(Common.Enums.ErrorType.NotFound, base.Translate("RecordNotFound"));

            var teamMemberRole = _teamMemberRoleRepository.Get().Results.First(x => x.Id == teamMemberInvitation.Role.Id);

            var requestUser = _personRepository.GetByIdUser(teamMemberInvitation.RequestUser.Id);

            string subject = base.Translate("TeamMemberInvitationEmailSubject");
            string body = String.Format(base.Translate("TeamMemberInvitationEmailBody"), requestUser.Fullname, championship.Name, teamMemberRole.Name);
            string link = base.BuildInvitationLink("team", team!.Id, teamMemberInvitation.Invitation);
            body = $"<br />{body}<br /><br /><br />{link}";

            string recipientAddress = teamMemberInvitation.Invitation.EmailAddress;
            string recipientName = teamMemberInvitation.Invitation.EmailAddress;

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
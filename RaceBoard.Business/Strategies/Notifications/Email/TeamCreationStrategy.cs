using Microsoft.Extensions.Configuration;
using RaceBoard.Business.Strategies.Notifications.Abstract;
using RaceBoard.Common.Exceptions;
using RaceBoard.Data.Repositories;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;
using RaceBoard.Mailing.Entities;
using RaceBoard.Notification.Interfaces;
using RaceBoard.Translations.Interfaces;
using Enums = RaceBoard.Domain.Enums;

namespace RaceBoard.Business.Strategies.Notifications.Email
{
    public class TeamCreationStrategy : AbstractStrategy, INotificationStrategy
    {
        private readonly ITeamRepository _teamRepository;
        private readonly ITeamMemberRepository _teamMemberRepository;

        public TeamCreationStrategy
            (
                IConfiguration configuration,
                ITranslator translator,
                IMemberRepository memberRepository,
                ITeamRepository teamRepository,
                ITeamMemberRepository teamMemberRepository
            ) : base(configuration, translator, memberRepository)
        {
            _teamRepository = teamRepository;
            _teamMemberRepository = teamMemberRepository;
        }

        public INotification Produce(object data)
        {
            var notificationData = this.BuildNotificationData(data as Team);

            return new EmailNotification()
            {
                Media = Notification.Enums.NotificationMedia.Mail,
                //Settings = new EmailNotificationSettings(),
                Data = notificationData
            };
        }

        #region Private Methods

        private EmailNotificationData BuildNotificationData(Team newTeam)
        {
            var team = _teamRepository.Get(newTeam.Id);
            if (team == null)
                throw new FunctionalException(Common.Enums.ErrorType.NotFound, base.Translate("RecordNotFound"));

            var teamMembers = _teamMemberRepository.Get(new TeamMemberSearchFilter() { Team = new Team() { Id = team.Id } }).Results;
            var teamLeader = teamMembers.Where(x => x.Role.Id == (int)Enums.TeamMemberRole.Leader).FirstOrDefault();
            if (teamLeader == null)
                throw new FunctionalException(Common.Enums.ErrorType.NotFound, base.Translate("TeamLeaderNotFound"));

            //var championship = _championshipRepository.Get(team.Championship.Id);
            //if (championship == null)
            //    throw new FunctionalException(Common.Enums.ErrorType.NotFound, base.Translate("RecordNotFound"));

            var organization = team.Championship.Organizations.FirstOrDefault();
            if (organization == null)
                throw new FunctionalException(Common.Enums.ErrorType.NotFound, base.Translate("OrganizationNotFound"));

            string subject = base.Translate("NewTeamCreated");
            string body = $"A new team has been created<br />";
            body += $"It has been registered to participate in '<b>{team.Championship.Name}</b>' championship, held by '<b>{organization.Name}</b>'";

            string link = base.BuildApplicationLink();
            string emailBody = $"<br />{body}<br /><br /><br />{link}";

            string recipientAddress = newTeam.CreationUser.Email;
            string recipientName = teamLeader.Person.Fullname;

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

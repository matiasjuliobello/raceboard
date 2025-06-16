using Microsoft.Extensions.Configuration;
using RaceBoard.Business.Managers.Abstract;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;
using RaceBoard.Notification.Interfaces;
using RaceBoard.Translations.Interfaces;
using System.Text;

namespace RaceBoard.Business.Managers
{
    public class InvitationManager : AbstractManager, IInvitationManager
    {
        private readonly INotificationStrategyFactory _notificationStrategyFactory;
        private readonly ITeamMemberRoleRepository _teamMemberRoleRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IChampionshipRepository _championshipRepository;
        private readonly IPersonRepository _personRepository;
        private readonly IRoleRepository _roleRepository;

        private readonly string _baseUrl = "";

        public InvitationManager
            (
                INotificationStrategyFactory notificationStrategyFactory,
                IRequestContextManager requestContextManager,
                IPersonRepository personRepository,
                ITeamRepository teamRepository,
                ITeamMemberRoleRepository teamMemberRoleRepository,
                IOrganizationRepository organizationRepository,
                IChampionshipRepository championshipRepository,
                IRoleRepository roleRepository,
                ITranslator translator,
                IConfiguration configuration
            ) : base(requestContextManager, translator)
        {
            _notificationStrategyFactory = notificationStrategyFactory;
            _teamMemberRoleRepository = teamMemberRoleRepository;
            _teamRepository = teamRepository;
            _organizationRepository = organizationRepository;
            _championshipRepository = championshipRepository;
            _roleRepository = roleRepository;
            _personRepository = personRepository;

            _baseUrl = Path.Combine(configuration["FrontEndUrl"], "invitations");
        }

        public void SendOrganizationInvitation(OrganizationMemberInvitation organizationMemberInvitation)
        {
            //var requestUser = _personRepository.GetByIdUser(organizationMemberInvitation.RequestUser.Id);

            //var role = _roleRepository.Get().Results.First(x => x.Id == organizationMemberInvitation.Role.Id);
            //organizationMemberInvitation.Role.Name = role.Name;

            //var organization = _organizationRepository.Get(organizationMemberInvitation.Organization.Id);

            //string emailSubject = $"You've been invited to join an organization";

            //string link = BuildInvitationLink("organization", organization!.Id, organizationMemberInvitation.Invitation);

            //var emailHtmlContent = new StringBuilder();
            //emailHtmlContent.AppendLine("<br />");
            //emailHtmlContent.AppendLine($"You've been invited by <b>{requestUser.Fullname}</b> to join <b>'{organization!.Name}'</b>, performing as <b>{organizationMemberInvitation.Role.Name}</b>");
            //emailHtmlContent.AppendLine("<br /><br /><br />");
            //emailHtmlContent.AppendLine(link);
            //string emailBody = emailHtmlContent.ToString();

            //string emailRecipientAddress = organizationMemberInvitation.Invitation.EmailAddress;
            //string emailRecipientName = organizationMemberInvitation.Invitation.EmailAddress;

            //_notificationStrategyFactory.SendMail(emailSubject, emailBody, emailRecipientAddress, emailRecipientName);

            var strategies = _notificationStrategyFactory.ResolveStrategy(Notification.Enums.NotificationType.Organization_Invitation);
            foreach(var strategy in strategies)
            {
                //strategy.SendNotificationAsync();
            }
        }

        public void SendChampionshipInvitation(ChampionshipMemberInvitation championshipMemberInvitation)
        {
            var requestUser = _personRepository.GetByIdUser(championshipMemberInvitation.RequestUser.Id);

            var role = _roleRepository.Get().Results.First(x => x.Id == championshipMemberInvitation.Role.Id);
            championshipMemberInvitation.Role.Name = role.Name;

            var championship = _championshipRepository.Get(championshipMemberInvitation.Championship.Id);

            string emailSubject = $"You've been invited to join a championship";

            string link = BuildInvitationLink("championship", championship!.Id, championshipMemberInvitation.Invitation);

            var emailHtmlContent = new StringBuilder();
            emailHtmlContent.AppendLine("<br />");
            emailHtmlContent.AppendLine($"You've been invited by <b>{requestUser.Fullname}</b> to join <b>'{championship.Name}'</b>, performing as <b>{championshipMemberInvitation.Role.Name}</b>");
            emailHtmlContent.AppendLine("<br /><br /><br />");
            emailHtmlContent.AppendLine(link);
            string emailBody = emailHtmlContent.ToString();

            string emailRecipientAddress = championshipMemberInvitation.Invitation.EmailAddress;
            string emailRecipientName = championshipMemberInvitation.Invitation.EmailAddress;

            //_notificationStrategyFactory.SendMail(emailSubject, emailBody, emailRecipientAddress, emailRecipientName);
        }    

        public void SendTeamInvitation(TeamMemberInvitation teamMemberInvitation)
        {
            var requestUser = _personRepository.GetByIdUser(teamMemberInvitation.RequestUser.Id);

            var role = _teamMemberRoleRepository.Get().Results.First(x => x.Id == teamMemberInvitation.Role.Id);
            teamMemberInvitation.Role.Name = role.Name;

            var team = _teamRepository.Get(teamMemberInvitation.Team.Id);
            var championship = team!.Championship;

            string emailSubject = Translate("TeamMemberInvitationEmailSubject");
            string emailBody = Translate("TeamMemberInvitationEmailBody");

            string link = BuildInvitationLink("team", team!.Id, teamMemberInvitation.Invitation);

            var emailHtmlContent = new StringBuilder();
            emailHtmlContent.AppendLine("<br />");
            emailHtmlContent.AppendLine(String.Format(emailBody, requestUser.Fullname, championship.Name, teamMemberInvitation.Role.Name));
            emailHtmlContent.AppendLine("<br /><br /><br />");
            emailHtmlContent.AppendLine(link);
            
            emailBody = emailHtmlContent.ToString();

            string emailRecipientAddress = teamMemberInvitation.Invitation.EmailAddress;
            string emailRecipientName = teamMemberInvitation.Invitation.EmailAddress;

            //_notificationStrategyFactory.SendMail(emailSubject, emailBody, emailRecipientAddress, emailRecipientName);
        }

        #region Private Methods

        private string BuildInvitationLink(string entityName, int entityId, Invitation invitation)
        {
            return $"<a href='{_baseUrl}?join_{entityName}={entityId}&token={invitation.Token}'>{Translate("InvitationEmailLinkText")}</a>";
        }

        #endregion
    }
}

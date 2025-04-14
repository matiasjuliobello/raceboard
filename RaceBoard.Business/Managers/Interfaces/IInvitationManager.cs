using RaceBoard.Domain;

namespace RaceBoard.Business.Managers.Interfaces
{
    public interface IInvitationManager
    {
        public void SendOrganizationInvitation(OrganizationMemberInvitation organizationMemberInvitation);
        public void SendChampionshipInvitation(ChampionshipMemberInvitation championshipMemberInvitation);
        public void SendTeamInvitation(TeamMemberInvitation teamMemberInvitation);
    }
}
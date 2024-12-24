namespace RaceBoard.DTOs.Organization.Request
{
    public class OrganizationMemberInvitationRequest
    {
        public int Id { get; set; }
        public int IdOrganization {  get; set; }
        public int IdRole { get; set; }
        public int? IdUser { get; set; }

        public InvitationRequest Invitation {  get; set; }
    }
}
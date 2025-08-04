using RaceBoard.DTOs.Organization.Response;

namespace RaceBoard.DTOs.Coach.Response
{
    public class CoachOrganizationResponse
    {
        public int Id { get; set; }
        public CoachResponse Coach { get; set; }
        public OrganizationResponse Organization { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public bool IsActive { get; set; }
    }
}
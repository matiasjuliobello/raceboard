using RaceBoard.DTOs.Organization.Response;

namespace RaceBoard.DTOs.Boat.Response
{
    public class BoatOrganizationResponse
    {
        public int Id { get; set; }
        public BoatResponse Boat { get; set; }
        public OrganizationResponse Organization { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
    }
}

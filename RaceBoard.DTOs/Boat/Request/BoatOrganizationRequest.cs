
namespace RaceBoard.DTOs.Boat.Request
{
    public class BoatOrganizationRequest
    {
        public int Id { get; set; }
        public int IdBoat { get; set; }
        public int IdOrganization { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
    }
}

using RaceBoard.DTOs.Organization.Response;
using RaceBoard.DTOs.RaceClass.Response;

namespace RaceBoard.DTOs.Boat.Response
{
    public class BoatResponse
    {
        public int Id { get; set; }
        public RaceClassResponse RaceClass { get; set; }
        public BoatOrganizationResponse? Organization { get; set; }
        public string Name { get; set; }
        public string SailNumber { get; set; }
        public string HullNumber { get; set; }
    }
}
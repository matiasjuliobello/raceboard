using RaceBoard.DTOs.Organization.Response;
using RaceBoard.DTOs.RaceClass.Response;

namespace RaceBoard.DTOs.Race.Response
{
    public class RaceResponse
    {
        public int Id { get; set; }
        public OrganizationResponse Organization { get; set; }
        public RaceClassResponse RaceClass { get; set; }
    }
}

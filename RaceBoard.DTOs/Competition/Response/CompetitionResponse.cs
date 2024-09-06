using RaceBoard.DTOs.City.Response;
using RaceBoard.DTOs.Organization.Response;

namespace RaceBoard.DTOs.Competition.Response
{
    public class CompetitionResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public CityResponse City { get; set; }
        public List<OrganizationResponse> Organizations { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
    }
}


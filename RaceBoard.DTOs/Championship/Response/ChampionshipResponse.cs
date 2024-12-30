using RaceBoard.DTOs.City.Response;
using RaceBoard.DTOs.File.Response;
using RaceBoard.DTOs.Organization.Response;

namespace RaceBoard.DTOs.Championship.Response
{
    public class ChampionshipResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public CityResponse City { get; set; }
        public List<OrganizationResponse> Organizations { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public FileResponse ImageFile { get; set; }
        public int Teams {  get; set; }
    }
}


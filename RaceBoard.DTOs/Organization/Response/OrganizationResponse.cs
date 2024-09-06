using RaceBoard.DTOs.City.Response;

namespace RaceBoard.DTOs.Organization.Response
{
    public class OrganizationResponse
    {
        public int Id { get; set; }
        public CityResponse City { get; set; }
        public string Name { get; set; }
    }
}
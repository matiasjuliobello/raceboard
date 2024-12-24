using RaceBoard.DTOs.City.Response;
using RaceBoard.DTOs.User.Response;

namespace RaceBoard.DTOs.Organization.Response
{
    public class OrganizationResponse
    {
        public int Id { get; set; }
        public CityResponse City { get; set; }
        public string Name { get; set; }
        public UserSimpleResponse CreationUser { get; set; }
        public DateTimeOffset CreationDate { get; set; }
    }
}
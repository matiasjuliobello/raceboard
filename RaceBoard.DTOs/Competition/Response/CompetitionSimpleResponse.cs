using RaceBoard.DTOs.City.Response;

namespace RaceBoard.DTOs.Competition.Response
{
    public class CompetitionSimpleResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public CityResponse City { get; set; }
    }
}
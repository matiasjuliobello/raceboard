using RaceBoard.DTOs.City.Response;

namespace RaceBoard.DTOs.Championship.Response
{
    public class ChampionshipSimpleResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public CityResponse City { get; set; }
    }
}
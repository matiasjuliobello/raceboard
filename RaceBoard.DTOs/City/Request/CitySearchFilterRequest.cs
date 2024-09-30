namespace RaceBoard.DTOs.City.Request
{
    public class CitySearchFilterRequest
    {
        public int[]? Ids { get; set; }
        public int? IdCountry { get; set; }
        public string? Name { get; set; }
    }
}
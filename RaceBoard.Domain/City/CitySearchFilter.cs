namespace RaceBoard.Domain
{
    public class CitySearchFilter
    {
        public int[]? Ids { get; set; }
        public Country? Country { get; set; }
        public string? Name { get; set; }
    }
}
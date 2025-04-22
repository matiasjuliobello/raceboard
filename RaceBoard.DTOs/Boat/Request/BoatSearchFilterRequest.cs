namespace RaceBoard.DTOs.Boat.Request
{
    public class BoatSearchFilterRequest
    {
        public int[]? Ids { get; set; }
        public int? IdRaceClass {  get; set; }
        public int? IdRaceCategory { get; set; }
        public string? Name { get; set; }
        public string? SailNumber { get; set; }
        public string? HullNumber { get; set; }
    }
}
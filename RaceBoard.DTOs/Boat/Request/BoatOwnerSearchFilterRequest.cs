namespace RaceBoard.DTOs.Boat.Request
{
    public class BoatOwnerSearchFilterRequest
    {
        public int[]? Ids { get; set; }
        public int? IdBoat { get; set; }
        public int? IdPerson { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public bool? IsActive { get; set; }
    }
}
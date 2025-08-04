namespace RaceBoard.DTOs.Boat.Request
{
    public class BoatOwnerRequest
    {
        public int Id { get; set; }
        public int IdBoat { get; set; }
        public int IdPerson { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
    }
}

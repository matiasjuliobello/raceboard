namespace RaceBoard.DTOs.Boat.Request
{
    public class BoatRequest
    {
        public int Id { get; set; }
        public int IdRaceClass { get; set; }
        public string Name { get; set; }
        public string SailNumber { get; set; }
        public string HullNumber { get; set; }
        public int[] IdsOwner { get; set; }
    }
}
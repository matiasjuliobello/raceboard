namespace RaceBoard.DTOs.Race.Request
{
    public class RaceRequest
    {
        public int Id { get; set; }
        public int IdCompetition { get; set; }
        public int[] IdsRaceClass { get; set; }
        public DateTimeOffset Schedule {  get; set; }
    }
}

namespace RaceBoard.DTOs.Championship.Request
{
    public class ChampionshipFlagRequest
    {
        public int Id { get; set; }
        public int IdGroup { get; set; }
        public int IdFlag { get; set; }
        public int Order { get; set; }
        public int? HoursToLower { get; set; }
        public int? MinutesToLower { get; set; }
    }
}

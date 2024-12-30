namespace RaceBoard.DTOs.Championship.Request
{
    public class ChampionshipFlagSearchFilterRequest
    {
        public int? IdFlag { get; set; }
        public int? IdPerson { get; set; }
        public DateTimeOffset? Raising { get; set; }
        public DateTimeOffset? Lowering { get; set; }
    }
}

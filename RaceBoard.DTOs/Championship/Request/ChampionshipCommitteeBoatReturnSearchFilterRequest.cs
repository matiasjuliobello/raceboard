namespace RaceBoard.DTOs.Championship.Request
{
    public class ChampionshipCommitteeBoatReturnSearchFilterRequest
    {
        public int[]? Ids { get; set; }
        public int? IdChampionship { get; set; }
        public int[]? IdsRaceClass { get; set; }
        public DateTimeOffset? ReturnTime { get; set; }
    }
}
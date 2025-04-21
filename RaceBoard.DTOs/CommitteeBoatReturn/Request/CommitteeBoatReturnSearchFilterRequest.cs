namespace RaceBoard.DTOs.CommitteeBoatReturn.Request
{
    public class CommitteeBoatReturnSearchFilterRequest
    {
        public int[]? Ids { get; set; }
        public int? IdChampionship { get; set; }
        public int[]? IdsRaceClass { get; set; }
        public DateTimeOffset? ReturnTime { get; set; }
    }
}
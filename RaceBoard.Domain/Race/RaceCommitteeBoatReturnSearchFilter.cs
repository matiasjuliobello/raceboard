namespace RaceBoard.Domain
{
    public class RaceCommitteeBoatReturnSearchFilter
    {
        public int[]? Ids { get; set; }
        public Competition? Competition { get; set; }
        public DateTimeOffset? Return { get; set; }
    }
}

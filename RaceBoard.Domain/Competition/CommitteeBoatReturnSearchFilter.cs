namespace RaceBoard.Domain
{
    public class CommitteeBoatReturnSearchFilter
    {
        public int[]? Ids { get; set; }
        public Competition? Competition { get; set; }
        public List<RaceClass>? RaceClasses { get; set; }
        public DateTimeOffset? ReturnTime { get; set; }
    }
}

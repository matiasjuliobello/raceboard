namespace RaceBoard.Domain
{
    public class CompetitionNotificationSearchFilter
    {
        public int[]? Ids { get; set; }
        public Competition? Competition { get; set; }
        public List<RaceClass>? RaceClasses { get; set; }
    }
}
namespace RaceBoard.Domain
{
    public class CompetitionFlagSearchFilter
    {
        public int[]? Ids { get; set; }
        public Competition? Competition { get; set; }
        public Flag? Flag { get; set; }
        public Person? Person { get; set; }
        public DateTimeOffset? Raising { get; set; }
        public DateTimeOffset? Lowering { get; set; }
        public bool? IsActive { get; set; }
    }
}
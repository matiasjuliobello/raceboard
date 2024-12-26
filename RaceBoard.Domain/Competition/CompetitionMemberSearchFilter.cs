namespace RaceBoard.Domain
{
    public class CompetitionMemberSearchFilter
    {
        public int[]? Ids { get; set; }
        public int? IdCompetition { get; set; }
        public int? IdRole { get; set; }
        public int? IdUser { get; set; }
    }
}
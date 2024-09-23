namespace RaceBoard.Domain
{
    public class Competition
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public City City { get; set; }
        public List<Organization> Organizations { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public CompetitionRegistrationTerm RegistrationTerms { get; set; }
        public CompetitionAccreditationTerm AccreditationTerms { get; set; }
        public int Teams { get; set; }
    }
}
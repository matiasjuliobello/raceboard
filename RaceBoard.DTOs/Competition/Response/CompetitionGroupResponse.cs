namespace RaceBoard.DTOs.Competition.Response
{
    public class CompetitionGroupResponse
    {
        public int Id { get; set; }
        public CompetitionSimpleResponse Competition { get; set; }
        public string Name { get; set; }
        public DateTimeOffset CompetitionStartDate { get; set; }
        public DateTimeOffset CompetitionEndDate { get; set; }
        public DateTimeOffset AccreditationStartDate { get; set; }
        public DateTimeOffset AccreditationEndDate { get; set; }
        public DateTimeOffset RegistrationStartDate { get; set; }
        public DateTimeOffset RegistrationEndDate { get; set; }
        public int RegistrationTotalCount { get; set; }
        public int AccreditationTotalCount { get; set; }
        public int CompetitionTotalCount { get; set; }

        public List<CompetitionRaceClassResponse> RaceClasses { get; set; }
    }
}
using RaceBoard.DTOs.RaceClass.Request;

namespace RaceBoard.DTOs.Competition.Request
{
    public class CompetitionGroupRequest
    {
        public int Id { get; set; }
        public int IdCompetition { get; set; }
        public string Name { get; set; }
        public DateTimeOffset CompetitionStartDate { get; set; }
        public DateTimeOffset CompetitionEndDate { get; set; }
        public DateTimeOffset AccreditationStartDate { get; set; }
        public DateTimeOffset AccreditationEndDate { get; set; }
        public DateTimeOffset RegistrationStartDate { get; set; }
        public DateTimeOffset RegistrationEndDate { get; set; }
        public List<RaceClassRequest> RaceClasses { get; set; }
    }
}
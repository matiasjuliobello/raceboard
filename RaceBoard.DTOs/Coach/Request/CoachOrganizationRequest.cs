namespace RaceBoard.DTOs.Coach.Request
{
    public class CoachOrganizationRequest
    {
        public int Id { get; set; }
        public int IdCoach { get; set; }
        public int IdOrganization { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
    }
}
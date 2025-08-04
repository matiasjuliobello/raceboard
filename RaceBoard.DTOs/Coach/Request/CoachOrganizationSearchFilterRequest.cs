namespace RaceBoard.DTOs.Coach.Request
{
    public class CoachOrganizationSearchFilterRequest
    {
        public int[]? Ids { get; set; }
        public int? IdCoach { get; set; }
        public int? IdOrganization { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public bool? IsActive { get; set; }
    }
}
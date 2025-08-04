using RaceBoard.DTOs.Organization.Response;
using RaceBoard.DTOs.Team.Response;

namespace RaceBoard.DTOs.Coach.Response
{
    public class CoachTeamResponse
    {
        public int Id { get; set; }
        public CoachResponse Coach { get; set; }
        public TeamResponse Team { get; set; }
        public int TeamMemberCount { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public bool IsActive { get; set; }
    }
}


namespace RaceBoard.DTOs.ChangeRequest.Request
{
    public class ChangeRequestSearchFilterRequest
    {
        public int[] Ids { get; set; }
        public int IdTeam { get; set; }
        public DateTimeOffset CreationDate { get; set; }
        public DateTimeOffset? ResolutionDate { get; set; }
    }
}
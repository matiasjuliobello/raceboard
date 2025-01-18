namespace RaceBoard.DTOs.ChangeRequest.Request
{
    public abstract class ChangeRequestRequest
    {
        public int Id { get; set; }
        public int IdTeam { get; set; }
        public string ChangeRequested { get; set; }
        public string ChangeReason { get; set; }
        public DateTimeOffset CreationDate {  get; set; }
        public DateTimeOffset? ResolutionDate { get; set; }
        public string? ResolutionComments {  get; set; }
    }
}

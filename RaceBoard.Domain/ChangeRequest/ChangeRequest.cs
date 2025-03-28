namespace RaceBoard.Domain
{
    public abstract class ChangeRequest
    {
        public int Id { get; set; }
        public Team Team {  get; set; }
        public User RequestUser {  get; set; }
        public Person RequestPerson { get; set; }
        public ChangeRequestStatus Status {  get; set; }
        public File? File { get; set; }
        public string ChangeRequested { get; set; }
        public string ChangeReason { get; set; }
        public DateTimeOffset CreationDate { get; set; }
        public DateTimeOffset? ResolutionDate { get; set;  }
        public string? ResolutionComments { get; set; }
    }
}

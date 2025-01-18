namespace RaceBoard.DTOs.ChangeRequest.Request
{
    public class CrewChangeRequestRequest : ChangeRequestRequest
    {
        public int IdReplacedUser { get; set; }
        public string ReplacementFullName { get; set; }
    }
}

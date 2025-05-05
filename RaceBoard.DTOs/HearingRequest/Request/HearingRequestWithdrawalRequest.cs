namespace RaceBoard.DTOs.HearingRequest.Request
{
    public class HearingRequestWithdrawalRequest
    {
        public int Id { get; set; }
        public bool IsRequested { get; set; }
        public bool IsAuthorized { get; set; }
    }
}
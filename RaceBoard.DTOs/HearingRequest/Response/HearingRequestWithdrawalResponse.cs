namespace RaceBoard.DTOs.HearingRequest.Response
{
    public class HearingRequestWithdrawalResponse
    {
        public int Id { get; set; }
        public bool IsRequested { get; set; }
        public bool IsAuthorized { get; set; }
    }
}
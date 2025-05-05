namespace RaceBoard.Domain
{
    public class HearingRequestWithdrawal
    {
        public int Id { get; set; }
        public HearingRequest HearingRequest { get; set; }
        public bool IsRequested { get; set; }
        public bool IsAuthorized { get; set; }
    }
}
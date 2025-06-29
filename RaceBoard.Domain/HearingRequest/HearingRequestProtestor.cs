namespace RaceBoard.Domain
{
    public class HearingRequestProtestor
    {
        public int Id { get; set; }
        public HearingRequest HearingRequest { get; set; }
        public TeamBoat TeamBoat { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public HearingRequestProtestorNotice Notice { get; set; }
    }
}
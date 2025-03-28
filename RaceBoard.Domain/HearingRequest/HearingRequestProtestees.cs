namespace RaceBoard.Domain
{
    public class HearingRequestProtestees
    {
        public HearingRequest HearingRequest { get; set; }
        public List<HearingRequestProtestee> Protestees { get; set; }
    }
}
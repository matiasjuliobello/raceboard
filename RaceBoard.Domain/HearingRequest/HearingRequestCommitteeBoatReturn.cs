namespace RaceBoard.Domain
{
    public class HearingRequestCommitteeBoatReturn
    {
        public int Id { get; set; }
        public HearingRequest HearingRequest { get; set; }
        public CommitteeBoatReturn CommitteeBoatReturn { get; set; }
    }
}

namespace RaceBoard.Domain
{
    public class HearingRequestProtestorNotice
    {
        public int Id { get; set; }
        public HearingRequest HearingRequest { get; set; }
        public bool Hailing { get; set; }
        public string HailingWhen { get; set; }
        public string HailingWordsUsed { get; set; }
        public bool RedFlag { get; set; }
        public string RedFlagWhen { get; set; }
        public bool Other { get; set; }
        public string OtherWhen { get; set; }
        public string OtherWhere { get; set; }
        public string OtherHow { get; set; }
    }
}
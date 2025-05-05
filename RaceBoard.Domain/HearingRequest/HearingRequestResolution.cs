namespace RaceBoard.Domain
{
    public class HearingRequestResolution
    {
        public int Id { get; set; }
        public HearingRequest HearingRequest { get; set; }
        public bool CommissionAcceptsShipSchematic { get; set; }
        public bool CommissionAttachesOwnSchematic { get; set; }
        public string Comments { get; set; }
        public bool Dismissed { get; set; }
        public bool ProtestedBoatsAreDisqualified { get; set; }
        public bool PenaltiesAreAssessed { get; set; }
        public string PenaltiesDescription { get; set; }
    }
}

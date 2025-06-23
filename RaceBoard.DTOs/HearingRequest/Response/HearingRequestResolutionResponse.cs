namespace RaceBoard.DTOs.HearingRequest.Response
{
    public class HearingRequestResolutionResponse
    {
        public int Id { get; set; }
        public string AcceptedFacts { get; set; }
        public bool CommissionAcceptsShipSchematic { get; set; }
        public bool CommissionAttachesOwnSchematic { get; set; }
        public string Comments { get; set; }
        public bool Dismissed { get; set; }
        public bool ProtestedBoatsAreDisqualified { get; set; }
        public bool PenaltiesAreAssessed { get; set; }
        public string PenaltiesDescription { get; set; }
        public string CommissionChairmanAndOthers { get; set; }
        public DateTimeOffset ResolutionDate { get; set; }
    }
}
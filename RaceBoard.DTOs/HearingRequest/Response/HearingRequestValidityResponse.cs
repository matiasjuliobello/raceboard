namespace RaceBoard.DTOs.HearingRequest.Response
{
    public class HearingRequestValidityResponse
    {
        public int Id { get; set; }
        public bool IsInterestedPartyObjection { get; set; }
        public string? InterestedPartyObjectionObservations { get; set; }
        public bool DidProtestIdentifyIncident { get; set; }
        public string? ProtestIdentifyObservations { get; set; }
        public bool WasObjectionSaidAloud { get; set; }
        public string? ObjectionSaidAloudObservations { get; set; }
        public bool DidProtestorGiveNotice { get; set; }
        public string? ProtestorGiveNoticeObservations { get; set; }
        public bool WasRedFlagWasDisplayed { get; set; }
        public string? RedFlagWasDisplayedObservations { get; set; }
        public bool WasRedFlagSeenByRaceCommission { get; set; }
        public string? RedFlagSeenByRaceCommissionObservations { get; set; }
        public bool IsValidProtest { get; set; }
    }
}
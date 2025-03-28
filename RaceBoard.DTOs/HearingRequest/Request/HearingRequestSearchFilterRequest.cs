namespace RaceBoard.DTOs.HearingRequest.Request
{
    public class HearingRequestSearchFilterRequest
    {
        public int[] Ids { get; set; }
        public int IdTeam { get; set; }
        public DateTimeOffset CreationDate { get; set; }
        //public int? IdStatus { get; set; }
        //public int? IdType { get; set; }
        public int? IdRequestUser { get; set; }
        public string? RaceNumber { get; set; }
        public HearingRequestProtestorRequest? Protestor { get; set; }
        public HearingRequestProtesteesRequest? Protestees { get; set; }
    }
}
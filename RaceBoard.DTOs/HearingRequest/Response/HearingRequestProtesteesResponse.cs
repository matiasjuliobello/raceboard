namespace RaceBoard.DTOs.HearingRequest.Response
{
    public class HearingRequestProtesteesResponse
    {
        public int Id { get; set; }
        public List<HearingRequestProtesteeResponse> Protestees {  get; set; }
    }
}

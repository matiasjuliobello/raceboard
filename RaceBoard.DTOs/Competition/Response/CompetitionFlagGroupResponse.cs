namespace RaceBoard.DTOs.Competition.Response
{
    public class CompetitionFlagGroupResponse
    {
        public int Id { get; set; }
        public CompetitionSimpleResponse Competition { get; set; }
        public List<CompetitionFlagResponse> Flags { get; set; }
    }
}

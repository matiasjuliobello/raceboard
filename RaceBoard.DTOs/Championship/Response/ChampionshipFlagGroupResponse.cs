namespace RaceBoard.DTOs.Championship.Response
{
    public class ChampionshipFlagGroupResponse
    {
        public int Id { get; set; }
        public ChampionshipSimpleResponse Championship { get; set; }
        public List<ChampionshipFlagResponse> Flags { get; set; }
    }
}

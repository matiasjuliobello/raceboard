namespace RaceBoard.DTOs.Championship.Request
{
    public class ChampionshipFlagGroupRequest
    {
        public int Id { get; set; }
        public int IdChampionship { get; set; }
        public ChampionshipFlagRequest[] Flags { get; set; }
    }
}

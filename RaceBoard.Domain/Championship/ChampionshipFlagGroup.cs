namespace RaceBoard.Domain
{
    public class ChampionshipFlagGroup
    {
        public int Id { get; set; }
        public Championship Championship { get; set; }
        public List<ChampionshipFlag> Flags { get; set; }

        public ChampionshipFlagGroup()
        {
            Flags = new List<ChampionshipFlag>();
        }
    }
}
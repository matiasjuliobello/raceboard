namespace RaceBoard.Domain
{
    public class ChampionshipCommitteeBoatReturn
    {
        public int Id { get; set; }
        public Championship Championship { get; set; }
        public List<RaceClass> RaceClasses { get; set; }
        public DateTimeOffset ReturnTime { get; set; }
        public string Name { get; set; }

        public ChampionshipCommitteeBoatReturn()
        {
            RaceClasses = new List<RaceClass>();
        }
    }
}

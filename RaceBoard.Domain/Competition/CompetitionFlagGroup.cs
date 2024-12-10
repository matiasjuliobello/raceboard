namespace RaceBoard.Domain
{
    public class CompetitionFlagGroup
    {
        public int Id { get; set; }
        public Competition Competition { get; set; }
        public List<CompetitionFlag> Flags { get; set; }

        public CompetitionFlagGroup()
        {
            Flags = new List<CompetitionFlag>();
        }
    }
}
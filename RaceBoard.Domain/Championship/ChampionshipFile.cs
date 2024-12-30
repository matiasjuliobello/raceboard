namespace RaceBoard.Domain
{
    public class ChampionshipFile
    {
        public int Id { get; set; }
        public Championship Championship { get; set; }
        public List<RaceClass> RaceClasses { get; set; }
        public File File { get; set; }
        public FileType FileType { get; set; }

        public ChampionshipFile()
        {
            this.RaceClasses = new List<RaceClass>();
        }
    }
}

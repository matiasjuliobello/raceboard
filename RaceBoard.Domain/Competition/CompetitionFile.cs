namespace RaceBoard.Domain
{
    public class CompetitionFile
    {
        public int Id { get; set; }
        public Competition Competition { get; set; }
        public List<RaceClass> RaceClasses { get; set; }
        public File File { get; set; }
        public FileType FileType { get; set; }

        public CompetitionFile()
        {
            this.RaceClasses = new List<RaceClass>();
        }
    }
}

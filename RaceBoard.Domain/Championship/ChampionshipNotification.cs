namespace RaceBoard.Domain
{
    public class ChampionshipNotification
    {
        public int Id { get; set; }
        public Championship Championship { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public List<RaceClass> RaceClasses {  get; set; }
        public User CreationUser { get; set; }
        public Person CreationPerson { get; set; }
        public DateTimeOffset CreationDate { get; set; }

        public ChampionshipNotification()
        {
            this.RaceClasses = new List<RaceClass>();
        }
    }
}
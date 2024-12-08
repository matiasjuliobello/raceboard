namespace RaceBoard.Domain
{
    public class CompetitionNotification
    {
        public int Id { get; set; }
        public Competition Competition { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public List<RaceClass> RaceClasses {  get; set; }
        public User CreationUser { get; set; }
        public Person CreationPerson { get; set; }
        public DateTimeOffset CreationDate { get; set; }

        public CompetitionNotification()
        {
            this.RaceClasses = new List<RaceClass>();
        }
    }
}
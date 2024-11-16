namespace RaceBoard.Domain
{
    public class CommitteeBoatReturn
    {
        public int Id { get; set; }
        public Competition Competition { get; set; }
        public List<RaceClass> RaceClasses { get; set; }
        public DateTimeOffset ReturnTime { get; set; }
        public string Name { get; set; }

        public CommitteeBoatReturn()
        {
            this.RaceClasses = new List<RaceClass>();
        }
    }
}

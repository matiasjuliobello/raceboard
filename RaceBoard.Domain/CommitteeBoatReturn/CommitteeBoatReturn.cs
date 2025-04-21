namespace RaceBoard.Domain
{
    public class CommitteeBoatReturn
    {
        public int Id { get; set; }
        public Championship Championship { get; set; }
        public List<RaceClass> RaceClasses { get; set; }
        public DateTimeOffset ReturnTime { get; set; }
        public string Name { get; set; }

        public CommitteeBoatReturn()
        {
            RaceClasses = new List<RaceClass>();
        }
    }
}

namespace RaceBoard.Domain
{
    public class RaceCommitteeBoatReturn
    {
        public int Id { get; set; }
        public Race Race { get; set; }
        public DateTimeOffset Return { get; set; }
        public string Name { get; set; }
    }
}

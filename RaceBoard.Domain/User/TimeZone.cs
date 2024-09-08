namespace RaceBoard.Domain
{
    public class TimeZone : AbstractEntity
    {
        public string Name { get; set; }
        public string Identifier { get; set; }
        public string Offset { get; set; }
    }
}
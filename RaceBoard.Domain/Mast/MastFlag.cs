namespace RaceBoard.Domain
{
    public class MastFlag : AbstractEntity
    {
        public Mast Mast { get; set; }
        public Flag Flag { get; set; }
        public Person Person { get; set; }
        public DateTimeOffset RaisingMoment { get; set; }
        public DateTimeOffset LoweringMoment { get; set; }
        public bool IsActive { get; set; }
    }
}
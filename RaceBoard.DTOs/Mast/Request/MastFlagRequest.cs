namespace RaceBoard.DTOs.Mast.Request
{
    public class MastFlagRequest
    {
        public int Id { get; set; }
        public int IdMast { get; set; }
        public int IdFlag { get; set; }
        public int IdPerson { get; set; }
        public DateTimeOffset RaisingMoment { get; set; }
        public DateTimeOffset LoweringMoment { get; set; }
        public bool IsActive { get; set; }
    }
}

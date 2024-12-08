namespace RaceBoard.DTOs.Mast.Request
{
    public class MastFlagRequest
    {
        public int Id { get; set; }
        public int IdMast { get; set; }
        public int IdCompetition { get; set; }
        public int IdFlag { get; set; }
        //public DateTimeOffset RaisingMoment { get; set; }
        public int? HoursToLower { get; set; }
        public int? MinutesToLower { get; set; }
    }
}

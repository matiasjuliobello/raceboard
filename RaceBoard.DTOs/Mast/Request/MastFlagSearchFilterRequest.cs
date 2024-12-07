namespace RaceBoard.DTOs.Mast.Request
{
    public class MastFlagSearchFilterRequest
    {
        public int? IdMast { get; set; }
        public int? IdCompetition { get; set; }
        public int? IdFlag { get; set; }
        public int? IdPerson { get; set; }
        public DateTimeOffset? RisingMoment { get; set; }
        public DateTimeOffset? LoweringMoment { get; set; }
        public bool? IsActive { get; set; }
    }
}

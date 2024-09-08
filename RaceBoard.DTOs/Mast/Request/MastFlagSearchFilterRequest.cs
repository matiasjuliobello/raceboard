namespace RaceBoard.DTOs.Mast.Request
{
    public class MastFlagSearchFilterRequest
    {
        public int? IdMast { get; set; }
        public int? IdFlag { get; set; }
        public int? IdPerson { get; set; }
        public DateTimeOffset? RaiseTime { get; set; }
        public DateTimeOffset? LowerTime { get; set; }
        public bool? IsActive { get; set; }
    }
}

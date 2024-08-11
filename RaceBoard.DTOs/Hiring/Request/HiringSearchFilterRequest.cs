namespace RaceBoard.DTOs.Hiring.Request
{
    public class HiringSearchFilterRequest
    {
        public string Name { get; set; }
        public int? IdType { get; set; }
        public int[]? IdsRole { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
    }
}
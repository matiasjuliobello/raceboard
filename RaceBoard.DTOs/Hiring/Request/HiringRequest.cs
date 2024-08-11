namespace RaceBoard.DTOs.Hiring.Request
{
    public class HiringRequest
    {
        public int Id { get; set; }
        public int IdUser { get; set; }
        public int IdType { get; set; }
        public int IdRole { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
    }
}

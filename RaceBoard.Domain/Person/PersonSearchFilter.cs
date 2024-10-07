namespace RaceBoard.Domain
{
    public class PersonSearchFilter
    {
        public int[]? Ids { get; set; }
        public int? IdUser { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? EmailAddress { get; set; }
    }
}
namespace RaceBoard.DTOs.Person.Request
{
    public class PersonSearchFilterRequest
    {
        public int[]? Ids { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? EmailAddress { get; set; }
    }
}
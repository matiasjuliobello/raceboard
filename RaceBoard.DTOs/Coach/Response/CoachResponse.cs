using RaceBoard.DTOs.Person.Response;

namespace RaceBoard.DTOs.Coach.Response
{
    public class CoachResponse
    {
        public int Id { get; set; }
        public PersonSimpleResponse Person { get; set; }
        public int OrganizationCount { get; set; }
        public int TeamCount { get; set; }
        public int CoacheeCount { get; set; }
    }
}
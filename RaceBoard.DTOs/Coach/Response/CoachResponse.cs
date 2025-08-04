using RaceBoard.DTOs.Person.Response;

namespace RaceBoard.DTOs.Coach.Response
{
    public class CoachResponse
    {
        public int Id { get; set; }
        public PersonSimpleResponse Person { get; set; }
    }
}
using RaceBoard.DTOs.Person.Response;

namespace RaceBoard.DTOs.Contestant.Response
{
    public class ContestantResponse
    {
        public int Id { get; set; }
        public PersonResponse Person { get; set; }
    }
}
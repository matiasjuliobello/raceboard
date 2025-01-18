using RaceBoard.DTOs.Person.Response;
using RaceBoard.DTOs.User.Response;

namespace RaceBoard.DTOs.ChangeRequest.Response
{
    public class CrewChangeRequestResponse : ChangeRequestResponse
    {
        public UserSimpleResponse ReplacedUser { get; set; }
        public PersonSimpleResponse ReplacedPerson { get; set; }
        public string ReplacementFullName { get; set; }
    }
}
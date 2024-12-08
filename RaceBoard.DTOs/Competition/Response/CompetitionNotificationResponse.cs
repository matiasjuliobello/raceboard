using RaceBoard.DTOs.Person.Response;
using RaceBoard.DTOs.RaceClass.Response;
using RaceBoard.DTOs.User.Response;

namespace RaceBoard.DTOs.Competition.Response
{
    public class CompetitionNotificationResponse
    {
        public int Id {  get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public List<RaceClassResponse> RaceClasses { get; set; }
        public UserSimpleResponse CreationUser { get; set; }
        public PersonSimpleResponse CreationPerson { get; set; }
        public DateTimeOffset CreationDate { get; set; }
    }
}

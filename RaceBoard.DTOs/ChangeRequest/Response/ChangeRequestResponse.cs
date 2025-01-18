using RaceBoard.DTOs.File.Response;
using RaceBoard.DTOs.Person.Response;
using RaceBoard.DTOs.Team.Response;
using RaceBoard.DTOs.User.Response;

namespace RaceBoard.DTOs.ChangeRequest.Response
{
    public abstract class ChangeRequestResponse
    {
        public int Id { get; set; }
        public RequestStatusResponse Status { get; set; }
        public TeamSimpleResponse Team { get; set; }
        public UserSimpleResponse RequestUser { get; set; }
        public PersonSimpleResponse RequestPerson { get; set; }
        public string ChangeRequested { get; set; }
        public string ChangeReason { get; set; }
        public DateTimeOffset CreationDate { get; set; }
        public DateTimeOffset? ResolutionDate { get; set; }
        public string? ResolutionComments { get; set; }
        public FileResponse? File { get; set; }
    }
}
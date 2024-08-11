using RaceBoard.DTOs.ApprovalStatus.Response;
using RaceBoard.DTOs.Permissions.Response;
using RaceBoard.DTOs.User.Response;

namespace RaceBoard.DTOs.Script.Response
{
    public class ScriptApprovalResponse
    {
        public int Id { get; set; }
        public ScriptSimpleResponse Script { get; set; }
        public ApprovalStatusResponse ApprovalStatus { get; set; }
        public RoleResponse Role { get; set; }
        public UserSimpleResponse User { get; set; }
        public DateTimeOffset Date { get; set; }
        public string Comments { get; set; }
    }
}
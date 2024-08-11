using RaceBoard.DTOs.Permissions.Response;
using RaceBoard.DTOs.Script.Response;
using RaceBoard.DTOs.User.Response;

namespace RaceBoard.DTOs.Payroll.Response
{
    public class PayrollResponse
    {
        public ScriptSimpleResponse Script { get; set; }
        public UserSimpleResponse Provider { get; set; }
        public RoleResponse ProviderRole { get; set; }
        public DateTimeOffset CollaborationDate { get; set; }
        public DateTimeOffset? PaymentDate { get; set; }

        public int LoopsCount { get; set; }
        public decimal LoopsAmount { get; set; }
    }
}

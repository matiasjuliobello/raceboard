using RaceBoard.DTOs.Hiring.Response;
using RaceBoard.DTOs.Permissions.Response;
using RaceBoard.DTOs.Script.Response;
using RaceBoard.DTOs.User.Response;

namespace RaceBoard.DTOs.Payment.Response
{
    public class PaymentResponse
    {
        public int Id { get; set; }
        public ScriptSimpleResponse Script { get; set; }
        public PaymentStatusResponse Status { get; set; }
        public PaymentMethodResponse? Method { get; set; }
        public UserSimpleResponse Provider { get; set; }
        public RoleResponse ProviderRole { get; set; }
        public HiringTypeResponse ProviderHiringType { get; set; }
        public int LoopsCount { get; set; }
        public decimal LoopsAmount { get; set; }
        public int NonLoopsCount { get; set; }
        public decimal NonLoopsAmount { get; set; }
        public DateTimeOffset CollaborationDate { get; set; }
        public DateTimeOffset? PaymentDate { get; set; }
    }
}

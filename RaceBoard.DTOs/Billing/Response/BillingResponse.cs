using RaceBoard.DTOs.Payment.Response;
using RaceBoard.DTOs.Script.Response;

namespace RaceBoard.DTOs.Billing.Response
{
    public class BillingResponse
    {
        public ScriptSimpleResponse Script { get; set; }
        public PaymentMethodResponse? PaymentMethod { get; set; }
        public PaymentStatusResponse PaymentStatus { get; set; }
        public int TotalPayments { get; set; }
        public int TotalLoops { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTimeOffset? PaymentDate { get; set; }
    }
}
namespace RaceBoard.DTOs.Payment.Request
{
    public class PaymentComplaintSearchFilterRequest
    {
        public int[] IdsPayment { get; set; }
        public int? IdScript { get; set; }
        public int? IdPaymentComplaintStatus { get; set; }
        public int? IdUser {  get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
    }
}

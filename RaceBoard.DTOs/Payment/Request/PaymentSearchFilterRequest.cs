namespace RaceBoard.DTOs.Payment.Request
{
    public class PaymentSearchFilterRequest
    {
        public int[]? Ids { get; set; }
        public int[]? IdsScript { get; set; }
        public int? IdPaymentStatus { get; set; }
        public int? IdPaymentMethod { get; set; }
        public int? IdProvider { get; set; }
        public int? IdProviderRole { get; set; }
        public int? IdProviderHiringType { get; set; }
        public DateTimeOffset? PaymentDate { get; set; }
    }
}

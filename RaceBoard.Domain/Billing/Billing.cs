namespace RaceBoard.Domain
{
    public class Billing
    {
        public Script Script { get; set; }
        public PaymentMethod? PaymentMethod { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public int TotalPayments { get; set; }
        public int TotalLoops { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTimeOffset? PaymentDate { get; set; }
    }
}
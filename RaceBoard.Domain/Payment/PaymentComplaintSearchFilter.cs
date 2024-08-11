namespace RaceBoard.Domain
{
    public class PaymentComplaintSearchFilter
    {
        public int? Id { get; set; }
        public List<Payment>? Payments { get; set; }
        public Script? Script { get; set; }
        public PaymentComplaintStatus? Status { get; set; }
        public User? User { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }

        public PaymentComplaintSearchFilter()
        {
            this.Payments = new List<Payment>();
        }
    }
}
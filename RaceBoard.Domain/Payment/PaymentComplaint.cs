
namespace RaceBoard.Domain
{
    public class PaymentComplaint
    {
        public int Id { get; set; }
        public Payment Payment { get; set; }
        public PaymentComplaintStatus Status { get; set; }
        public List<PaymentComplaintItem> Items { get; set; }
    }
}

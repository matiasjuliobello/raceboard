namespace RaceBoard.Domain
{
    public class PaymentComplaintItem
    {
        public int Id { get; set; }
        public PaymentComplaint PaymentComplaint { get; set; }
        public User User { get; set; }
        public DateTimeOffset Date { get; set; }
        public string Message { get; set; }
    }
}

namespace RaceBoard.DTOs.Payment.Request
{
    public class PaymentComplaintRequest
    {
        public int IdPayment { get; set; }
        public int IdStatus { get; set; }
        public string Message { get; set; }
    }
}

namespace RaceBoard.DTOs.Payment.Response
{
    public class PaymentComplaintResponse
    {
        public int Id { get; set; }
        public PaymentResponse Payment { get; set; }
        public PaymentComplaintStatusResponse Status { get; set; }
        public List<PaymentComplaintItemResponse> Items { get; set; }
    }
}

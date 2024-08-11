using RaceBoard.DTOs.User.Response;

namespace RaceBoard.DTOs.Payment.Response
{
    public class PaymentComplaintItemResponse
    {
        public int Id { get; set; }
        public PaymentComplaintResponse PaymentComplaint { get; set; }
        public UserSimpleResponse User { get; set; }
        public DateTimeOffset Date { get; set; }
        public string Message { get; set; }
    }
}


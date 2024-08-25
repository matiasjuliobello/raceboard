namespace RaceBoard.DTOs.User.Request
{
    public class UserIdentificationRequest
    {
        public int Id { get; set; }
        public int? IdUser { get; set; }
        public int? IdType { get; set; }
        public string Number { get; set; }
    }
}

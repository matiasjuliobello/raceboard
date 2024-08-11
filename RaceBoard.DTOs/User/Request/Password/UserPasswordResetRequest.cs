namespace RaceBoard.DTOs.User.Request.Password
{
    public class UserPasswordResetRequest
    {
        public string EmailAddress { get; set; }
        public string Token { get; set; }
        public string Password {  get; set; }
    }
}

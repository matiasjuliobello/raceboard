namespace RaceBoard.DTOs.Authentication.Response
{
    public class AccessTokenResponse
    {
        public string Token { get; set; }
        public DateTime ExpiresAt {  get; set; } 
    }
}

namespace RaceBoard.DTOs.User.Request
{
    public class UserSearchFilterRequest
    {
        public int[]? Ids { get; set; }
        public string Username { get; set; }
        public string EmailAddress { get; set; }
    }
}
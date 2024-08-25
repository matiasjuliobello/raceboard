namespace RaceBoard.DTOs.User.Response
{
    public class UserSimpleResponse
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
    }
}
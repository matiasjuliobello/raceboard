using RaceBoard.DTOs.Person.Request;

namespace RaceBoard.DTOs.User.Request
{
    public class UserRequest
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public PersonRequest Person { get; set; }
        public int IdRole { get; set; }
    }
}

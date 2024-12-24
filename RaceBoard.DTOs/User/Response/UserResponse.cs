namespace RaceBoard.DTOs.User.Response
{
    public class UserResponse
    {
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Fullname { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public DateTimeOffset BirthDate { get; set; }
        public bool IsActive { get; set; }
        public List<UserIdentificationResponse> Identifications {  get; set; }
        public UserRoleResponse Role {  get; set; }
    }
}

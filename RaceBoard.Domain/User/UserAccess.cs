namespace RaceBoard.Domain
{
    public class UserAccess
    {
        public int Id { get; set; }
        public User User {  get; set; }
        public Role Role { get; set; }
        public Competition Competition { get; set; }
    }
}
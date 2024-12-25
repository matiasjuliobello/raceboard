namespace RaceBoard.Domain
{
    public class TeamMemberInvitation
    {
        public int Id { get; set; }
        public Team Team { get; set; }
        public Role Role { get; set; }
        public User RequestUser { get; set; }
        public User? User { get; set; }
        public Person? Person { get; set; }
        public DateTimeOffset RequestDate { get; set; }
        public bool IsPending { get; set; }
        public Invitation Invitation { get; set; }

        public TeamMemberInvitation()
        {
            this.Invitation = new Invitation();
        }
    }
}
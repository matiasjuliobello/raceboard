namespace RaceBoard.Domain
{
    public class CompetitionMemberInvitation
    {
        public int Id { get; set; }
        public Competition Competition { get; set; }
        public Role Role { get; set; }
        public User RequestUser { get; set; }
        public User? User { get; set; }
        public Person? Person { get; set; }
        public DateTimeOffset RequestDate { get; set; }
        public bool IsPending { get; set; }
        public Invitation Invitation { get; set; }

        public CompetitionMemberInvitation()
        {
            this.Invitation = new Invitation();
        }
    }
}
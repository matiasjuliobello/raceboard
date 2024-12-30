namespace RaceBoard.Domain
{
    public class ChampionshipMemberInvitation
    {
        public int Id { get; set; }
        public Championship Championship { get; set; }
        public Role Role { get; set; }
        public User RequestUser { get; set; }
        public User? User { get; set; }
        public Person? Person { get; set; }
        public DateTimeOffset RequestDate { get; set; }
        public bool IsPending { get; set; }
        public Invitation Invitation { get; set; }

        public ChampionshipMemberInvitation()
        {
            this.Invitation = new Invitation();
        }
    }
}
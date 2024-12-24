namespace RaceBoard.Domain
{
    public class OrganizationMemberInvitation
    {
        public int Id { get; set; }
        public Organization Organization { get; set; }
        public Role Role { get; set; }
        public User RequestUser { get; set; }
        public User? User { get; set; }
        public Person? Person { get; set; }
        public DateTimeOffset RequestDate { get; set; }
        public bool IsPending { get; set; }
        public Invitation Invitation {  get; set; }

        public OrganizationMemberInvitation()
        {
            this.Invitation = new Invitation();
        }
    }
}
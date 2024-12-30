namespace RaceBoard.Domain
{
    public class Team
    {
        public int Id { get; set; }
        public Organization Organization { get; set; }
        public Championship Championship { get; set; }
        public RaceClass RaceClass { get; set; }
        public Boat Boat { get; set; }
        public List<TeamMember> Members { get; set; }
        //public Coach Coach { get; set; }

        public User CreationUser { get; set; }

        public Team()
        {
            this.Members = new List<TeamMember>();
        }
    }
}
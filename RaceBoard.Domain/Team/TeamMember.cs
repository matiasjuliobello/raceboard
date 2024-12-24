namespace RaceBoard.Domain
{
    public class TeamMember
    {
        public int Id { get; set; }
        public Team Team {  get; set; }
        public Person Person { get; set; }
        public TeamMemberRole Role { get; set; }
    }
}

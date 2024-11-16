namespace RaceBoard.Domain
{
    public class TeamContestant
    {
        public int Id { get; set; }
        public Team Team {  get; set; }
        public Person Person { get; set; }
        public ContestantRole Role { get; set; }
    }
}

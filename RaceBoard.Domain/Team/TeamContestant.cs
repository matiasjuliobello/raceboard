namespace RaceBoard.Domain
{
    public class TeamContestant : AbstractEntity
    {
        public Team Team {  get; set; }
        public Contestant Contestant { get; set; }
        public ContestantRole Role { get; set; }
    }
}

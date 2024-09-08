namespace RaceBoard.Domain
{
    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Competition Competition { get; set; }
        public RaceClass RaceClass { get; set; }
        public Boat Boat { get; set; }
        public List<TeamContestant> Contestants { get; set; }
        //public Coach Coach { get; set; }
    }
}
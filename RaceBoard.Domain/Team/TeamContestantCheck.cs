using RaceBoard.Domain._Enums;

namespace RaceBoard.Domain
{
    public class TeamContestantCheck
    {
        public int Id { get; set; }
        public TeamContestant TeamContestant { get; set; }
        //public Person Person { get; set; }
        public Competition Competition { get; set; }
        public CheckType CheckType { get; set; }
        public DateTimeOffset CheckTime { get; set; }
    }
}

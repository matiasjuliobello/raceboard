using RaceBoard.Domain.Enums;

namespace RaceBoard.Domain
{
    public class TeamMemberCheck
    {
        public int Id { get; set; }
        public TeamMember TeamMember { get; set; }
        //public Person Person { get; set; }
        public Competition Competition { get; set; }
        public TeamMemberCheckType CheckType { get; set; }
        public DateTimeOffset CheckTime { get; set; }
    }
}

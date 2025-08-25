namespace RaceBoard.Domain
{
    public class Coach
    {
        public int Id { get; set; }
        public Person Person { get; set; }
        public int OrganizationCount { get; set; }
        public int TeamCount { get; set; }
        public int CoacheeCount { get; set; }
    }
}
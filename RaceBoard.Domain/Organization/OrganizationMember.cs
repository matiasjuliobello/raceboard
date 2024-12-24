namespace RaceBoard.Domain
{
    public class OrganizationMember
    {
        public int Id { get; set; }
        public Organization Organization { get; set; }
        public User User { get; set; }
        public Person Person {  get; set; }
        public Role Role { get; set; }
        public bool IsActive { get; set; }
        public DateTimeOffset JoinDate { get; set; }
    }
}
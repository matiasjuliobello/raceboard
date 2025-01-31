﻿namespace RaceBoard.Domain
{
    public class TeamMember
    {
        public int Id { get; set; }
        public Team Team { get; set; }
        public User User { get; set; }
        public Person Person { get; set; }
        public TeamMemberRole Role { get; set; }
        public bool IsActive { get; set; }
        public DateTimeOffset JoinDate { get; set; }
    }
}
﻿namespace RaceBoard.Domain
{
    public class ChampionshipFlag
    {
        public int Id { get; set; }
        public ChampionshipFlagGroup Group { get; set; }
        public Flag Flag { get; set; }
        public DateTimeOffset Raising { get; set; }
        public DateTimeOffset? Lowering { get; set; }
        public int? HoursToLower { get; set; }
        public int? MinutesToLower { get; set; }
        public int Order { get; set; }
        public Person Person { get; set; }
        public User User { get; set; }
    }
}
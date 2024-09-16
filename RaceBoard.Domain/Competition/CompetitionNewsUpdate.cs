﻿namespace RaceBoard.Domain
{
    public class CompetitionNewsUpdate
    {
        public int Id { get; set; }
        public Competition Competition { get; set; }
        public string Message { get; set; }
        public DateTimeOffset Timestamp { get; set; }
    }
}
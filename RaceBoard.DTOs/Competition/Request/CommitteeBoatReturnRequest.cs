﻿namespace RaceBoard.DTOs.Competition.Request
{
    public class CommitteeBoatReturnRequest
    {
        public int Id { get; set; }
        public int IdCompetition { get; set; }
        public int[] IdsRaceClass { get; set; }
        public DateTimeOffset ReturnTime { get; set; }
        public string Name { get; set; }
    }
}
﻿namespace RaceBoard.Domain
{
    public class Boat
    {
        public int Id { get; set; }
        public RaceClass RaceClass { get; set; }
        public BoatOrganization? Organization { get; set; }
        public string Name { get; set; }
        public string SailNumber { get; set; }
        public string HullNumber { get; set; }
        
    }
}

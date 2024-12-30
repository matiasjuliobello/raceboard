namespace RaceBoard.Domain
{
    public class ChampionshipGroup
    {
        public int Id { get; set; }
        public Championship Championship { get; set; }
        public string Name { get; set; }
        public DateTimeOffset ChampionshipStartDate { get; set; }
        public DateTimeOffset ChampionshipEndDate { get; set; }
        public DateTimeOffset AccreditationStartDate { get; set; }
        public DateTimeOffset AccreditationEndDate { get; set; }
        public DateTimeOffset RegistrationStartDate { get; set; }
        public DateTimeOffset RegistrationEndDate { get; set; }
        public List<RaceClass> RaceClasses { get; set; }

        public int RegistrationTotalCount { get; set; }
        public int AccreditationTotalCount { get; set; }
        public int ChampionshipTotalCount { get; set; }

        public ChampionshipGroup()
        {
            this.RaceClasses = new List<RaceClass>();
        }
    }
}
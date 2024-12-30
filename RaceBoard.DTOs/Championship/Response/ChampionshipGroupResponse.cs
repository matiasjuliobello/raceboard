namespace RaceBoard.DTOs.Championship.Response
{
    public class ChampionshipGroupResponse
    {
        public int Id { get; set; }
        public ChampionshipSimpleResponse Championship { get; set; }
        public string Name { get; set; }
        public DateTimeOffset ChampionshipStartDate { get; set; }
        public DateTimeOffset ChampionshipEndDate { get; set; }
        public DateTimeOffset AccreditationStartDate { get; set; }
        public DateTimeOffset AccreditationEndDate { get; set; }
        public DateTimeOffset RegistrationStartDate { get; set; }
        public DateTimeOffset RegistrationEndDate { get; set; }
        public int RegistrationTotalCount { get; set; }
        public int AccreditationTotalCount { get; set; }
        public int ChampionshipTotalCount { get; set; }

        public List<ChampionshipRaceClassResponse> RaceClasses { get; set; }
    }
}
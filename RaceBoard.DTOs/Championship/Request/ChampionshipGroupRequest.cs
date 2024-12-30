using RaceBoard.DTOs.RaceClass.Request;

namespace RaceBoard.DTOs.Championship.Request
{
    public class ChampionshipGroupRequest
    {
        public int Id { get; set; }
        public int IdChampionship { get; set; }
        public string Name { get; set; }
        public DateTimeOffset ChampionshipStartDate { get; set; }
        public DateTimeOffset ChampionshipEndDate { get; set; }
        public DateTimeOffset AccreditationStartDate { get; set; }
        public DateTimeOffset AccreditationEndDate { get; set; }
        public DateTimeOffset RegistrationStartDate { get; set; }
        public DateTimeOffset RegistrationEndDate { get; set; }
        public List<RaceClassRequest> RaceClasses { get; set; }
    }
}

namespace RaceBoard.DTOs.Race.Request
{
    public class RaceCommitteeBoatReturnRequest
    {
        public int Id { get; set; }
        public int IdRace { get; set; }
        public DateTimeOffset Return { get; set; }
        public string Name { get; set; }
    }
}

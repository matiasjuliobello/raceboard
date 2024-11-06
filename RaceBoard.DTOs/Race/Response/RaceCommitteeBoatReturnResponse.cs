namespace RaceBoard.DTOs.Race.Response
{
    public class RaceCommitteeBoatReturnResponse
    {
        public int Id { get; set; }
        public RaceResponse Race { get; set; }
        public DateTimeOffset Return { get; set; }
        public string Name { get; set; }
    }
}

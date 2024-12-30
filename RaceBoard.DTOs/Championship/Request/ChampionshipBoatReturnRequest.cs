namespace RaceBoard.DTOs.Championship.Request
{
    public class ChampionshipBoatReturnRequest
    {
        public int Id { get; set; }
        public int IdChampionship { get; set; }
        public int[] IdsRaceClass { get; set; }
        public DateTimeOffset ReturnTime { get; set; }
        public string Name { get; set; }
    }
}

namespace RaceBoard.DTOs.RaceClass.Request
{
    public class RaceClassSearchFilterRequest
    {
        public int[] Ids { get; set; }
        public int IdCategory {  get; set; }
        public string Name { get; set; }
    }
}

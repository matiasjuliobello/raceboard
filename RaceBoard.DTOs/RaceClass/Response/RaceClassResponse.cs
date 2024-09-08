using RaceBoard.DTOs.RaceCategory.Response;

namespace RaceBoard.DTOs.RaceClass.Response
{
    public class RaceClassResponse
    {
        public int Id { get; set; }
        public RaceCategoryResponse RaceCategory { get; set; }
        public string Name { get; set; }
    }
}

namespace RaceBoard.Domain
{
    public class ChampionshipFileSearchFilter
    {
        public int[]? Ids {  get; set; }
        public FileType? FileType { get; set; }
        public Championship? Championship { get; set; }
        public List<RaceClass>? RaceClasses { get; set; }
        public string? Description { get; set; }
        public DateTimeOffset? UploadTime { get; set; }
    }
}

namespace RaceBoard.Domain
{
    public class CompetitionFileSearchFilter
    {
        public int[]? Ids {  get; set; }
        public FileType? FileType { get; set; }
        public Competition? Competition { get; set; }
        public List<RaceClass>? RaceClasses { get; set; }
        public string? Description { get; set; }
        public DateTimeOffset? UploadTime { get; set; }
    }
}
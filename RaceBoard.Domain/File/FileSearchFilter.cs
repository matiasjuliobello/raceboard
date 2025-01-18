namespace RaceBoard.Domain
{
    public class FileSearchFilter
    {
        public int[] Ids {  get; set; }
        public User? CreationUser { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        //public FileType? Type { get; set; }
        public DateTimeOffset? CreationDate { get; set; }
    }
}
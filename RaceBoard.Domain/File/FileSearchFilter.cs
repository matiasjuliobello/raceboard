namespace RaceBoard.Domain
{
    public class FileSearchFilter
    {
        public User CreationUser { get; set; }
        public string PhysicalName { get; set; }
        //public FileType? Type { get; set; }
        public DateTimeOffset CreationDate { get; set; }
    }
}
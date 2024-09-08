
namespace RaceBoard.Domain
{
    public class File
    {
        public int Id { get; set; }
        public string PhysicalName { get; set; }
        public User CreationUser { get; set; }
        public DateTimeOffset CreationDate { get; set; }
        public FileType Type { get; set; }
    }
}


namespace RaceBoard.Domain
{
    public class File : AbstractEntity
    {
        public string PhysicalName { get; set; }
        public User CreationUser { get; set; }
        public DateTimeOffset CreationDate { get; set; }
        public FileType Type { get; set; }
    }
}

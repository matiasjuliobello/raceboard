
namespace RaceBoard.Domain
{
    public class File
    {
        public int Id { get; set; }
        public string Description {  get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public User CreationUser { get; set; }
        public Person CreationPerson {  get; set; }
        public DateTimeOffset CreationDate { get; set; }
        public byte[] Content { get; set; }
    }
}

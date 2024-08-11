using RaceBoard.Domain.StudioManagement;

namespace RaceBoard.Domain
{
    public class Studio
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsEnabled { get; set; }
        public Customer Customer { get; set; }
    }
}

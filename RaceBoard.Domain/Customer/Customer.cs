namespace RaceBoard.Domain
{
    public class Customer
    {
        public string Name { get; set; }
        public bool IsEnabled { get; set; }
        public Country Country { get; set; }
    }
}

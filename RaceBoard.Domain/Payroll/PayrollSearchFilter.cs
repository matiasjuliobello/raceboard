namespace RaceBoard.Domain
{
    public class PayrollSearchFilter
    {
        public Script Script { get; set; }
        public User Provider { get; set; }
        public Role ProviderRole { get; set; }
        public DateTimeOffset Date { get; set; }
        public int Loops { get; set; }
        public decimal Amount { get; set; }
    }
}
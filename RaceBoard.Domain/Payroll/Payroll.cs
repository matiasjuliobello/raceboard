namespace RaceBoard.Domain
{
    public class Payroll
    {
        public Script Script { get; set; }
        public User Provider { get; set; }
        public Role ProviderRole { get; set; }
        public DateTimeOffset CollaborationDate { get; set; }
        public DateTimeOffset? PaymentDate { get; set; }
        public int LoopsCount { get; set; }
        public decimal LoopsAmount { get; set; }
    }
}
namespace RaceBoard.Domain
{
    public class Payment
    {
        public int Id { get; set; }
        public Script Script { get; set; }
        public PaymentStatus Status { get; set; }
        public PaymentMethod? Method { get; set; }
        public User Provider { get; set; }
        public Role ProviderRole { get; set; }

        public int IdProviderHiringType { get; set; }
        public HiringType ProviderHiringType { get; set; }
        public int LoopsCount { get; set; }
        public decimal LoopsAmount { get; set; }
        public int NonLoopsCount { get; set; }
        public decimal NonLoopsAmount { get; set; }
        public DateTimeOffset CollaborationDate { get; set; }
        public DateTimeOffset? PaymentDate { get; set; }
    }
}

namespace RaceBoard.Domain
{
    public class PaymentSearchFilter
    {
        public int[]? Ids { get; set; }
        public List<Script>? Scripts { get; set; }
        public PaymentStatus? Status { get; set; }
        public PaymentMethod? Method { get; set; }
        public User? Provider { get; set; }
        public Role? ProviderRole { get; set; }
        public HiringType? ProviderHiringType { get; set; }
        public int? LoopsCount { get; set; }
        public decimal? LoopsAmount { get; set; }
        public int? NonLoopsCount { get; set; }
        public decimal? NonLoopsAmount { get; set; }
        public DateTimeOffset? CollaborationDate { get; set; }
        public DateTimeOffset? PaymentDate { get; set; }
    }
}
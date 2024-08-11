namespace RaceBoard.Business.Entities
{
    public class PaymentCalculationData
    {
        public int SongCount { get; set; }
        public int ChorusCount { get; set; }
        public int WordCount { get; set; }
        public int LoopCount { get; set; }
        public decimal NonLoopAmount { get; set; }
        public decimal LoopAmount { get; set; }
        //public decimal ImporteTotal { get; set; }
        public int IdScript { get; set; }
        public int IdUserProvider { get; set; }
        public int IdUserRole { get; set; }
        public int IdHiringType { get; set; }
        public DateTimeOffset Date { get; set; }
    }
}

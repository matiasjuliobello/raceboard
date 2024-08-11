namespace RaceBoard.Domain
{
    public class SpendingItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public SpendingItemCategory Category { get; set; }
    }
}
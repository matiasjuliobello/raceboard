namespace RaceBoard.DTOs.Spending.Response
{
    public class SpendingItemResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public SpendingItemCategoryResponse Category { get; set; }
    }
}
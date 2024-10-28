namespace RaceBoard.Domain
{
    public class Device
    {
        public int Id { get; set; }
        public Platform Platform { get; set; }
        public string Token { get; set; }
        public DateTimeOffset CreationDate { get; set; }
        public DateTimeOffset LastUpdateDate { get; set; }
    }
}

namespace RaceBoard.Domain
{
    public class Identification : AbstractEntity
    {
        public string Number { get; set; }
        public IdentificationType Type { get; set; }
    }
}
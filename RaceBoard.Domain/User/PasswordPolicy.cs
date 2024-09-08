namespace RaceBoard.Domain
{
    public class PasswordPolicy : AbstractEntity
    {
        public int VersionNumber { get; set; }
        public DateTimeOffset CreationDate { get; set; }
        public int MinLowercaseChars { get; set; }
        public int MinUppercaseChars { get; set; }
        public int MinSpecialChars { get; set; }
        public int MinNumericChars { get; set; }
        public int MinLength { get; set; }
        public int MaxLength { get; set; }
    }
}
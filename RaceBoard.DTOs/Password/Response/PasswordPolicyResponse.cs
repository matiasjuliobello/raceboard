namespace RaceBoard.DTOs.Password.Response
{
    public class PasswordPolicyResponse
    {
        public int VersionNumber { get; }
        public DateTimeOffset CreationDate { get; }
        public int MinLowercaseChars { get; }
        public int MinUppercaseChars { get; }
        public int MinSpecialChars { get; }
        public int MinNumericChars { get; }
        public int PasswordMinLength { get; }
        public int PasswordMaxLength { get; }
    }
}
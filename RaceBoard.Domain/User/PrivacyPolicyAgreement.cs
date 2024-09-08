namespace RaceBoard.Domain
{
    public class PrivacyPolicyAgreement
    {
        public int Id { get; set; }
        public PrivacyPolicy PrivacyPolicy { get; set; }
        public User User { get; set; }
        public DateTimeOffset AgreementDate { get; set; }
    }
}
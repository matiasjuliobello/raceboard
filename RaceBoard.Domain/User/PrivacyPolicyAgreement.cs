namespace RaceBoard.Domain
{
    public class PrivacyPolicyAgreement : AbstractEntity
    {
        public PrivacyPolicy PrivacyPolicy { get; set; }
        public User User { get; set; }
        public DateTimeOffset AgreementDate { get; set; }
    }
}
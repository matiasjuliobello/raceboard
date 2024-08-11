namespace RaceBoard.DTOs.User.Response
{
    public class PrivacyPolicyAgreementResponse
    {
        public int Id { get; set; }
        public int IdPrivacyPolicy { get; set; }
        public int IdUser { get; set; }
        public DateTimeOffset AgreementDate { get; set; }
    }
}

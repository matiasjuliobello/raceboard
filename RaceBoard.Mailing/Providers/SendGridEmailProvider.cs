using RaceBoard.Mailing.BaseClasses;
using RaceBoard.Mailing.Enums;
using Microsoft.Extensions.Configuration;

namespace RaceBoard.Mailing.Providers
{
    // https://www.twilio.com/docs/sendgrid/api-reference
    public class SendGridEmailProvider : AbstractEmailProviderAPI
    {
        public class SendGridContent
        {
            public EmailAddress From { get; set; }
            public List<EmailAddress> To { get; set; }
            public string Subject { get; set; }
            public string Text { get; set; }
            public string Html { get; set; }
        }

        public SendGridEmailProvider(IConfiguration configuration) : base(configuration) { }

        public override void PrepareSend(EmailDeliveryType emailDeliveryType)
        {
        }

        public void Send()
        {
            base.Send();
        }
    }
}

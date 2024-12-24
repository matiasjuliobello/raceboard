using Microsoft.Extensions.Configuration;
using RaceBoard.Mailing.BaseClasses;
using RaceBoard.Mailing.Enums;

namespace RaceBoard.Mailing.Providers
{
    public class MailSmtpClientEmailProvider : AbstractEmailProviderSMTP
    {
        public class MailSenderContent
        {
            public EmailAddress From { get; set; }
            public List<EmailAddress> To { get; set; }
            public string Subject { get; set; }
            public string Text { get; set; }
            public string Html { get; set; }
        }

        public MailSmtpClientEmailProvider(IConfiguration configuration) 
            : base(configuration) 
        {
        }

        public override void PrepareSend(EmailDeliveryType emailDeliveryType)
        {
            //object content = null;

            //switch (emailDeliveryType)
            //{
            //    case EmailDeliveryType.Single:
            //        content = BuildContentForSingleDelivery();
            //        break;

            //    case EmailDeliveryType.Bulk:
            //        content = BuildContentForBulkDelivery();
            //        break;
            //    case EmailDeliveryType.Unspecified:
            //        break;
            //}

            //_content = content;
            //_deliveryType = emailDeliveryType;
        }

        public void Send()
        {
            base.Send();
        }

        #region Private Methods

        private MailSenderContent BuildContentForSingleDelivery()
        {
            throw new NotImplementedException();
        }

        private List<MailSenderContent> BuildContentForBulkDelivery()
        {
            throw new NotImplementedException();   
        }


        #endregion
    }
}

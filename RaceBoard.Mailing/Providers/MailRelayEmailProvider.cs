//using RaceBoard.Mailing.BaseClasses;
//using RaceBoard.Mailing.Enums;
//using RaceBoard.Mailing.Interfaces;
//using Microsoft.Extensions.Configuration;

//namespace RaceBoard.Mailing.Providers
//{
//    // https://apidocs.mailrelay.com/
//    public class MailRelayEmailProvider : AbstractEmailProviderAPI
//    {
//        //{
//        //  "attachments": [
//        //    {
//        //      "content": "",
//        //      "file_name": "example.jpg",
//        //      "content_type": "image/jpg",
//        //      "content_id": ""
//        //    }
//        //  ]
//        //}
//        public class MailSenderContent
//        {
//            public EmailAddress From { get; set; }
//            public List<EmailAddress> To { get; set; }
//            public string Subject { get; set; }
//            public string Text { get; set; }
//            public string Html_Part { get; set; }
//        }

//        public MailRelayEmailProvider(IConfiguration configuration) : base(configuration) { }

//        public override void PrepareSend(EmailDeliveryType emailDeliveryType)
//        {
//            object content = null;

//            switch (emailDeliveryType)
//            {
//                case EmailDeliveryType.Single:
//                    content = BuildContentForSingleDelivery();
//                    break;

//                case EmailDeliveryType.Bulk:
//                    content = BuildContentForBulkDelivery();
//                    break;
//                case EmailDeliveryType.Unspecified:
//                    break;
//            }

//            _content = content;
//            _deliveryType = emailDeliveryType;
//        }

//        public void Send()
//        {
//            base.Send();
//        }

//        #region Private Methods

//        private MailSenderContent BuildContentForSingleDelivery()
//        {
//            IEmailAddress sender = _sender;
//            IEnumerable<IEmailAddress> recipients = _recipients;
//            List<string> subjects = _subjects;
//            List<string> bodies = _bodies;

//            var content = new MailSenderContent();

//            content.From = new EmailAddress(sender.Email, sender.Name);

//            content.To = new List<EmailAddress>();
//            foreach (var recipient in recipients)
//            {
//                content.To.Add(new EmailAddress(recipient.Email, recipient.Name));
//            }

//            content.Subject = subjects[0];
//            content.Html_Part = bodies[0];
//            //content.Text = string.Empty;

//            return content;
//        }

//        private List<MailSenderContent> BuildContentForBulkDelivery()
//        {
//            IEmailAddress sender = _sender;
//            List<IEmailAddress> recipients = _recipients.ToList();
//            List<string> subjects = _subjects;
//            List<string> bodies = _bodies;

//            if (recipients.Count != bodies.Count)
//                throw new Exception("Recipients count do not match Body count match");

//            var mails = new List<MailSenderContent>();

//            for (int i = 0; i < bodies.Count; i++)
//            {
//                var content = new MailSenderContent()
//                {
//                    From = new EmailAddress(sender.Email, sender.Name),
//                    To = new List<EmailAddress>()
//                    {
//                        new EmailAddress(recipients[i].Email, recipients[i].Name)
//                    },
//                    Subject = subjects[i],
//                    Html_Part = bodies[i]
//                };
//                mails.Add(content);
//            }

//            return mails;
//        }


//        #endregion
//    }
//}

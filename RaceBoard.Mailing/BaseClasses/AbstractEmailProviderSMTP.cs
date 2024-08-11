using RaceBoard.Mailing.Enums;
using RaceBoard.Mailing.Interfaces;

namespace RaceBoard.Mailing.BaseClasses
{
    public abstract class AbstractEmailProviderSMTP : IEmailProvider
    {
        protected string _host = string.Empty;
        protected int _port = 0;
        protected string _username = string.Empty;
        protected string _password = string.Empty;

        protected string _subject = string.Empty;
        protected string _body = string.Empty;

        protected IEmailAddress _sender;
        protected List<IEmailAddress> _recipients = new List<IEmailAddress>();
        protected List<IEmailAttachment> _attachments = new List<IEmailAttachment>();

        public AbstractEmailProviderSMTP(string host, int port, string username, string password)
        {
            _host = host;
            _port = port;
            _username = username;
            _password = password;
        }

        public void AddSender(IEmailAddress mailAddress)
        {
            _sender = mailAddress;
        }

        public virtual void AddRecipient(IEmailAddress mailAddress)
        {
            _recipients.Add(mailAddress);
        }
        public void AddRecipients(IEnumerable<IEmailAddress> mailAddresses)
        {
            _recipients.AddRange(mailAddresses);
        }

        public void AddAttachments(IEnumerable<IEmailAttachment> attachments)
        {
            _attachments.AddRange(attachments);
        }

        public virtual void AddSubject(string value)
        {
            _subject = value;
        }

        public virtual void AddBody(string value)
        {
            _body = value;
        }

        public abstract void PrepareSend(EmailDeliveryType emailDeliveryType);

        public virtual void Send()
        {
            throw new NotImplementedException("Method not available in base class <EmailSenderSMTP>. Must implement in derived class.");
        }
    }
}
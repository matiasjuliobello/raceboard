using RaceBoard.Mailing.Enums;
using RaceBoard.Mailing.Interfaces;
using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Configuration;
using RaceBoard.Notification.Interfaces;
using RaceBoard.Mailing.Entities;

namespace RaceBoard.Mailing.BaseClasses
{
    public abstract class AbstractEmailProviderSMTP : IEmailProvider
    {
        protected string _senderEmailAddress = string.Empty;
        protected string _senderFriendlyName = string.Empty;

        protected string _host = string.Empty;
        protected int _port = 0;
        protected bool _useSSL = false;
        protected string _username = string.Empty;
        protected string _password = string.Empty;

        protected string _subject = string.Empty;
        protected string _body = string.Empty;

        protected IEmailAddress _sender;
        protected List<IEmailAddress> _recipients = new List<IEmailAddress>();
        protected List<IEmailAttachment> _attachments = new List<IEmailAttachment>();

        public AbstractEmailProviderSMTP(IConfiguration configuration)
        {
            _host = configuration["EmailProvider_SMTP_Host"];
            _port = Convert.ToInt32(configuration["EmailProvider_SMTP_Port"]);
            _useSSL = Convert.ToBoolean(configuration["EmailProvider_SMTP_SSL"]);
            _username = configuration["EmailProvider_SMTP_Username"];
            _password = configuration["EmailProvider_SMTP_Password"];

            _senderEmailAddress = configuration["EmailProvider_SMTP_SenderEmailAddress"];
            _senderFriendlyName = configuration["EmailProvider_SMTP_SenderFriendlyName"];
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

        public async Task SendAsync()
        {
            await Task.Run(() => Send());
        }

        public virtual void Send()
        {
            //throw new NotImplementedException("Method not available in base class <EmailSenderSMTP>. Must implement in derived class.");

            var smtpClient = new SmtpClient(_host)
            {
                Port = _port,
                Credentials = new NetworkCredential(_username, _password),
                EnableSsl = _useSSL,
            };
            smtpClient.Timeout = 5000;

            if (_sender == null)
            {
                _sender = new EmailAddress(_senderEmailAddress, _senderFriendlyName);
            }

            var mailMessage = new MailMessage()
            {
                From = new MailAddress(_sender.Email, _sender.Name),
                Sender = new MailAddress(_sender.Email, _sender.Name),
                Subject = _subject,
                Body = _body,
                IsBodyHtml = true
            };

            foreach (var recipient in _recipients)
            {
                mailMessage.To.Add(new MailAddress(recipient.Email, recipient.Name));
            }

            foreach (var attachment in _attachments)
            {
                Attachment mailAttachment = attachment.Content != null ?
                    new Attachment(new MemoryStream(attachment.Content), attachment.Filename) :
                    new Attachment(attachment.Filename, attachment.Type);

                mailMessage.Attachments.Add(mailAttachment);
            }

            smtpClient.Send(mailMessage);
        }

        public Task Send(INotification notification)
        {
            var settings = notification.Settings as EmailNotificationSettings;
            var data = notification.Data as EmailNotificationData;

            //this.AddSender(settings);
            this.AddRecipient(new EmailAddress(data.EmailAddress, data.FullName));
            //this.AddAttachments(data.Attachments);
            this.AddBody(data.Body);
            this.AddSubject(data.Subject);

            return this.SendAsync();
        }
    }
}
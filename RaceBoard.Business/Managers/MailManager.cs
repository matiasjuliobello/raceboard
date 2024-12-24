using Microsoft.Extensions.Configuration;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Mailing.BaseClasses;
using RaceBoard.Mailing.Interfaces;

namespace RaceBoard.Business.Managers
{
    public class MailManager : IMailManager
    {
        private readonly IEmailProvider _emailProvider;

        private readonly bool _enabled;

        public MailManager
        (
            IConfiguration configuration,
            IEmailProvider emailProvider
        )
        {
            _emailProvider = emailProvider;

            bool.TryParse(configuration["Mailing_Enabled"], out _enabled);
        }

        public async Task SendMail(string subject, string body, string emailAddress, string fullName)
        {
            if (!_enabled)
                return;

            //_emailProvider.AddSender(new EmailAddress(senderEmailAddress, senderFriendlyName)); 
            _emailProvider.AddRecipient(new EmailAddress(emailAddress, fullName));
            _emailProvider.AddSubject(subject);
            _emailProvider.AddBody(body);
            //_emailProvider.PrepareSend(Mailing.Enums.EmailDeliveryType.Single);
            _emailProvider.Send();
        }
    }
}
using RaceBoard.Mailing.Enums;

namespace RaceBoard.Mailing.Interfaces
{
    public interface IEmailProvider
    {
        void AddSender(IEmailAddress mailAddress);
        void AddRecipient(IEmailAddress mailAddress);
        void AddRecipients(IEnumerable<IEmailAddress> mailAddresses);
        void AddAttachments(IEnumerable<IEmailAttachment> attachments);
        void AddSubject(string value);
        void AddBody(string value);
        void PrepareSend(EmailDeliveryType emailDeliveryType);
        void Send();
    }
}
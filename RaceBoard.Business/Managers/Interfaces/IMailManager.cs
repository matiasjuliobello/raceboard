namespace RaceBoard.Business.Managers.Interfaces
{
    public interface IMailManager
    {
        Task SendMail(string subject, string body, string emailAddress, string fullName);
    }
}
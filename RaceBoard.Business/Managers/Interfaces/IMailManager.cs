namespace RaceBoard.Managers.Interfaces
{
    public interface IMailManager
    {
        Task Send(string subject, string body, string emailAddress, string fullName);
    }
}
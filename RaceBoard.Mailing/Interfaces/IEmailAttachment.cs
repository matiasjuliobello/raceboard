namespace RaceBoard.Mailing.Interfaces
{
    public interface IEmailAttachment
    {
        string Filename { get; set; }
        byte[] Content { get; set; }
        string Type { get; set; }
        string Disposition { get; set; }
    }
}

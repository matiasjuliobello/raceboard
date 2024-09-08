namespace RaceBoard.Domain.Upload
{
    public class UploadedFile : AbstractEntity
    {
        public string Filename { get; set; }
        public string UniqueFilename { get; set; }
        public byte[] Content { get; set; }
        public string ContentType { get; set; }
    }
}

namespace RaceBoard.Domain
{
    public class FileUpload
    {
        public int Id { get; set; }
        public string Filename { get; set; }
        public string UniqueFilename { get; set; }
        public byte[] Content { get; set; }
        public string ContentType { get; set; }
    }
}

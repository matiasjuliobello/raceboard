namespace RaceBoard.Domain.Spreadsheet.Abstract
{
    public abstract class Spreadsheet
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public SpreadsheetType Type { get; set; }
        public File File {  get; set; }
        public User CreationUser { get; set; }
        public DateTimeOffset CreationDate { get; set; }
    }
}
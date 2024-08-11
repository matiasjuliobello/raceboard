namespace RaceBoard.DTOs.Script.Response
{
    public class ScriptItemResponse
    {
        public int IdScript { get; set; }
        public int IdActor { get; set; }
        public int IdRecordingType { get; set; }
        public string Character { get; set; }
        public DateTimeOffset RecordingDate { get; set; }
        public int Loops { get; set; }
        public bool IsSubtitled { get; set; }
    }
}
namespace RaceBoard.Domain
{
    public class ScriptItem
    {
        public int Id { get; set; }
        public Script Script { get; set; }
        public User Actor { get; set; }
        public RecordingType? RecordingType { get; set; }
        public string Character { get; set; }
        public DateTimeOffset? RecordingDate { get; set; }
        public int Loops { get; set; }
        public bool IsSubtitled { get; set; }
    }
}


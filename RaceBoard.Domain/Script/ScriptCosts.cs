namespace RaceBoard.Domain
{
    public class ScriptCosts
    {
        public int Id { get; set; }
        public Script Script { get; set; }
        public decimal MinLoopsAmount { get; set; }
        public decimal MinLoopsValue { get; set; }
        public decimal LoopUnitValue { get; set; }
        public decimal SongUnitValue { get; set; }
        public decimal ChorusUnitValue { get; set; }
        public decimal WordUnitValue { get; set; }
        public decimal DirectionFactor { get; set; }
        public decimal TranslationFactor { get; set; }
        public decimal AdaptationFactor { get; set; }
        public decimal MixFactor { get; set; }
        public decimal TimingFactor { get; set; }
    }
}
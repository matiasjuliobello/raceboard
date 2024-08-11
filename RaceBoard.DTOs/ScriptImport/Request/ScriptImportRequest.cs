using System.ComponentModel.DataAnnotations;

namespace RaceBoard.DTOs.ScriptImport.Request
{
    public class ScriptImportRequest
    {
        [Required]
        public int IdProject { get; set; }
        public int? IdScript { get; set; }
        public int? IdTranslator { get; set; }
        public bool SkipFirstRow { get; set; }
    }
}

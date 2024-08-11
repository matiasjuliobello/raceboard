namespace RaceBoard.Domain
{
    public class Script
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Episode { get; set; }
        public string Comments { get; set; }
        public Project Project { get; set; }
        public ScriptStatus Status { get; set; }
        public User CreationUser { get; set; }
        public DateTimeOffset CreationDate { get; set; }
        public int RunningTime { get; set; }
        public int Pages { get; set; }
        public int Loops { get; set; }
        public int Characters { get; set; }
        public Language OriginalLanguage { get; set; }
        public Language DubbingLanguage { get; set; }
        public bool IsActive { get; set; }

        #region Calculated Fields

        public bool HasImport
        { 
            get;
            private set; 
        }
        
        public bool HasPendingApproval
        { 
            get; 
            private set; 
        }

        #endregion

        #region Public Methods

        public string GetDescription()
        {
            return $"Ep. {Episode} - {Title}";
        }
        
        #endregion
    }
}

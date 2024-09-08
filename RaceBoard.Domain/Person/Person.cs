namespace RaceBoard.Domain
{
    public class Person : AbstractEntity
    {
        public User User { get; set; }
        public Country Country { get; set; }
        public BloodType BloodType { get; set; }
        public MedicalInsurance MedicalInsurance { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTimeOffset BirthDate { get; set; }
        public string EmailAddress { get; set; }
        public string ContactPhone { get; set; }

        #region Calculated Properties

        public string Fullname
        {
            get
            {
                return $"{Firstname} {Lastname}";
            }
        }

        #endregion
    }
}

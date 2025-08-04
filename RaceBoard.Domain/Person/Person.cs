namespace RaceBoard.Domain
{
    public class Person
    {
        public int Id { get; set; }
        public User User { get; set; }
        public Gender? Gender { get; set; }
        public Country? Country { get; set; }
        public BloodType? BloodType { get; set; }
        public MedicalInsurance? MedicalInsurance { get; set; }
        public string MedicalInsuranceNumber { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTimeOffset? BirthDate { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string EmergencyContactPhoneNumber { get; set; }
        public string PersonalIdentificationCardNumber { get; set; }

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

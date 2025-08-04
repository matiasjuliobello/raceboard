using RaceBoard.DTOs.BloodType.Response;
using RaceBoard.DTOs.Country.Response;
using RaceBoard.DTOs.Gender.Response;
using RaceBoard.DTOs.MedicalInsurance.Response;
using RaceBoard.DTOs.User.Response;

namespace RaceBoard.DTOs.Person.Response
{
    public class PersonResponse
    {
        public int Id { get; set; }
        public GenderResponse Gender { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTimeOffset BirthDate { get; set; }
        public string EmailAddress { get; set; }
        public string Address { get; set; }
        public string PhoneNumber{ get; set; }
        public string EmergencyContactPhoneNumber { get; set; }
        public UserSimpleResponse User { get; set; }
        public CountryResponse Country { get; set; }
        public BloodTypeResponse BloodType { get; set; }
        public MedicalInsuranceResponse MedicalInsurance { get; set; }
        public string MedicalInsuranceNumber { get; set; }
        public string? PersonalIdentificationCardNumber { get; set; }


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
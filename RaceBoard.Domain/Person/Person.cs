namespace RaceBoard.Domain
{
    public class Person
    {
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTimeOffset BirthDate { get; set; }
        public string EmailAddress { get; set; }
        public string ContactPhone { get; set; }
        public BloodType BloodType { get; set; }
        public MedicalInsurance MedicalInsurance { get; set; }
    }
}

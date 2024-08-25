namespace RaceBoard.DTOs.Person.Request
{
    public class PersonRequest
    {
        public int Id { get; set; }
        public int IdUser { get; set; }
        public int IdCountry { get; set; }
        public int IdBloodType { get; set; }
        public int IdMedicalInsurance { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTimeOffset BirthDate { get; set; }
        public string EmailAddress { get; set; }
        public string ContactPhone { get; set; }
    }
}
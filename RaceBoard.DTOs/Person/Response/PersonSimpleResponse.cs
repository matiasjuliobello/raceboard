namespace RaceBoard.DTOs.Person.Response
{
    public class PersonSimpleResponse
    {
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }

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
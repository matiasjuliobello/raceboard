using RaceBoard.Mailing.Interfaces;

namespace RaceBoard.Mailing.BaseClasses
{
    public class EmailAddress : IEmailAddress
    {
        private string _address = string.Empty;
        private string _name = string.Empty;

        public string Email
        {
            get { return _address; }
            set { _address = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public EmailAddress(string address, string name)
        {
            Email = address;
            Name = name;
        }
    }
}

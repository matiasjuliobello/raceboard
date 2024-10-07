using RaceBoard.DTOs.Authentication.Response;
using RaceBoard.DTOs.Person.Response;

namespace RaceBoard.DTOs.User.Response
{
    public class UserLoginResponse
    {
        public UserSimpleResponse User { get; set; }
        public PersonSimpleResponse Person { get; set; }
        public AccessTokenResponse AccessToken { get; set; }
    }
}

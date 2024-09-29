using RaceBoard.DTOs.Authentication.Response;

namespace RaceBoard.DTOs.User.Response
{
    public class UserLoginResponse
    {
        public UserSimpleResponse User { get; set; }
        public AccessTokenResponse AccessToken { get; set; }
    }
}

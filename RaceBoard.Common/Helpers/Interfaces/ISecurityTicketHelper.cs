using Microsoft.IdentityModel.Tokens;

namespace RaceBoard.Common.Helpers.Interfaces
{
    public interface ISecurityTicketHelper
    {
        AccessToken CreateToken(string username);
        SecurityToken GetSecurityToken(string token);
    }
}

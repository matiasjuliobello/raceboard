using Microsoft.IdentityModel.Tokens;

namespace RaceBoard.Common.Helpers.Interfaces
{
    public interface ISecurityTicketHelper
    {
        AccessToken GenerateToken(string id, string email);
        SecurityToken GetSecurityToken(string token);
    }
}

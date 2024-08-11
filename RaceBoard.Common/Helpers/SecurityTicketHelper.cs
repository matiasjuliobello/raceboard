using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using RaceBoard.Common.Exceptions;
using RaceBoard.Common.Helpers.Interfaces;
using Microsoft.Extensions.Configuration;

namespace RaceBoard.Common.Helpers
{
    public class SecurityTicketHelper : ISecurityTicketHelper
    {
        #region Private Members

        private readonly int _tokenLifetime;
        private readonly string _secretKey;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly string _securityAlgorithm;

        #endregion

        public SecurityTicketHelper(IConfiguration configuration)
        {
            _tokenLifetime = Convert.ToInt32(configuration["SecurityTicket_TokenLifetime"]);
            _secretKey = configuration["SecurityTicket_SecretKey"];
            _issuer = configuration["SecurityTicket_Issuer"];
            _audience = configuration["SecurityTicket_Audience"];

            _securityAlgorithm = SecurityAlgorithms.HmacSha512Signature;
        }

        #region ISecurityTicketHelper implementation

        public AccessToken CreateToken(string username)
        {
            var signingCredentials = GetSigningCredentials();

            var claims = BuildClaims(username, username);

            var expires = GetTokenExpirationTimestamp(_tokenLifetime);

            var securityToken = new JwtSecurityToken
                (
                    issuer: _issuer,
                    audience: _audience,
                    claims: claims.Claims,
                    expires: expires,
                    signingCredentials: signingCredentials
                );

            var tokenHandler = new JwtSecurityTokenHandler();

            string token = tokenHandler.WriteToken(securityToken);

            var accessToken = new AccessToken() 
            {
                Token = token,
                ExpiresAt = expires 
            };

            return accessToken;
        }

        public SecurityToken GetSecurityToken(string token)
        {
            ValidateToken(token);

            var tokenHandler = new JwtSecurityTokenHandler();

            SecurityToken securityToken = tokenHandler.ReadJwtToken(token);

            return securityToken;
        }

        #endregion

        #region Private Methods

        private SigningCredentials GetSigningCredentials()
        {
            SymmetricSecurityKey symmetricSecurityKey = BuildSymmetricSecurityKey(_secretKey);

            return new SigningCredentials(symmetricSecurityKey, _securityAlgorithm);
        }

        private void ValidateToken(string token)
        {
            TokenValidationParameters validationParameters = GetValidationParameters();

            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                SecurityToken validatedToken;
                IPrincipal principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
            }
            catch (SecurityTokenSignatureKeyNotFoundException e)
            {
                throw new FunctionalException(Enums.ErrorType.Unauthorized, "Token signature is not valid.");
            }
            catch (SecurityTokenInvalidLifetimeException e)
            {
                throw new FunctionalException(Enums.ErrorType.Unauthorized, "Token lifetime is not valid.");
            }
            catch(Exception e)
            {
                throw new FunctionalException(Enums.ErrorType.Unauthorized, "Token validation failed.");
            }
        }

        private TokenValidationParameters GetValidationParameters()
        {
            SymmetricSecurityKey symmetricSecurityKey = BuildSymmetricSecurityKey(_secretKey);

            return new TokenValidationParameters()
            {
                RequireSignedTokens = true,
                IssuerSigningKey = symmetricSecurityKey,

                ValidateLifetime = true,
                LifetimeValidator = LifetimeValidator,

                ValidateAudience = true,
                ValidateIssuer = true,
                ValidIssuer = _issuer,
                ValidAudience = _audience
            };
        }

        private bool LifetimeValidator(DateTime? notBefore, DateTime? notAfter, SecurityToken securityToken, TokenValidationParameters validationParameters)
        {
            bool isStartValid = true;
            bool isEndvalid = true;

            var currentTimestamp = DateTime.UtcNow;

            if (notBefore != null)
                isStartValid = currentTimestamp >= notBefore;

            if (notAfter != null)
                isEndvalid = currentTimestamp < notAfter;

            return isStartValid && isEndvalid;
        }

        private SymmetricSecurityKey BuildSymmetricSecurityKey(string secretKey)
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        }

        private DateTime GetTokenExpirationTimestamp(int tokenLifeTimeInMinutes)
        {
            var currentTimestamp = DateTime.UtcNow;

            return currentTimestamp.AddMinutes(tokenLifeTimeInMinutes);
        }

        private ClaimsIdentity BuildClaims(string JWT_ID, string username)
        {
            return new ClaimsIdentity
                (
                    new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Jti, JWT_ID),
                        new Claim(JwtRegisteredClaimNames.Sub, username),
                        new Claim(JwtRegisteredClaimNames.Email, username)
                    }
                );
        }

        #endregion
    }
}
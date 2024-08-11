using RaceBoard.Common.Helpers.Interfaces;
using System.Security.Cryptography;

namespace RaceBoard.Common.Helpers
{
    public class StringHelper : IStringHelper
    {
        public string GenerateRandomString(int length)
        {
            var randomNumber = new byte[length];

            using (var randomNumberGenerator = RandomNumberGenerator.Create())
            {
                randomNumberGenerator.GetBytes(randomNumber);

                return Convert.ToBase64String(randomNumber);
            }
        }
    }
}

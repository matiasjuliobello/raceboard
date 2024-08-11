using RaceBoard.Common.Helpers.Interfaces;
//using System.Security.Cryptography;

namespace RaceBoard.Common.Helpers
{
    public class CryptographyHelper : ICryptographyHelper
    {
        private int _workFactor;
        private bool _enhancedEntropy;

        public CryptographyHelper()
        {
            _workFactor = 13;
            _enhancedEntropy = true;
        }

        #region Public ICryptographyHelper implementation

        public string ComputeHash(string text)
        {
            string hash = BCrypt.Net.BCrypt.EnhancedHashPassword(text);//, _workFactor, BCrypt.Net.HashType.SHA512);

            return hash;
        }

        public bool VerifyHash(string hash, string text)
        {
            bool verified = BCrypt.Net.BCrypt.EnhancedVerify(text, hash); //, BCrypt.Net.HashType.SHA512);

            return verified;
        }

        #endregion

        #region Private Methods

        #endregion
    }
}

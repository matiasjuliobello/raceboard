using Microsoft.Extensions.Configuration;
using RaceBoard.Common.Helpers.Interfaces;

namespace RaceBoard.Common.Helpers
{
    public class ConfigurationHelper : IConfigurationHelper
    {
        private readonly IConfiguration _configuration;

        public ConfigurationHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetValue(string key, bool throwException = true)
        {
            string value = _configuration[key];

            if (string.IsNullOrEmpty(value))
            {
                if (throwException)
                    throw new ArgumentNullException($"Parameter <{key}> is missing.");
            }

            return value;
        }

        public T GetValue<T>(string key, bool throwException = true)
        {
            string value = _configuration[key];

            if (string.IsNullOrEmpty(value))
            {
                if (throwException)
                    throw new ArgumentNullException($"Parameter <{key}> is missing.");
            }

            return (T)Convert.ChangeType(value, typeof(T));
        }
    }
}

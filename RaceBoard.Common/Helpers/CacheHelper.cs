using RaceBoard.Common.Helpers.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace RaceBoard.Common.Helpers
{
    public class CacheHelper : ICacheHelper
    {
        private const int _DEFAULT_LIFETIME = 60;

        private readonly IMemoryCache _memoryCache;
        private readonly TimeSpan _timeSpan;

        public CacheHelper(IMemoryCache memoryCache, IConfiguration configuration)
        {
            string lifetime = configuration["Cache_Lifetime"];

            _timeSpan = TimeSpan.FromMinutes(!string.IsNullOrEmpty(lifetime) ? Convert.ToInt32(lifetime): _DEFAULT_LIFETIME);

            _memoryCache = memoryCache;
        }

        public T Get<T>(string key)
        {
            return _memoryCache.Get<T>(key);
        }

        public void Set<T>(string key, T item)
        {
            _memoryCache.Set(key, item, _timeSpan);
        }
    }
}

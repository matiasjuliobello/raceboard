using System.Collections.Concurrent;

namespace RaceBoard.Common.Extensions
{
    public static class DictionaryExtensions
    {
        public static void AddOrUpdate<K, V>(this ConcurrentDictionary<K, V> dictionary, K key, V value)
        {
            dictionary.AddOrUpdate(key, value, (oldkey, oldvalue) => value);
        }
    }
}

using RaceBoard.Common.Helpers.Interfaces;
using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;

namespace RaceBoard.Common.Helpers
{
    [ExcludeFromCodeCoverage]
    public class SerializationHelper : ISerializationHelper
    {
        public string Serialize<T>(T value)
        {
            return JsonConvert.SerializeObject(value, Formatting.Indented);
        }

        public T DeSerialize<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }
    }
}

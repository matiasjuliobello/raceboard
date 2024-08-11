namespace RaceBoard.Common.Helpers.Interfaces
{
    public interface ISerializationHelper
    {
        public string Serialize<T>(T value);

        public T DeSerialize<T>(string value);
    }
}

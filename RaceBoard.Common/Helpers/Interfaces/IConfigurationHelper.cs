namespace RaceBoard.Common.Helpers.Interfaces
{
    public interface IConfigurationHelper
    {
        string GetValue(string key, bool throwException = true);
        T GetValue<T>(string key, bool throwException = true);
    }
}

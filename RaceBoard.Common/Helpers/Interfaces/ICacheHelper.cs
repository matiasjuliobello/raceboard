namespace RaceBoard.Common.Helpers.Interfaces
{
    public interface ICacheHelper
    {
        T Get<T>(string key);
        void Set<T>(string key, T item);
    }
}

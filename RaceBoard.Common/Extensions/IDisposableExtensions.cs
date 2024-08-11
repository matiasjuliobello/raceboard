namespace RaceBoard.Common.Extensions
{
    public static class IDisposableExtensions
    {
        public static void SafeDispose<T>(this T obj) where T : IDisposable
        {
            if (obj != null)
            {
                obj.Dispose();
            }
        }
    }
}

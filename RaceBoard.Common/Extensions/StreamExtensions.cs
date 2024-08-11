
namespace RaceBoard.Common.Extensions
{
    public static class StreamExtensions
    {
        public static void SafeClose(this Stream stream)
        {
            if (stream == null) return;
            if (stream.CanRead)
                stream.Close();
        }
    }
}

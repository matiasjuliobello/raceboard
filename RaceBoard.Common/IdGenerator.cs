namespace RaceBoard.Common
{
    public class IdGenerator
    {
        public static string BuildUniqueId()
        {
            return Guid.NewGuid().ToString();
        }
    }
}

namespace RaceBoard.Common.Extensions
{
    public static class StringExtensions
    {

        public static string RemoveFirstInstanceOfString(this string value, string removeString)
        {
            int index = value.IndexOf(removeString, StringComparison.InvariantCultureIgnoreCase);

            return index < 0 ? value : value.Remove(index, removeString.Length);
        }

    }
}

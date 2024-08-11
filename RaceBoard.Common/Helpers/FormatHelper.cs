using RaceBoard.Common.Helpers.Interfaces;
using System.Globalization;

namespace RaceBoard.Common.Helpers
{
    // Reference: https://learn.microsoft.com/en-us/dotnet/standard/base-types/standard-numeric-format-strings

    public class FormatHelper : IFormatHelper
    {
        #region IFormatHelper implementation

        public string FormatTime(DateTimeOffset value, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public string FormatDate(DateTimeOffset value, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public string FormatDateTime(DateTimeOffset value, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public string FormatNumber(decimal value, int decimalPlaces, CultureInfo culture)
        {
            return value.ToString($"N{decimalPlaces}", culture);
        }

        public string FormatCurrency(decimal value, int decimalPlaces, CultureInfo culture)
        {
            return value.ToString($"C{decimalPlaces}", culture);
        }

        #endregion
    }
}
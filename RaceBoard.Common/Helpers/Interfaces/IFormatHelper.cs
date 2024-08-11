using System.Globalization;

namespace RaceBoard.Common.Helpers.Interfaces
{
    public interface IFormatHelper
    {
        string FormatDate(DateTimeOffset value, CultureInfo culture);
        string FormatTime(DateTimeOffset value, CultureInfo culture);
        string FormatDateTime(DateTimeOffset value, CultureInfo culture);
        string FormatNumber(decimal value, int decimalPlaces, CultureInfo culture);
        string FormatCurrency(decimal value, int decimalPlaces, CultureInfo culture);
    }
}

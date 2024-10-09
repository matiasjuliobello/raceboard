using RaceBoard.Data;
using RaceBoard.Domain;

namespace RaceBoard.Business.Managers.Interfaces
{
    public interface IFormatManager
    {
        //List<NumericFormat> GetTimeFormat(ITransactionalContext? context = null);
        List<DateFormat> GetDateFormats(ITransactionalContext? context = null);
        //List<TimeFormat> GetNumericFormat(ITransactionalContext? context = null);
    }
}
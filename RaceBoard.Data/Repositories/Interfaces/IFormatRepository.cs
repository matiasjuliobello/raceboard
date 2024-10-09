using RaceBoard.Domain;

namespace RaceBoard.Data.Repositories.Interfaces
{
    public interface IFormatRepository
    {
        List<DateFormat> GetDateFormats(ITransactionalContext? context = null);
    }
}

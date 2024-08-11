using TimeZone = RaceBoard.Domain.TimeZone;

namespace RaceBoard.Data.Repositories.Interfaces
{
    public interface ITimeZoneRepository
    {
        List<TimeZone> Get(ITransactionalContext? context = null);
    }
}

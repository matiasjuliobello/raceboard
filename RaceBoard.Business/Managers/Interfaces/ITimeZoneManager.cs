using RaceBoard.Data;
using TimeZone = RaceBoard.Domain.TimeZone;

namespace RaceBoard.Business.Managers.Interfaces
{
    public interface ITimeZoneManager
    {
        List<TimeZone> Get(ITransactionalContext? context = null);
    }
}
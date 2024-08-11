using RaceBoard.Common;
using RaceBoard.Domain;

namespace RaceBoard.Data.Helpers.Interfaces
{
    public interface IContextResolver
    {
        string GetDatabaseConnection();
        User GetCurrentUser(RequestContext context);
    }
}

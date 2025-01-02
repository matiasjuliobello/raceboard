using RaceBoard.Common;
using RaceBoard.Domain;

namespace RaceBoard.Business.Managers.Interfaces
{
    public interface IRequestContextManager
    {
        RequestContext GetContext();
        User GetUser();
    }
}

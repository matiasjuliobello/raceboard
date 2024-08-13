using RaceBoard.Common;
using RaceBoard.Domain;

namespace RaceBoard.Service.Helpers.Interfaces
{
    public interface IRequestContextHelper
    {
        RequestContext GetContext();
        User GetUser();
    }
}

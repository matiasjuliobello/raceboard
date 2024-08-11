using RaceBoard.Common.Helpers.Interfaces;
using RaceBoard.Common;
using RaceBoard.Domain;

namespace RaceBoard.Data.Repositories.Base.Interfaces
{
    public interface IRepository
    {
        string ConnectionString { get; }
        RequestContext Context { get; set; }
        User CurrentUser { get; set; }
        IHttpHeaderHelper HttpHeaderHelper { get; set; }
    }
}

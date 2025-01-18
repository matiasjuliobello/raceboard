using RaceBoard.Data;

namespace RaceBoard.Business.Managers.Interfaces
{
    public interface IFileManager
    {
        Domain.File Get(int id, ITransactionalContext? context = null);
        Domain.File Get(string url, ITransactionalContext? context = null);
    }
}

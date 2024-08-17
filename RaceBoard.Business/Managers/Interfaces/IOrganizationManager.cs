using RaceBoard.Data;
using RaceBoard.Domain;

namespace RaceBoard.Business.Managers.Interfaces
{
    public interface IOrganizationManager
    {
        void Create(Organization organization, ITransactionalContext? context = null);
        void Update(Organization organization, ITransactionalContext? context = null);
        void Delete(int id, ITransactionalContext? context = null);
    }
}

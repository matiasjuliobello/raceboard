using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Domain;

namespace RaceBoard.Data.Repositories.Interfaces
{
    public interface IPersonRepository
    {
        ITransactionalContext GetTransactionalContext(TransactionContextScope scope = TransactionContextScope.Internal);
        void ConfirmTransactionalContext(ITransactionalContext context);
        void CancelTransactionalContext(ITransactionalContext context);

        bool Exists(int id, ITransactionalContext? context = null);
        bool ExistsDuplicate(Person person, ITransactionalContext? context = null);

        PaginatedResult<Person> Get(PersonSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null);
        Person? Get(int id, ITransactionalContext? context = null);
        Person GetByIdUser(int idUser, ITransactionalContext? context = null);
        void Create(Person person, ITransactionalContext? context = null);
        void Update(Person person, ITransactionalContext? context = null);
        int Delete(int id, ITransactionalContext? context = null);
        void SetUserAssociation(Person person, ITransactionalContext? context = null);
    }
}

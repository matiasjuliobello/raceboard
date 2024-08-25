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

        PaginatedResult<Person> Get(PersonSearchFilter searchFilter, PaginationFilter paginationFilter, Sorting sorting, ITransactionalContext? context = null);
        void Create(Person person, ITransactionalContext? context = null);
        void Update(Person person, ITransactionalContext? context = null);
        int Delete(int id, ITransactionalContext? context = null);
        void SetUserAssociation(Person person, ITransactionalContext? context = null);
    }
}

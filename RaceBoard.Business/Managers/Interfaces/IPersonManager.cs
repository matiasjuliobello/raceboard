using Microsoft.AspNetCore.Mvc;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data;
using RaceBoard.Domain;

namespace RaceBoard.Business.Managers.Interfaces
{
    public interface IPersonManager
    {
        PaginatedResult<Person> Get([FromQuery] PersonSearchFilter? searchFilter = null, [FromQuery] PaginationFilter? paginationFilter = null, [FromQuery] Sorting? sorting = null, ITransactionalContext? context = null);
        Person Get(int id, ITransactionalContext? context = null);
        void Create(Person person, ITransactionalContext? context = null);
        void Update(Person person, ITransactionalContext? context = null);
        void Delete(int id, ITransactionalContext? context = null);
    }
}

﻿using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Domain;

namespace RaceBoard.Data.Repositories.Interfaces
{
    public interface IRaceRepository
    {
        ITransactionalContext GetTransactionalContext(TransactionContextScope scope = TransactionContextScope.Internal);
        void ConfirmTransactionalContext(ITransactionalContext context);
        void CancelTransactionalContext(ITransactionalContext context);

        PaginatedResult<Race> Get(RaceSearchFilter searchFilter, PaginationFilter paginationFilter, Sorting sorting, ITransactionalContext? context = null);
        void Create(Race race, ITransactionalContext? context = null);
        void Update(Race race, ITransactionalContext? context = null);
        int Delete(int id, ITransactionalContext? context = null);
    }
}

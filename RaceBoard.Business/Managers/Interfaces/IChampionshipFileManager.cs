﻿using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data;
using RaceBoard.Domain;

namespace RaceBoard.Business.Managers.Interfaces
{
    public interface IChampionshipFileManager
    {
        PaginatedResult<ChampionshipFile> Get(ChampionshipFileSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null);
        void Create(ChampionshipFile championshipFileUpload, ITransactionalContext? context = null);
        void Delete(int id, ITransactionalContext? context = null);
    }
}
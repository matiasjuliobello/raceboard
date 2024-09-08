﻿using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data.Helpers.Interfaces;
using RaceBoard.Data.Repositories.Base.Abstract;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;

namespace RaceBoard.Data.Repositories
{
    public class RaceCategoryRepository : AbstractRepository, IRaceCategoryRepository
    {
        #region Private Members

        private readonly Dictionary<string, string> _columnsMapping = new()
        {
            { "Id", "[RaceCategory].Id" },
            { "Name", "[RaceCategory].Name"}
        };

        #endregion

        #region Constructors

        public RaceCategoryRepository(IContextResolver contextResolver, IQueryBuilder queryBuilder) : base(contextResolver, queryBuilder)
        {
        }

        #endregion

        #region IRaceCategoryRepository implementation

        public PaginatedResult<RaceCategory> Get(RaceCategorySearchFilter searchFilter, PaginationFilter paginationFilter, Sorting sorting, ITransactionalContext? context = null)
        {
            return this.GetRaceCategories(searchFilter: searchFilter, paginationFilter: paginationFilter, sorting: sorting, context: context);
        }

        #endregion

        #region Private Methods

        private PaginatedResult<RaceCategory> GetRaceCategories(RaceCategorySearchFilter searchFilter, PaginationFilter paginationFilter, Sorting sorting, ITransactionalContext? context = null)
        {
            string sql = $@"SELECT
                                [RaceCategory].Id [Id],
                                [RaceCategory].Name [Name]
                            FROM [RaceCategory] [RaceCategory]";

            QueryBuilder.AddCommand(sql);
            ProcessSearchFilter(searchFilter);

            QueryBuilder.AddSorting(sorting, _columnsMapping);
            QueryBuilder.AddPagination(paginationFilter);

            return base.GetMultipleResultsWithPagination<RaceCategory>(context);
        }

        private void ProcessSearchFilter(RaceCategorySearchFilter searchFilter)
        {
            if (searchFilter.Ids != null && searchFilter.Ids.Length > 0)
            {
                QueryBuilder.AddCondition($"[RaceCategory].Id IN @ids");
                QueryBuilder.AddParameter("ids", searchFilter.Ids);
            }

            if (!string.IsNullOrEmpty(searchFilter.Name))
            {
                QueryBuilder.AddCondition($"[RaceCategory].Name LIKE {AddLikeWildcards("@name")}");
                QueryBuilder.AddParameter("name", searchFilter.Name);
            }
        }

        #endregion
    }
}
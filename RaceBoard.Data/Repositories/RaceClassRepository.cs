using Dapper;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data.Helpers.Interfaces;
using RaceBoard.Data.Repositories.Base.Abstract;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;

namespace RaceBoard.Data.Repositories
{
    public class RaceClassRepository : AbstractRepository, IRaceClassRepository
    {
        #region Private Members

        private readonly Dictionary<string, string> _columnsMapping = new()
        {
            { "Id", "[RaceClass].Id" },
            { "Name", "[RaceClass].Name"},
            { "[RaceCategory].Id", "[RaceCategory].Id" },
            { "[RaceCategory].Name", "[RaceCategory].Name"}
        };

        #endregion

        #region Constructors

        public RaceClassRepository(IContextResolver contextResolver, IQueryBuilder queryBuilder) : base(contextResolver, queryBuilder)
        {
        }

        #endregion

        #region IRaceClassRepository implementation

        public PaginatedResult<RaceClass> Get(RaceClassSearchFilter searchFilter, PaginationFilter paginationFilter, Sorting sorting, ITransactionalContext? context = null)
        {
            return this.GetRaceCategories(searchFilter: searchFilter, paginationFilter: paginationFilter, sorting: sorting, context: context);
        }

        #endregion

        #region Private Methods

        private PaginatedResult<RaceClass> GetRaceCategories(RaceClassSearchFilter searchFilter, PaginationFilter paginationFilter, Sorting sorting, ITransactionalContext? context = null)
        {
            string sql = $@"SELECT
                                [RaceClass].Id [Id],
                                [RaceClass].Name [Name],
                                [RaceCategory].Id [Id],
                                [RaceCategory].Name [Name]
                            FROM [RaceClass] [RaceClass]
                            INNER JOIN [RaceCategory] [RaceCategory] ON [RaceCategory].Id = [RaceClass].IdRaceCategory";

            QueryBuilder.AddCommand(sql);
            ProcessSearchFilter(searchFilter);

            QueryBuilder.AddSorting(sorting, _columnsMapping);
            QueryBuilder.AddPagination(paginationFilter);

            var raceClasses = new List<RaceClass>();

            PaginatedResult<RaceClass> items = base.GetPaginatedResults<RaceClass>
                (
                    (reader) =>
                    {
                        return reader.Read<RaceClass, RaceCategory, RaceClass>
                        (
                            (raceClass, raceCategory) =>
                            {
                                raceClass.RaceCategory = raceCategory;

                                raceClasses.Add(raceClass);

                                return raceClass;
                            },
                            splitOn: "Id, Id"
                        ).AsList();
                    },
                    context
                );

            items.Results = raceClasses;

            return items;
        }

        private void ProcessSearchFilter(RaceClassSearchFilter searchFilter)
        {
            if (searchFilter.Ids != null && searchFilter.Ids.Length > 0)
            {
                QueryBuilder.AddCondition($"[RaceClass].Id IN @ids");
                QueryBuilder.AddParameter("ids", searchFilter.Ids);
            }

            if (searchFilter.RaceCategory != null && searchFilter.RaceCategory.Id > 0)
            {
                QueryBuilder.AddCondition($"[RaceClass].IdRaceCategory = @idRaceCategory");
                QueryBuilder.AddParameter("idRaceCategory", searchFilter.RaceCategory.Id);
            }

            if (!string.IsNullOrEmpty(searchFilter.Name))
            {
                QueryBuilder.AddCondition($"[RaceClass].Name LIKE {AddLikeWildcards("@name")}");
                QueryBuilder.AddParameter("name", searchFilter.Name);
            }
        }

        #endregion
    }
}
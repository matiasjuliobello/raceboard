using Dapper;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data.Helpers.Interfaces;
using RaceBoard.Data.Repositories.Base.Abstract;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;

namespace RaceBoard.Data.Repositories
{
    public class MastRepository : AbstractRepository, IMastRepository
    {
        #region Private Members

        private readonly Dictionary<string, string> _columnsMapping = new()
        {
            { "Id", "[Mast].Id" },
            { "Competition.Id", "[Competition].Id" },
            { "Competition.Name", "[Competition].Name" }
        };

        #endregion

        #region Constructors

        public MastRepository(IContextResolver contextResolver, IQueryBuilder queryBuilder) : base(contextResolver, queryBuilder)
        {
        }

        #endregion

        #region IMastRepository implementation

        public PaginatedResult<Mast> Get(MastSearchFilter searchFilter, PaginationFilter paginationFilter, Sorting sorting, ITransactionalContext? context = null)
        {
            return this.GetMasts(searchFilter: searchFilter, paginationFilter: paginationFilter, sorting: sorting, context: context);
        }

        #endregion

        #region Private Methods

        private PaginatedResult<Mast> GetMasts(MastSearchFilter searchFilter, PaginationFilter paginationFilter, Sorting sorting, ITransactionalContext? context = null)
        {
            string sql = $@"SELECT
                                [Mast].Id [Id],
                                [Competition].Id [Id],
                                [Competition].Name [Name]
                            FROM [Mast] [Mast]
                            INNER JOIN [Competition] [Competition] ON [Competition].Id = [Mast].IdCompetition";

            QueryBuilder.AddCommand(sql);

            ProcessSearchFilter(searchFilter);

            QueryBuilder.AddSorting(sorting, _columnsMapping);
            QueryBuilder.AddPagination(paginationFilter);

            var masts = new List<Mast>();

            PaginatedResult<Mast> items = base.GetPaginatedResults<Mast>
                (
                    (reader) =>
                    {
                        return reader.Read<Mast, Competition, Mast>
                        (
                            (mast, competition) =>
                            {
                                mast.Competition = competition;

                                masts.Add(mast);

                                return mast;
                            },
                            splitOn: "Id, Id"
                        ).AsList();
                    },
                    context
                );

            items.Results = masts;

            return items;
        }

        private void ProcessSearchFilter(MastSearchFilter searchFilter)
        {
            if (searchFilter.Ids != null && searchFilter.Ids.Length > 0)
            {
                QueryBuilder.AddCondition($"[Mast].Id IN @ids");
                QueryBuilder.AddParameter("ids", searchFilter.Ids);
            }

            if (searchFilter.IdCompetition.HasValue && searchFilter.IdCompetition > 0 )
            {
                QueryBuilder.AddCondition($"[Mast].IdCompetition = @idCompetition");
                QueryBuilder.AddParameter("idCompetition", searchFilter.IdCompetition.Value);
            }
        }

        #endregion
    }
}
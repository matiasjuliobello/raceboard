using Dapper;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data.Helpers.Interfaces;
using RaceBoard.Data.Repositories.Base.Abstract;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;

namespace RaceBoard.Data.Repositories
{
    public class CityRepository : AbstractRepository, ICityRepository
    {
        #region Private Members

        private readonly Dictionary<string, string> _columnsMapping = new()
        {
            { "Id", "[City].Id" },
            { "Name", "[City].Name"}
        };

        #endregion

        #region Constructors

        public CityRepository(IContextResolver contextResolver, IQueryBuilder queryBuilder) : base(contextResolver, queryBuilder)
        {
        }

        #endregion

        #region ICityRepository implementation

        public bool Exists(int id, ITransactionalContext? context = null)
        {
            return base.Exists(id, "City", "Id", context);
        }

        public PaginatedResult<City> Get(CitySearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            return this.GetCities(searchFilter: searchFilter, paginationFilter: paginationFilter, sorting: sorting, context: context);
        }

        #endregion

        #region Private Methods

        private PaginatedResult<City> GetCities(CitySearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            string sql = $@"SELECT
                                [City].Id [Id],
                                [City].Name [Name],
                                [Country].Id [Id],
                                [Country].Name [Name]
                            FROM [City] [City]
                            INNER JOIN [Country] [Country] ON [Country].Id = [City].IdCountry";

            QueryBuilder.AddCommand(sql);

            ProcessSearchFilter(searchFilter);

            QueryBuilder.AddSorting(sorting, _columnsMapping);
            QueryBuilder.AddPagination(paginationFilter);

            var cities = new List<City>();

            PaginatedResult<City> items = base.GetPaginatedResults<City>
                (
                    (reader) =>
                    {
                        return reader.Read<City, Country, City>
                        (
                            (city, country) =>
                            {
                                city.Country = country;

                                cities.Add(city);

                                return city;
                            },
                            splitOn: "Id, Id"
                        ).AsList();
                    },
                    context
                );

            items.Results = cities;

            return items;
        }

        private void ProcessSearchFilter(CitySearchFilter? searchFilter = null)
        {
            if (searchFilter == null)
                return;

            base.AddFilterCriteria(ConditionType.In, "City", "Id", "ids", searchFilter.Ids);
            base.AddFilterCriteria(ConditionType.Like, "City", "Name", "name", searchFilter.Name);
            base.AddFilterCriteria(ConditionType.Equal, "Country", "Id", "idCountry", searchFilter.Country?.Id);
        }

        #endregion
    }
}
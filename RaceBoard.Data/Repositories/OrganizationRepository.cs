using Dapper;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data.Helpers.Interfaces;
using RaceBoard.Data.Repositories.Base.Abstract;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;

namespace RaceBoard.Data.Repositories
{
    public class OrganizationRepository : AbstractRepository, IOrganizationRepository
    {
        #region Private Members

        private readonly Dictionary<string, string> _columnsMapping = new()
        {
            { "Id", "[Organization].Id" },
            { "Name", "[Organization].Name" },
            { "City.Id", "[City].Id" },
            { "City.Name", "[City].Name"}
        };

        #endregion

        #region Constructors

        public OrganizationRepository(IContextResolver contextResolver, IQueryBuilder queryBuilder) : base(contextResolver, queryBuilder)
        {
        }

        #endregion

        #region IOrganizationRepository implementation

        public ITransactionalContext GetTransactionalContext(TransactionContextScope scope = TransactionContextScope.Internal)
        {
            return base.GetTransactionalContext(scope);
        }

        public void ConfirmTransactionalContext(ITransactionalContext context)
        {
            base.ConfirmTransactionalContext(context);
        }

        public void CancelTransactionalContext(ITransactionalContext context)
        {
            base.CancelTransactionalContext(context);
        }

        public PaginatedResult<Organization> Get(OrganizationSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            return this.GetOrganizations(searchFilter: searchFilter, paginationFilter: paginationFilter, sorting: sorting, context: context);
        }

        public Organization? Get(int id, ITransactionalContext? context = null)
        {
            var searchFilter = new OrganizationSearchFilter() { Ids = new int[] { id } };

            return this.GetOrganizations(searchFilter: searchFilter, paginationFilter: null, sorting: null, context: context).Results.FirstOrDefault();
        }

        public void Create(Organization organization, ITransactionalContext? context = null)
        {
            this.CreateOrganization(organization, context);
        }

        public void Update(Organization organization, ITransactionalContext? context = null)
        {
            this.UpdateOrganization(organization, context);
        }

        public int Delete(int id, ITransactionalContext? context = null)
        {
            return base.Delete("[Organization]", id, "Id", context);
        }

        #endregion

        #region Private Methods

        private PaginatedResult<Organization> GetOrganizations(OrganizationSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            string sql = $@"SELECT
                                [Organization].Id [Id],
                                [Organization].Name [Name],
                                [City].Id [Id],
                                [City].Name [Name]
                            FROM [Organization] [Organization]
                            INNER JOIN [City] [City] ON [City].Id = [Organization].IdCity";

            QueryBuilder.AddCommand(sql);

            ProcessSearchFilter(searchFilter);

            QueryBuilder.AddSorting(sorting, _columnsMapping);
            QueryBuilder.AddPagination(paginationFilter);

            var organizations = new List<Organization>();

            PaginatedResult<Organization> items = base.GetPaginatedResults<Organization>
                (
                    (reader) =>
                    {
                        return reader.Read<Organization, City, Organization>
                        (
                            (organization, city) =>
                            {
                                organization.City = city;

                                organizations.Add(organization);

                                return organization;
                            },
                            splitOn: "Id, Id"
                        ).AsList();
                    },
                    context
                );

            items.Results = organizations;

            return items;
        }

        private void ProcessSearchFilter(OrganizationSearchFilter? searchFilter = null)
        {
            if (searchFilter == null)
                return;

            base.AddFilterCriteria(ConditionType.In, "Organization", "Id", "ids", searchFilter.Ids);
            base.AddFilterCriteria(ConditionType.Like, "Organization", "Name", "name", searchFilter.Name);
            base.AddFilterCriteria(ConditionType.Equal, "City", "Id", "idCity", searchFilter.City?.Id);
        }

        private void CreateOrganization(Organization organization, ITransactionalContext? context = null)
        {
            string sql = @" INSERT INTO [Organization]
                                ( IdCity, Name )
                            VALUES
                                ( @idCity, @name )";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("idCity", organization.City.Id);
            QueryBuilder.AddParameter("name", organization.Name);

            QueryBuilder.AddReturnLastInsertedId();

            organization.Id = base.Execute<int>(context);
        }

        private void UpdateOrganization(Organization organization, ITransactionalContext? context = null)
        {
            string sql = @" UPDATE [Organization] SET
                                IdCity = @idCity,
                                Name = @name";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("idCity", organization.City.Id);
            QueryBuilder.AddParameter("name", organization.Name);

            QueryBuilder.AddParameter("id", organization.Id);
            QueryBuilder.AddCondition("Id = @id");

            base.ExecuteAndGetRowsAffected(context);
        }

        #endregion
    }
}
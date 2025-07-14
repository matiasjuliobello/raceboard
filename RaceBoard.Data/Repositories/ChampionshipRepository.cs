using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data.Helpers.Interfaces;
using RaceBoard.Data.Repositories.Base.Abstract;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;
using System.Data;

namespace RaceBoard.Data.Repositories
{
    public class ChampionshipRepository : AbstractRepository, IChampionshipRepository
    {
        #region Private Members

        private readonly Dictionary<string, string> _columnsMapping = new()
        {
            { "Id", "[Championship].Id" },
            { "Name", "[Championship].Name" },
            { "StartDate", "[Championship].StartDate" },
            { "EndDate", "[Championship].EndDate" },
            { "City.Id", "[City].Id" },
            { "City.Name", "[City].Name" }
        };

        #endregion

        #region Constructors

        public ChampionshipRepository(IContextResolver contextResolver, IQueryBuilder queryBuilder) : base(contextResolver, queryBuilder)
        {
        }

        #endregion

        #region IChampionshipRepository implementation

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

        public bool Exists(int id, ITransactionalContext? context = null)
        {
            return base.Exists(id, "Championship", "Id", context);
        }

        public bool ExistsDuplicate(Championship championship, ITransactionalContext? context = null)
        {
            string existsQuery = base.GetExistsDuplicateQuery("[Championship]", "[Name] = @name AND IdCity = @idCity", "Id", "@id");

            QueryBuilder.AddCommand(existsQuery);
            QueryBuilder.AddParameter("name", championship.Name);
            QueryBuilder.AddParameter("idCity", championship.City.Id);
            //QueryBuilder.AddParameter("idsOrganization", championship.Organizations.Select(x => x.Id));
            QueryBuilder.AddParameter("id", championship.Id);

            return base.Execute<bool>(context);
        }

        public PaginatedResult<Championship> Get(ChampionshipSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            return this.GetChampionships(searchFilter, paginationFilter, sorting, context);
        }

        public Championship? Get(int id, ITransactionalContext? context = null)
        {
            var searchFilter = new ChampionshipSearchFilter() { Ids = new int[] { id } };

            return this.GetChampionships(searchFilter: searchFilter, paginationFilter: null, sorting: null, context: context).Results.FirstOrDefault();
        }

        public void Create(Championship championship, ITransactionalContext? context = null)
        {
            this.CreateChampionship(championship, context);
        }

        public void Update(Championship championship, ITransactionalContext? context = null)
        {
            this.UpdateChampionship(championship, context);
        }

        public int Delete(int id, ITransactionalContext? context = null)
        {
            return base.Delete("[Championship]", id, "Id", context);
        }

        public void SetOrganizations(int idChampionship, List<Organization> organizations, ITransactionalContext? context = null)
        {
            this.SetChampionshipOrganizations(idChampionship, organizations, context);
        }

        public int DeleteOrganizations(int idChampionship, ITransactionalContext? context = null)
        {
            return this.DeleteChampionshipOrganizations(idChampionship, context);
        }

        #endregion

        #region Private Methods

        private PaginatedResult<Championship> GetChampionships(ChampionshipSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            string sql = $@"SELECT
                                [Championship].Id [Id],
                                [Championship].Name [Name],
                                [Championship].StartDate [StartDate],
                                [Championship].EndDate [EndDate],
                                (SELECT COUNT(1) FROM Team WHERE Team.IdChampionship = Championship.Id) [Teams],
                                [City].Id [Id],
                                [City].Name [Name],
                                [Country].Id [Id],
                                [Country].Name [Name],
                                [Organization].Id [Id],
                                [Organization].Name [Name],
                                [File].Id [Id],
                                [File].Description [Description],
                                [File].Name [Name],
                                [File].Path [Path]
                            FROM [Championship] [Championship]
                            INNER JOIN [City] [City] ON [City].Id = [Championship].IdCity
                            INNER JOIN [Country] [Country] ON [Country].Id = [City].IdCountry
                            INNER JOIN [Championship_Organization] [Championship_Organization] ON [Championship_Organization].IdChampionship = [Championship].Id
                            INNER JOIN [Organization] [Organization] ON [Organization].Id = Championship_Organization.IdOrganization
                            LEFT JOIN [File] [File] ON [File].Id = [Championship].IdFileImage";

            QueryBuilder.AddCommand(sql);

            ProcessSearchFilter(searchFilter);

            QueryBuilder.AddSorting(sorting, _columnsMapping);
            QueryBuilder.AddPagination(paginationFilter);

            var championships = new List<Championship>();

            PaginatedResult<Championship> items = base.GetPaginatedResults<Championship>
                (
                    (reader) =>
                    {
                        return reader.Read<Championship, City, Country, Organization, Domain.File, Championship>
                        (
                            (championship, city, country, organization, file) =>
                            {
                                var existingChampionship = championships.FirstOrDefault(x => x.Id == championship.Id);
                                if (existingChampionship == null)
                                {
                                    championships.Add(championship);
                                }
                                championship = championships.FirstOrDefault(x => x.Id == championship.Id);

                                if (championship.Organizations == null)
                                    championship.Organizations = new List<Organization>();

                                championship.Organizations.Add(organization);

                                city.Country = country;
                                championship.City = city;

                                championship.ImageFile = file;

                                return championship;
                            },
                            splitOn: "Id, Id, Id, Id, Id"
                        );
                    },
                    context
                );

            base.__FixPaginationResults(ref items, championships, paginationFilter);

            return items;
        }

        private void ProcessSearchFilter(ChampionshipSearchFilter? searchFilter = null)
        {
            if (searchFilter == null)
                return;

            base.AddFilterCriteria(ConditionType.In, "Championship", "Id", "id", searchFilter.Ids);
            base.AddFilterCriteria(ConditionType.Equal, "Championship", "IdCity", "idCity", searchFilter.City?.Id);
            base.AddFilterCriteria(ConditionType.Like, "Championship", "Name", "name", searchFilter.Name);
        }

        private void CreateChampionship(Championship championship, ITransactionalContext? context = null)
        {
            string sql = @" INSERT INTO [Championship]
                                ( IdCity, Name, StartDate, EndDate, IdFileImage )
                            VALUES
                                ( @idCity, @name, @startDate, @endDate, @idFileImage)";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("idCity", championship.City.Id);
            QueryBuilder.AddParameter("name", championship.Name);
            QueryBuilder.AddParameter("startDate", championship.StartDate);
            QueryBuilder.AddParameter("endDate", championship.EndDate);
            QueryBuilder.AddParameter("idFileImage", championship.ImageFile != null && championship.ImageFile.Id > 0 ? championship.ImageFile!.Id : null);

            QueryBuilder.AddReturnLastInsertedId();

            championship.Id = base.Execute<int>(context);
        }

        private void UpdateChampionship(Championship championship, ITransactionalContext? context = null)
        {
            string sql = @" UPDATE [Championship] SET
                                IdCity = @idCity,
                                Name = @name,
                                StartDate = @startDate,
                                EndDate = @endDate";

            bool hasImage = championship.ImageFile != null && championship.ImageFile.Id > 0;

            if (hasImage)
                sql += ", IdFileImage = @idFileImage";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("idCity", championship.City.Id);
            QueryBuilder.AddParameter("name", championship.Name);
            QueryBuilder.AddParameter("startDate", championship.StartDate);
            QueryBuilder.AddParameter("endDate", championship.EndDate);
            QueryBuilder.AddParameter("idFileImage", hasImage ? championship.ImageFile!.Id : null);

            QueryBuilder.AddParameter("id", championship.Id);
            QueryBuilder.AddCondition("Id = @id");

            base.ExecuteAndGetRowsAffected(context);
        }

        private void SetChampionshipOrganizations(int idChampionship, List<Organization> organizations, ITransactionalContext? context = null)
        {
            int affectedRecords = this.DeleteChampionshipOrganizations(idChampionship, context);

            foreach (var organization in organizations)
            {
                string sql = @" INSERT INTO [Championship_Organization]
                                ( IdChampionship, IdOrganization )
                            VALUES
                                ( @idChampionship, @idOrganization )";

                QueryBuilder.AddCommand(sql);

                QueryBuilder.AddParameter("idChampionship", idChampionship);
                QueryBuilder.AddParameter("idOrganization", organization.Id);

                QueryBuilder.AddReturnLastInsertedId();

                base.Execute<int>(context);
            }
        }

        private int DeleteChampionshipOrganizations(int idChampionship, ITransactionalContext? context = null)
        {
            return base.Delete("[Championship_Organization]", idChampionship, "IdChampionship", context);
        }

        #endregion
    }
}
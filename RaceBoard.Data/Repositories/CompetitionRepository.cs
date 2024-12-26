using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data.Helpers.Interfaces;
using RaceBoard.Data.Repositories.Base.Abstract;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;
using System.Data;

namespace RaceBoard.Data.Repositories
{
    public class CompetitionRepository : AbstractRepository, ICompetitionRepository
    {
        #region Private Members

        private readonly Dictionary<string, string> _columnsMapping = new()
        {
            { "Id", "[Competition].Id" },
            { "Name", "[Competition].Name" },
            { "StartDate", "[Competition].StartDate" },
            { "EndDate", "[Competition].EndDate" },
            { "City.Id", "[City].Id" },
            { "City.Name", "[City].Name" }
        };

        #endregion

        #region Constructors

        public CompetitionRepository(IContextResolver contextResolver, IQueryBuilder queryBuilder) : base(contextResolver, queryBuilder)
        {
        }

        #endregion

        #region ICompetitionRepository implementation

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
            return base.Exists(id, "Competition", "Id", context);
        }

        public bool ExistsDuplicate(Competition competition, ITransactionalContext? context = null)
        {
            string existsQuery = base.GetExistsDuplicateQuery("[Competition]", "[Name] = @name AND IdCity = @idCity", "Id", "@id");

            QueryBuilder.AddCommand(existsQuery);
            QueryBuilder.AddParameter("name", competition.Name);
            QueryBuilder.AddParameter("idCity", competition.City.Id);
            //QueryBuilder.AddParameter("idsOrganization", competition.Organizations.Select(x => x.Id));
            QueryBuilder.AddParameter("id", competition.Id);

            return base.Execute<bool>(context);
        }

        public PaginatedResult<Competition> Get(CompetitionSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            return this.GetCompetitions(searchFilter, paginationFilter, sorting, context);
        }

        public Competition? Get(int id, ITransactionalContext? context = null)
        {
            var searchFilter = new CompetitionSearchFilter() { Ids = new int[] { id } };

            return this.GetCompetitions(searchFilter: searchFilter, paginationFilter: null, sorting: null, context: context).Results.FirstOrDefault();
        }

        public void Create(Competition competition, ITransactionalContext? context = null)
        {
            this.CreateCompetition(competition, context);
        }

        public void Update(Competition competition, ITransactionalContext? context = null)
        {
            this.UpdateCompetition(competition, context);
        }

        public int Delete(int id, ITransactionalContext? context = null)
        {
            return base.Delete("[Competition]", id, "Id", context);
        }

        public void SetOrganizations(int idCompetition, List<Organization> organizations, ITransactionalContext? context = null)
        {
            this.SetCompetitionOrganizations(idCompetition, organizations, context);
        }

        public int DeleteOrganizations(int idCompetition, ITransactionalContext? context = null)
        {
            return this.DeleteCompetitionOrganizations(idCompetition, context);
        }

        #endregion

        #region Private Methods

        private PaginatedResult<Competition> GetCompetitions(CompetitionSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            string sql = $@"SELECT
                                [Competition].Id [Id],
                                [Competition].Name [Name],
                                [Competition].StartDate [StartDate],
                                [Competition].EndDate [EndDate],
                                (SELECT COUNT(1) FROM Team WHERE Team.IdCompetition = Competition.Id) [Teams],
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
                            FROM [Competition] [Competition]
                            INNER JOIN [City] [City] ON [City].Id = [Competition].IdCity
                            INNER JOIN [Country] [Country] ON [Country].Id = [City].IdCountry
                            INNER JOIN [Competition_Organization] [Competition_Organization] ON [Competition_Organization].IdCompetition = [Competition].Id
                            INNER JOIN [Organization] [Organization] ON [Organization].Id = Competition_Organization.IdOrganization
                            LEFT JOIN [File] [File] ON [File].Id = [Competition].IdFileImage";

            QueryBuilder.AddCommand(sql);

            ProcessSearchFilter(searchFilter);

            QueryBuilder.AddSorting(sorting, _columnsMapping);
            QueryBuilder.AddPagination(paginationFilter);

            var competitions = new List<Competition>();

            PaginatedResult<Competition> items = base.GetPaginatedResults<Competition>
                (
                    (reader) =>
                    {
                        return reader.Read<Competition, City, Country, Organization, Domain.File, Competition>
                        (
                            (competition, city, country, organization, file) =>
                            {
                                var existingCompetition = competitions.FirstOrDefault(x => x.Id == competition.Id);
                                if (existingCompetition == null)
                                {
                                    competitions.Add(competition);
                                }
                                competition = competitions.FirstOrDefault(x => x.Id == competition.Id);

                                if (competition.Organizations == null)
                                    competition.Organizations = new List<Organization>();

                                competition.Organizations.Add(organization);

                                city.Country = country;
                                competition.City = city;

                                competition.ImageFile = file;

                                return competition;
                            },
                            splitOn: "Id, Id, Id, Id, Id"
                        );
                    },
                    context
                );

            base.__FixPaginationResults(ref items, competitions, paginationFilter);

            return items;
        }

        private void ProcessSearchFilter(CompetitionSearchFilter? searchFilter = null)
        {
            if (searchFilter == null)
                return;

            base.AddFilterCriteria(ConditionType.In, "Competition", "Id", "id", searchFilter.Ids);
            base.AddFilterCriteria(ConditionType.Equal, "Competition", "IdCity", "idCity", searchFilter.City?.Id);
            base.AddFilterCriteria(ConditionType.Like, "Competition", "Name", "name", searchFilter.Name);
        }

        private void CreateCompetition(Competition competition, ITransactionalContext? context = null)
        {
            string sql = @" INSERT INTO [Competition]
                                ( IdCity, Name, StartDate, EndDate, IdFileImage )
                            VALUES
                                ( @idCity, @name, @startDate, @endDate, @idFileImage)";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("idCity", competition.City.Id);
            QueryBuilder.AddParameter("name", competition.Name);
            QueryBuilder.AddParameter("startDate", competition.StartDate);
            QueryBuilder.AddParameter("endDate", competition.EndDate);
            QueryBuilder.AddParameter("idFileImage", competition.ImageFile?.Id);

            QueryBuilder.AddReturnLastInsertedId();

            competition.Id = base.Execute<int>(context);
        }

        private void UpdateCompetition(Competition competition, ITransactionalContext? context = null)
        {
            string sql = @" UPDATE [Competition] SET
                                IdCity = @idCity,
                                Name = @name,
                                StartDate = @startDate,
                                EndDate = @endDate,
                                IdFileImage =  @idFileImage";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("idCity", competition.City.Id);
            QueryBuilder.AddParameter("name", competition.Name);
            QueryBuilder.AddParameter("startDate", competition.StartDate);
            QueryBuilder.AddParameter("endDate", competition.EndDate);
            QueryBuilder.AddParameter("idFileImage", competition.ImageFile?.Id);

            QueryBuilder.AddParameter("id", competition.Id);
            QueryBuilder.AddCondition("Id = @id");

            base.ExecuteAndGetRowsAffected(context);
        }

        private void SetCompetitionOrganizations(int idCompetition, List<Organization> organizations, ITransactionalContext? context = null)
        {
            int affectedRecords = this.DeleteCompetitionOrganizations(idCompetition, context);

            foreach (var organization in organizations)
            {
                string sql = @" INSERT INTO [Competition_Organization]
                                ( IdCompetition, IdOrganization )
                            VALUES
                                ( @idCompetition, @idOrganization )";

                QueryBuilder.AddCommand(sql);

                QueryBuilder.AddParameter("idCompetition", idCompetition);
                QueryBuilder.AddParameter("idOrganization", organization.Id);

                QueryBuilder.AddReturnLastInsertedId();

                base.Execute<int>(context);
            }
        }

        private int DeleteCompetitionOrganizations(int idCompetition, ITransactionalContext? context = null)
        {
            return base.Delete("[Competition_Organization]", idCompetition, "IdCompetition", context);
        }

        #endregion
    }
}
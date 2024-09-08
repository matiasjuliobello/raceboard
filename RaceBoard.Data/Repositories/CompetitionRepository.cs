using Dapper;
using RaceBoard.Common.Helpers.Interfaces;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data.Helpers.Interfaces;
using RaceBoard.Data.Repositories.Base.Abstract;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;
using System.Collections.Generic;
using System.Data;
using System.Text;

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
            string existsQuery = base.GetExistsQuery("[Competition]", "[Id] = @id");

            QueryBuilder.AddCommand(existsQuery);
            QueryBuilder.AddParameter("id", id);

            return base.Execute<bool>(context);
        }

        public bool ExistsDuplicate(Competition competition, ITransactionalContext? context = null)
        {
            string existsQuery = base.GetExistsDuplicateQuery("[Competition]", "[Name] = @name AND IdCity = @idCity", "Id", "@id");

            QueryBuilder.AddCommand(existsQuery);
            QueryBuilder.AddParameter("name", competition.Name);
            QueryBuilder.AddParameter("idCity", competition.City.Id);
            QueryBuilder.AddParameter("idsOrganization", competition.Organizations.Select(x => x.Id));
            QueryBuilder.AddParameter("id", competition.Id);

            return base.Execute<bool>(context);
        }

        public PaginatedResult<Competition> Get(CompetitionSearchFilter searchFilter, PaginationFilter paginationFilter, Sorting sorting, ITransactionalContext? context = null)
        {
            return this.GetCompetitions(searchFilter, paginationFilter, sorting, context);
        }

        public void Create(Competition competition, ITransactionalContext? context = null)
        {
            this.CreateCompetition(competition, context);          
        }

        public void SetOrganizations(int idCompetition, List<Organization> organizations, ITransactionalContext? context = null)
        {
            this.SetCompetitionOrganizations(idCompetition, organizations, context);
        }

        public void DeleteOrganizations(int idCompetition, ITransactionalContext? context = null)
        {
            this.DeleteCompetitionOrganizations(idCompetition, context);
        }

        public void Update(Competition competition, ITransactionalContext? context = null)
        {
            this.UpdateCompetition(competition, context);
        }

        public int Delete(int id, ITransactionalContext? context = null)
        {
            return base.Delete("[Competition]", id, "Id", context);
        }

        #endregion

        #region Private Methods

        private PaginatedResult<Competition> GetCompetitions(CompetitionSearchFilter searchFilter, PaginationFilter paginationFilter, Sorting sorting, ITransactionalContext? context = null)
        {
            string sql = $@"SELECT
                                [Competition].Id [Id],
                                [Competition].Name [Name],
                                [Competition].StartDate [StartDate],
                                [Competition].EndDate [EndDate],
                                [City].Id [Id],
                                [City].Name [Name],
                                [Organization].Id [Id],
                                [Organization].Name [Name]
                            FROM [Competition] [Competition]
                            INNER JOIN [City] [City] ON [City].Id = [Competition].IdCity
                            INNER JOIN [Competition_Organization] [Competition_Organization] ON [Competition_Organization].IdCompetition = [Competition].Id
                            INNER JOIN [Organization] [Organization] ON [Organization].Id = Competition_Organization.IdOrganization";

            QueryBuilder.AddCommand(sql);

            ProcessSearchFilter(searchFilter);

            QueryBuilder.AddSorting(sorting, _columnsMapping);
            QueryBuilder.AddPagination(paginationFilter);

            var competitions = new List<Competition>();

            PaginatedResult<Competition> items = base.GetPaginatedResults<Competition>
                (
                    (reader) =>
                    {
                        return reader.Read<Competition, City, Organization, Competition>
                        (
                            (competition, city, organization) =>
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

                                competition.City = city;

                                return competition;
                            },
                            splitOn: "Id, Id, Id"
                        );
                    },
                    context
                );

            base.__FixPaginationResults(ref items, competitions, paginationFilter);

            return items;
        }

        private void ProcessSearchFilter(CompetitionSearchFilter searchFilter)
        {
            base.AddFilterCriteria(ConditionType.In, "Competition", "Id", searchFilter.Ids);
            base.AddFilterCriteria(ConditionType.Equal, "Competition", "IdCity", searchFilter.City?.Id);
            base.AddFilterCriteria(ConditionType.Like, "Competition", "Name", searchFilter.Name);
        }

        private void CreateCompetition(Competition competition, ITransactionalContext? context = null)
        {
            string sql = @" INSERT INTO [Competition]
                                ( IdCity, Name, StartDate, EndDate )
                            VALUES
                                ( @idCity, @name, @startDate, @endDate )";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("idCity", competition.City.Id);
            QueryBuilder.AddParameter("name", competition.Name);
            QueryBuilder.AddParameter("startDate", competition.StartDate);
            QueryBuilder.AddParameter("endDate", competition.EndDate);

            QueryBuilder.AddReturnLastInsertedId();

            competition.Id = base.Execute<int>(context);
        }

        private void UpdateCompetition(Competition competition, ITransactionalContext? context = null)
        {
            string sql = @" UPDATE [Competition] SET
                                IdCity = @idCity,
                                Name = @name,
                                StartDate = @startDate,
                                EndDate = @endDate";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("idCity", competition.City.Id);
            QueryBuilder.AddParameter("name", competition.Name);
            QueryBuilder.AddParameter("startDate", competition.StartDate);
            QueryBuilder.AddParameter("endDate", competition.EndDate);

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
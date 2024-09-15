using BCrypt.Net;
using RaceBoard.Common.Helpers.Interfaces;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data.Helpers.Interfaces;
using RaceBoard.Data.Helpers.SqlBulkHelper;
using RaceBoard.Data.Repositories.Base.Abstract;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;
using System.Data;

namespace RaceBoard.Data.Repositories
{
    public class CompetitionRepository : AbstractRepository, ICompetitionRepository
    {
        public class BulkCompetitionRaceClass
        {
            public int CompetitionId { get; set; }
            public int RaceClassId { get; set; }
        }

        public class BulkCompetitionTerm
        {
            public int CompetitionRaceClassId { get; set; }
            public DateTimeOffset StartDate { get; set; }
            public DateTimeOffset EndDate { get; set; }
        }

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
        private readonly ISqlBulkInsertHelper _bulkInsertHelper;

        #endregion

        #region Constructors

        public CompetitionRepository(IContextResolver contextResolver, IQueryBuilder queryBuilder, ISqlBulkInsertHelper bulkInsertHelper) : base(contextResolver, queryBuilder)
        {
            _bulkInsertHelper = bulkInsertHelper;
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
            QueryBuilder.AddParameter("idsOrganization", competition.Organizations.Select(x => x.Id));
            QueryBuilder.AddParameter("id", competition.Id);

            return base.Execute<bool>(context);
        }

        public PaginatedResult<Competition> Get(CompetitionSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            return this.GetCompetitions(searchFilter, paginationFilter, sorting, context);
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

        public List<CompetitionRaceClass> GetRaceClasses(int idCompetition, ITransactionalContext? context = null)
        {
            return this.GetCompetitionRaceClasses(idCompetition, context);
        }

        public void AddRaceClasses(int idCompetition, List<RaceClass> raceClasses, ITransactionalContext? context = null)
        {
            this.AddCompetitionRaceClasses(idCompetition, raceClasses, context);
        }

        public int RemoveRaceClasses(int idCompetition, ITransactionalContext? context = null)
        {
            return this.RemoveCompetitionRaceClasses(idCompetition, context);
        }
        
        public List<CompetitionTerm> GetRegistrationTerms(int idCompetition, ITransactionalContext? context = null)
        {
            return this.GetCompetitionRegistrationTerms(idCompetition, context);
        }

        public void AddRegistrationTerms(List<CompetitionRegistrationTerm> registrationTerms, ITransactionalContext? context = null)
        {
            this.AddCompetitionRegistrationTerms(registrationTerms, context);
        }
        
        public int RemoveRegistrationTerms(int idCompetition, ITransactionalContext? context = null)
        {
            return this.RemoveCompetitionRegistrationTerms(idCompetition, context);
        }

        public List<CompetitionTerm> GetAccreditationTerms(int idCompetition, ITransactionalContext? context = null)
        {
            return this.GetCompetitionAccreditationTerms(idCompetition, context);
        }

        public void AddAccreditationTerms(List<CompetitionAccreditationTerm> accreditationTerms, ITransactionalContext? context = null)
        {
            this.AddCompetitionAccreditationTerms(accreditationTerms, context);
        }

        public int RemoveAccreditationTerms(int idCompetition, ITransactionalContext? context = null)
        {
            return this.RemoveCompetitionAccreditationTerms(idCompetition, context);
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

        private List<CompetitionRaceClass> GetCompetitionRaceClasses(int idCompetition, ITransactionalContext? context = null)
        {
            string sql = $@"SELECT
                                [Competition_RaceClass].Id [Id],
                                [Competition].Id [Id],
                                [Competition].Name [Name],
                                [RaceClass].Id [Id],
                                [RaceClass].Name [Name],
                                [RaceCategory].Id [Id],
                                [RaceCategory].Name [Name]
                            FROM [Competition_RaceClass] [Competition_RaceClass]
                            INNER JOIN [Competition] [Competition] ON [Competition].Id = [Competition_RaceClass].IdCompetition
                            INNER JOIN [RaceClass] [RaceClass] ON [RaceClass].Id = [Competition_RaceClass].IdRaceClass
                            INNER JOIN [RaceCategory] [RaceCategory] ON [RaceCategory].Id = [RaceClass].IdRaceCategory";

            base.AddFilterCriteria(ConditionType.Equal, "Competition", "Id", "idCompetition", idCompetition);

            QueryBuilder.AddCommand(sql);

            var competitionRaceClasses = new List<CompetitionRaceClass>();

            base.GetReader(
                (x) =>
                {
                    competitionRaceClasses = x.Read<CompetitionRaceClass, Competition, RaceClass, RaceCategory, CompetitionRaceClass>
                        (
                            (competitionRaceClass, competition, raceClass, raceCategory) =>
                            {
                                competitionRaceClass.Competition = competition;
                                raceClass.RaceCategory = raceCategory;
                                competitionRaceClass.RaceClass = raceClass;

                                return competitionRaceClass;
                            },
                            splitOn: "Id, Id, Id, Id"
                        ).ToList();
                },
                context
            );

            return competitionRaceClasses;
        }

        private void AddCompetitionRaceClasses(int idCompetition, List<RaceClass> raceClasses, ITransactionalContext? context = null)
        {
            var data = new List<BulkCompetitionRaceClass>();
            raceClasses.ForEach(x => data.Add(new BulkCompetitionRaceClass()
            {
                CompetitionId = idCompetition,
                RaceClassId = x.Id
            }));

            var sqlBulkSettings = new SqlBulkSettings<BulkCompetitionRaceClass>();
            sqlBulkSettings.TableName = "Competition_RaceClass";
            sqlBulkSettings.Mappings.Add(new SqlBulkColumnMapping("CompetitionId", "IdCompetition"));
            sqlBulkSettings.Mappings.Add(new SqlBulkColumnMapping("RaceClassId", "IdRaceClass"));
            sqlBulkSettings.Data = data;

            _bulkInsertHelper.PerformBulkInsert(sqlBulkSettings, context);
        }

        private int RemoveCompetitionRaceClasses(int idCompetition, ITransactionalContext? context = null)
        {
            return base.Delete("[Competition_RaceClass]", idCompetition, "IdCompetition", context);
        }

        private List<CompetitionTerm> GetCompetitionRegistrationTerms(int idCompetition, ITransactionalContext? context = null)
        {
            return this.GetCompetitionTerms<CompetitionRegistrationTerm>("Competition_RegistrationTerm", idCompetition, context);
        }

        private void AddCompetitionRegistrationTerms(List<CompetitionRegistrationTerm> registrationTerms, ITransactionalContext? context = null)
        {
            //var competitionRaceClasses = this.GetCompetitionRaceClasses(registrationTerms[0].Competition.Id, context);

            //var data = new List<BulkCompetitionTerm>();
            //registrationTerms.ForEach(x => data.Add(new BulkCompetitionTerm()
            //{
            //    CompetitionRaceClassId = competitionRaceClasses.First(y => y.RaceClass.Id == x.RaceClass.Id).Id,
            //    StartDate = x.StartDate,
            //    EndDate = x.EndDate
            //}));

            //var sqlBulkSettings = new SqlBulkSettings<BulkCompetitionTerm>();
            //sqlBulkSettings.TableName = "Competition_RegistrationTerm";
            //sqlBulkSettings.Mappings.Add(new SqlBulkColumnMapping("CompetitionRaceClassId", "IdCompetition_RaceClass"));
            //sqlBulkSettings.Mappings.Add(new SqlBulkColumnMapping("StartDate", "StartDate"));
            //sqlBulkSettings.Mappings.Add(new SqlBulkColumnMapping("EndDate", "EndDate"));
            //sqlBulkSettings.Data = data;

            //_bulkInsertHelper.PerformBulkInsert(sqlBulkSettings, context);

            this.AddCompetitionTerms("Competition_RegistrationTerm", registrationTerms, context);
        }

        private int RemoveCompetitionRegistrationTerms(int idCompetition, ITransactionalContext? context = null)
        {
            return this.RemoveCompetitionTerms(idCompetition, "Competition_RegistrationTerm", context);
        }

        private List<CompetitionTerm> GetCompetitionAccreditationTerms(int idCompetition, ITransactionalContext? context = null)
        {
            return this.GetCompetitionTerms<CompetitionAccreditationTerm>("Competition_AccreditationTerm", idCompetition, context);
        }

        private void AddCompetitionAccreditationTerms(List<CompetitionAccreditationTerm> accreditationTerms, ITransactionalContext? context = null)
        {
            this.AddCompetitionTerms("Competition_AccreditationTerm", accreditationTerms, context);
        }

        private int RemoveCompetitionAccreditationTerms(int idCompetition, ITransactionalContext? context = null)
        {
            return this.RemoveCompetitionTerms(idCompetition, "Competition_AccreditationTerm", context);
        }

        private List<CompetitionTerm> GetCompetitionTerms<T>(string tableName, int idCompetition, ITransactionalContext? context = null) where T : CompetitionTerm
        {
            string sql = $@"SELECT
                                [{tableName}].Id [Id],
                                [{tableName}].StartDate [StartDate],
                                [{tableName}].EndDate [EndDate],
                                [Competition].Id [Id],
                                [Competition].Name [Name],
                                [RaceClass].Id [Id],
                                [RaceClass].Name [Name],
                                [RaceCategory].Id [Id],
                                [RaceCategory].Name [Name]
                            FROM [{tableName}] [{tableName}]
                            INNER JOIN [Competition_RaceClass] [Competition_RaceClass] ON [Competition_RaceClass].Id = [{tableName}].IdCompetition_RaceClass
                            INNER JOIN [Competition] [Competition] ON [Competition].Id = [Competition_RaceClass].IdCompetition
                            INNER JOIN [RaceClass] [RaceClass] ON [RaceClass].Id = [Competition_RaceClass].IdRaceClass
                            INNER JOIN [RaceCategory] [RaceCategory] ON [RaceCategory].Id = [RaceClass].IdRaceCategory";

            base.AddFilterCriteria(ConditionType.Equal, "Competition", "Id", "idCompetition", idCompetition);

            QueryBuilder.AddCommand(sql);

            var competitionTerms = new List<CompetitionTerm>();

            base.GetReader(
                (x) =>
                {
                    competitionTerms = x.Read<T, Competition, RaceClass, RaceCategory, CompetitionTerm>
                        (
                            (competitionTerm, competition, raceClass, raceCategory) =>
                            {
                                competitionTerm.Competition = competition;
                                raceClass.RaceCategory = raceCategory;
                                competitionTerm.RaceClass = raceClass;

                                return competitionTerm;
                            },
                            splitOn: "Id, Id, Id, Id"
                        ).ToList();
                },
                context
            );

            return competitionTerms;
        }

        private void AddCompetitionTerms<T>(string tableName, List<T> competitionTerms, ITransactionalContext? context = null) where T: CompetitionTerm
        {
            var competitionRaceClasses = this.GetCompetitionRaceClasses(competitionTerms[0].Competition.Id, context);

            var data = new List<BulkCompetitionTerm>();
            competitionTerms.ForEach(x => data.Add(new BulkCompetitionTerm()
            {
                CompetitionRaceClassId = competitionRaceClasses.First(y => y.RaceClass.Id == x.RaceClass.Id).Id,
                StartDate = x.StartDate,
                EndDate = x.EndDate
            }));

            var sqlBulkSettings = new SqlBulkSettings<BulkCompetitionTerm>();
            sqlBulkSettings.TableName = tableName;
            sqlBulkSettings.Mappings.Add(new SqlBulkColumnMapping("CompetitionRaceClassId", "IdCompetition_RaceClass"));
            sqlBulkSettings.Mappings.Add(new SqlBulkColumnMapping("StartDate", "StartDate"));
            sqlBulkSettings.Mappings.Add(new SqlBulkColumnMapping("EndDate", "EndDate"));
            sqlBulkSettings.Data = data;

            _bulkInsertHelper.PerformBulkInsert(sqlBulkSettings, context);
        }

        private int RemoveCompetitionTerms(int idCompetition, string tableName, ITransactionalContext? context = null)
        {
            string condition = "IdCompetition_RaceClass IN (SELECT Id FROM [Competition_RaceClass] WHERE IdCompetition = @idCompetition)";

            return base.Delete(tableName, "idCompetition", idCompetition, condition, context);
        }

        #endregion
    }
}
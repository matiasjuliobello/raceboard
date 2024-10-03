using RaceBoard.Data.Helpers.Interfaces;
using RaceBoard.Data.Helpers.SqlBulkHelper;
using RaceBoard.Data.Repositories.Base.Abstract;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;
using System.Xml.Linq;

namespace RaceBoard.Data.Repositories
{
    public class CompetitionGroupRepository : AbstractRepository, ICompetitionGroupRepository
    {
        public class BulkCompetitionGroupRaceClass
        {
            public int IdCompetitionGroup { get; set; }
            public int IdRaceClass { get; set; }

            public BulkCompetitionGroupRaceClass(int idCompetition, int idRaceClass)
            {
                this.IdCompetitionGroup = idCompetition;
                this.IdRaceClass = idRaceClass;
            }
        }

        #region Private Members

        private readonly Dictionary<string, string> _columnsMapping = new()
        {
            { "Id", "[CompetitionGroup].Id" }
        };
        private readonly ISqlBulkInsertHelper _bulkInsertHelper;

        #endregion

        #region Constructors

        public CompetitionGroupRepository(IContextResolver contextResolver, IQueryBuilder queryBuilder, ISqlBulkInsertHelper bulkInsertHelper) : base(contextResolver, queryBuilder)
        {
            _bulkInsertHelper = bulkInsertHelper;
        }

        #endregion

        #region ICompetitionGroupRepository implementation

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
            return base.Exists(id, "CompetitionGroup", "Id", context);
        }

        public bool ExistsDuplicate(CompetitionGroup competitionGroup, ITransactionalContext? context = null)
        {
            string condition = "[IdCompetition] = @idCompetition AND [Name] = @name";

            string existsQuery = base.GetExistsDuplicateQuery("[CompetitionGroup]", condition, "Id", "@id");

            QueryBuilder.AddCommand(existsQuery);
            QueryBuilder.AddParameter("idCompetition", competitionGroup.Competition.Id);
            QueryBuilder.AddParameter("name", competitionGroup.Name);
            QueryBuilder.AddParameter("id", competitionGroup.Id);

            return base.Execute<bool>(context);
        }

        public List<CompetitionGroup> Get(int idCompetition, ITransactionalContext? context = null)
        {
            return this.GetCompetitionGroups(id: null, idCompetition: idCompetition, context);
        }
        public CompetitionGroup GetById(int id, ITransactionalContext? context = null)
        {
            return this.GetCompetitionGroups(id: id, idCompetition: null, context).FirstOrDefault();
        }

        public void Create(CompetitionGroup competitionGroup, ITransactionalContext? context = null)
        {
            this.CreateCompetitionGroup(competitionGroup, context);
        }

        public void Update(CompetitionGroup competitionGroup, ITransactionalContext? context = null)
        {
            this.UpdateCompetitionGroup(competitionGroup, context);
        }

        public int Delete(int id, ITransactionalContext? context = null)
        {
            return base.Delete("[CompetitionGroup]", id, "Id", context);
        }

        public void CreateRaceClasses(int idCompetitionGroup, List<RaceClass> raceClasses, ITransactionalContext? context = null)
        {
            this.CreateCompetitionGroupRaceClasses(idCompetitionGroup, raceClasses, context);
        }

        public int DeleteRaceClasses(int id, ITransactionalContext? context = null)
        {
            return this.DeleteCompetitionGroupRaceClasses(id, context);
        }

        #endregion

        #region Private Methods

        private List<CompetitionGroup> GetCompetitionGroups(int? id = null, int? idCompetition = null, ITransactionalContext? context = null)
        {
            string sql = $@"SELECT
                                [CompetitionGroup].Id [Id],
                                [CompetitionGroup].Name [Name],
                                [CompetitionGroup].CompetitionStartDate [CompetitionStartDate],
                                [CompetitionGroup].CompetitionEndDate [CompetitionEndDate],	
                                [CompetitionGroup].RegistrationStartDate [RegistrationStartDate],	
                                [CompetitionGroup].RegistrationEndDate [RegistrationEndDate],	
                                [CompetitionGroup].AccreditationStartDate [AccreditationStartDate],
                                [CompetitionGroup].AccreditationEndDate [AccreditationEndDate]
                            FROM [CompetitionGroup] [CompetitionGroup]";

            QueryBuilder.AddCommand(sql);

            if (id.HasValue)
                base.AddFilterCriteria(ConditionType.Equal, "[CompetitionGroup]", "Id", "id", id);

            if (idCompetition.HasValue)
                base.AddFilterCriteria(ConditionType.Equal, "[CompetitionGroup]", "IdCompetition", "idCompetition", idCompetition);


            var competitionGroups = base.GetMultipleResults<CompetitionGroup>(context).ToList();

            foreach (var competitionGroup in competitionGroups)
            {
                var raceClasses = this.GetCompetitionGroupRaceClasses(competitionGroup.Id, context);
                competitionGroup.RaceClasses.AddRange(raceClasses);

                var totals =  this.GetGroupTotals(competitionGroup.Id, context);
                competitionGroup.RegistrationTotalCount = totals.RegistrationCount;
                competitionGroup.AccreditationTotalCount = totals.AccreditationCount;
                competitionGroup.CompetitionTotalCount = totals.CompetitionCount;
            }

            return competitionGroups;
        }

        private List<RaceClass> GetCompetitionGroupRaceClasses(int idCompetitionGroup, ITransactionalContext? context = null)
        {
            string sql = $@"SELECT
                                [RaceClass].Id [Id],
                                [RaceClass].Name [Name]
                            FROM [CompetitionGroup_RaceClass] [CompetitionGroup_RaceClass]
                            INNER JOIN [RaceClass] [RaceClass] ON [RaceClass].Id = [CompetitionGroup_RaceClass].IdRaceClass";

            QueryBuilder.AddCommand(sql);

            base.AddFilterCriteria(ConditionType.Equal, "[CompetitionGroup_RaceClass]", "IdCompetitionGroup", "idCompetitionGroup", idCompetitionGroup);

            return base.GetMultipleResults<RaceClass>(context).ToList();
        }

        private CompetitionGroupTotals GetGroupTotals(int idCompetitionGroup, ITransactionalContext? context = null)
        {
            string sql = @$"SELECT 
	                            ISNULL(
	                            (
		                            SELECT COUNT(1)
		                            FROM [Team]
		                            INNER JOIN [Competition] ON [Competition].Id = [Team].IdCompetition
		                            INNER JOIN [CompetitionGroup] ON [CompetitionGroup].IdCompetition = [Team].IdCompetition
		                            INNER JOIN [CompetitionGroup_RaceClass] ON ([CompetitionGroup_RaceClass].IdCompetitionGroup = [CompetitionGroup].Id AND [CompetitionGroup_RaceClass].IdRaceClass = [Team].IdRaceClass)
                                    INNER JOIN [RaceClass] ON ( [CompetitionGroup_RaceClass].IdRaceClass = [RaceClass].Id )
		                            WHERE [CompetitionGroup].Id = {idCompetitionGroup}
	                            ), 0) [RegistrationCount],
	                            ISNULL(
	                            (
		                            SELECT COUNT(1)
		                            FROM [Team]
		                            INNER JOIN [Competition] ON [Competition].Id = [Team].IdCompetition
		                            INNER JOIN [CompetitionGroup] ON [CompetitionGroup].IdCompetition = [Team].IdCompetition
		                            INNER JOIN [CompetitionGroup_RaceClass] ON ([CompetitionGroup_RaceClass].IdCompetitionGroup = [CompetitionGroup].Id AND [CompetitionGroup_RaceClass].IdRaceClass = [Team].IdRaceClass)
                                    INNER JOIN [RaceClass] ON ( [CompetitionGroup_RaceClass].IdRaceClass = [RaceClass].Id )
                                    WHERE [CompetitionGroup].Id = {idCompetitionGroup} AND [Team].HasAccreditation = 1
	                            ), 0) [AccreditationCount],
	                            ISNULL(
	                            (
		                            SELECT COUNT(1)
		                            FROM [Team]
		                            INNER JOIN [Race] ON ( [Race].IdRaceClass = [Team].IdRaceClass AND [Race].IdCompetition = [Team].IdCompetition )
		                            INNER JOIN [Competition] ON [Competition].Id = [Team].IdCompetition
		                            INNER JOIN [CompetitionGroup] ON [CompetitionGroup].IdCompetition = [Competition].Id
		                            INNER JOIN [CompetitionGroup_RaceClass] ON ([CompetitionGroup_RaceClass].IdCompetitionGroup = [CompetitionGroup].Id AND [CompetitionGroup_RaceClass].IdRaceClass = [Team].IdRaceClass)
		                            WHERE [CompetitionGroup].Id = {idCompetitionGroup}
	                            ), 0) [CompetitionCount]";

            QueryBuilder.AddCommand(sql);

            return base.GetSingleResult<CompetitionGroupTotals>(context);
        }

        private void CreateCompetitionGroup(CompetitionGroup competitionGroup, ITransactionalContext? context = null)
        {
            string sql = @" INSERT INTO [CompetitionGroup]
                                (
                                    IdCompetition, Name, 
                                    CompetitionStartDate, CompetitionEndDate,
                                    AccreditationStartDate, AccreditationEndDate,
                                    RegistrationStartDate, RegistrationEndDate 
                                )
                            VALUES
                                (
                                    @idCompetition, @name,
                                    @competitionStartDate,   @competitionEndDate,
                                    @accreditationStartDate, @accreditationEndDate,
                                    @registrationStartDate,  @registrationEndDate 
                                )";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("idCompetition", competitionGroup.Competition.Id);
            QueryBuilder.AddParameter("name", competitionGroup.Name);
            QueryBuilder.AddParameter("competitionStartDate", competitionGroup.CompetitionStartDate);
            QueryBuilder.AddParameter("competitionEndDate", competitionGroup.CompetitionEndDate);
            QueryBuilder.AddParameter("accreditationStartDate", competitionGroup.AccreditationStartDate);
            QueryBuilder.AddParameter("accreditationEndDate", competitionGroup.AccreditationEndDate);
            QueryBuilder.AddParameter("registrationStartDate", competitionGroup.RegistrationStartDate);
            QueryBuilder.AddParameter("registrationEndDate", competitionGroup.RegistrationEndDate);

            QueryBuilder.AddReturnLastInsertedId();

            competitionGroup.Id = base.Execute<int>(context);
        }

        private void UpdateCompetitionGroup(CompetitionGroup competitionGroup, ITransactionalContext? context = null)
        {
            string sql = @" UPDATE [CompetitionGroup] SET
                                Name = @name,
                                CompetitionStartDate = @competitionStartDate, 
                                CompetitionEndDate = @competitionEndDate,
                                AccreditationStartDate = @accreditationStartDate, 
                                AccreditationEndDate = @accreditationEndDate,
                                RegistrationStartDate = @registrationStartDate, 
                                RegistrationEndDate = @registrationEndDate";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("name", competitionGroup.Name);
            QueryBuilder.AddParameter("competitionStartDate", competitionGroup.CompetitionStartDate);
            QueryBuilder.AddParameter("competitionEndDate", competitionGroup.CompetitionEndDate);
            QueryBuilder.AddParameter("accreditationStartDate", competitionGroup.AccreditationStartDate);
            QueryBuilder.AddParameter("accreditationEndDate", competitionGroup.AccreditationEndDate);
            QueryBuilder.AddParameter("registrationStartDate", competitionGroup.RegistrationStartDate);
            QueryBuilder.AddParameter("registrationEndDate", competitionGroup.RegistrationEndDate);

            QueryBuilder.AddParameter("id", competitionGroup.Id);
            QueryBuilder.AddCondition("Id = @id");

            base.ExecuteAndGetRowsAffected(context);
        }

        private void CreateCompetitionGroupRaceClasses(int id, List<RaceClass> raceClasses, ITransactionalContext? context = null)
        {
            var bulkItems = new List<BulkCompetitionGroupRaceClass>();

            raceClasses.ForEach(x => { bulkItems.Add(new BulkCompetitionGroupRaceClass(id, x.Id)); });


            var sqlBulkSettings = new SqlBulkSettings<BulkCompetitionGroupRaceClass>();
            sqlBulkSettings.TableName = "CompetitionGroup_RaceClass";
            sqlBulkSettings.Mappings.Add(new SqlBulkColumnMapping(nameof(BulkCompetitionGroupRaceClass.IdCompetitionGroup), "IdCompetitionGroup"));
            sqlBulkSettings.Mappings.Add(new SqlBulkColumnMapping(nameof(BulkCompetitionGroupRaceClass.IdRaceClass), "IdRaceClass"));
            sqlBulkSettings.Data = bulkItems;

            _bulkInsertHelper.PerformBulkInsert(sqlBulkSettings, context);
        }

        private int DeleteCompetitionGroupRaceClasses(int id, ITransactionalContext? context = null)
        {
            return base.Delete("[CompetitionGroup_RaceClass]", id, "IdCompetitionGroup", context);
        }

        #endregion
    }
}

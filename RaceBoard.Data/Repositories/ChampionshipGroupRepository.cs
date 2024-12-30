using RaceBoard.Data.Helpers.Interfaces;
using RaceBoard.Data.Helpers.SqlBulkHelper;
using RaceBoard.Data.Repositories.Base.Abstract;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;
using System.Xml.Linq;

namespace RaceBoard.Data.Repositories
{
    public class ChampionshipGroupRepository : AbstractRepository, IChampionshipGroupRepository
    {
        public class BulkChampionshipGroupRaceClass
        {
            public int IdChampionshipGroup { get; set; }
            public int IdRaceClass { get; set; }

            public BulkChampionshipGroupRaceClass(int idChampionship, int idRaceClass)
            {
                this.IdChampionshipGroup = idChampionship;
                this.IdRaceClass = idRaceClass;
            }
        }

        #region Private Members

        private readonly Dictionary<string, string> _columnsMapping = new()
        {
            { "Id", "[ChampionshipGroup].Id" }
        };
        private readonly ISqlBulkInsertHelper _bulkInsertHelper;

        #endregion

        #region Constructors

        public ChampionshipGroupRepository(IContextResolver contextResolver, IQueryBuilder queryBuilder, ISqlBulkInsertHelper bulkInsertHelper) : base(contextResolver, queryBuilder)
        {
            _bulkInsertHelper = bulkInsertHelper;
        }

        #endregion

        #region IChampionshipGroupRepository implementation

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
            return base.Exists(id, "ChampionshipGroup", "Id", context);
        }

        public bool ExistsDuplicate(ChampionshipGroup championshipGroup, ITransactionalContext? context = null)
        {
            string condition = "[IdChampionship] = @idChampionship AND [Name] = @name";

            string existsQuery = base.GetExistsDuplicateQuery("[ChampionshipGroup]", condition, "Id", "@id");

            QueryBuilder.AddCommand(existsQuery);
            QueryBuilder.AddParameter("idChampionship", championshipGroup.Championship.Id);
            QueryBuilder.AddParameter("name", championshipGroup.Name);
            QueryBuilder.AddParameter("id", championshipGroup.Id);

            return base.Execute<bool>(context);
        }

        public List<ChampionshipGroup> Get(int idChampionship, ITransactionalContext? context = null)
        {
            return this.GetChampionshipGroups(id: null, idChampionship: idChampionship, context);
        }
        public ChampionshipGroup GetById(int id, ITransactionalContext? context = null)
        {
            return this.GetChampionshipGroups(id: id, idChampionship: null, context).FirstOrDefault();
        }

        public void Create(ChampionshipGroup championshipGroup, ITransactionalContext? context = null)
        {
            this.CreateChampionshipGroup(championshipGroup, context);
        }

        public void Update(ChampionshipGroup championshipGroup, ITransactionalContext? context = null)
        {
            this.UpdateChampionshipGroup(championshipGroup, context);
        }

        public int Delete(int id, ITransactionalContext? context = null)
        {
            return base.Delete("[ChampionshipGroup]", id, "Id", context);
        }

        public void CreateRaceClasses(int idChampionshipGroup, List<RaceClass> raceClasses, ITransactionalContext? context = null)
        {
            this.CreateChampionshipGroupRaceClasses(idChampionshipGroup, raceClasses, context);
        }

        public int DeleteRaceClasses(int id, ITransactionalContext? context = null)
        {
            return this.DeleteChampionshipGroupRaceClasses(id, context);
        }

        #endregion

        #region Private Methods

        private List<ChampionshipGroup> GetChampionshipGroups(int? id = null, int? idChampionship = null, ITransactionalContext? context = null)
        {
            string sql = $@"SELECT
                                [ChampionshipGroup].Id [Id],
                                [ChampionshipGroup].Name [Name],
                                [ChampionshipGroup].ChampionshipStartDate [ChampionshipStartDate],
                                [ChampionshipGroup].ChampionshipEndDate [ChampionshipEndDate],	
                                [ChampionshipGroup].RegistrationStartDate [RegistrationStartDate],	
                                [ChampionshipGroup].RegistrationEndDate [RegistrationEndDate],	
                                [ChampionshipGroup].AccreditationStartDate [AccreditationStartDate],
                                [ChampionshipGroup].AccreditationEndDate [AccreditationEndDate]
                            FROM [ChampionshipGroup] [ChampionshipGroup]";

            QueryBuilder.AddCommand(sql);

            if (id.HasValue)
                base.AddFilterCriteria(ConditionType.Equal, "[ChampionshipGroup]", "Id", "id", id);

            if (idChampionship.HasValue)
                base.AddFilterCriteria(ConditionType.Equal, "[ChampionshipGroup]", "IdChampionship", "idChampionship", idChampionship);


            var championshipGroups = base.GetMultipleResults<ChampionshipGroup>(context).ToList();

            foreach (var championshipGroup in championshipGroups)
            {
                var raceClasses = this.GetChampionshipGroupRaceClasses(championshipGroup.Id, context);
                championshipGroup.RaceClasses.AddRange(raceClasses);

                // TODO disabled this code when removed IdRaceClass from [Race] table (needs rework now)
                //var totals =  this.GetGroupTotals(championshipGroup.Id, context);
                //championshipGroup.RegistrationTotalCount = totals.RegistrationCount;
                //championshipGroup.AccreditationTotalCount = totals.AccreditationCount;
                //championshipGroup.ChampionshipTotalCount = totals.ChampionshipCount;
            }

            return championshipGroups;
        }

        private List<RaceClass> GetChampionshipGroupRaceClasses(int idChampionshipGroup, ITransactionalContext? context = null)
        {
            string sql = $@"SELECT
                                [RaceClass].Id [Id],
                                [RaceClass].Name [Name]
                            FROM [ChampionshipGroup_RaceClass] [ChampionshipGroup_RaceClass]
                            INNER JOIN [RaceClass] [RaceClass] ON [RaceClass].Id = [ChampionshipGroup_RaceClass].IdRaceClass";

            QueryBuilder.AddCommand(sql);

            base.AddFilterCriteria(ConditionType.Equal, "[ChampionshipGroup_RaceClass]", "IdChampionshipGroup", "idChampionshipGroup", idChampionshipGroup);

            return base.GetMultipleResults<RaceClass>(context).ToList();
        }

        private ChampionshipGroupTotals GetGroupTotals(int idChampionshipGroup, ITransactionalContext? context = null)
        {
            string sql = @$"SELECT 
	                            ISNULL(
	                            (
		                            SELECT COUNT(1)
		                            FROM [Team]
		                            INNER JOIN [Championship] ON [Championship].Id = [Team].IdChampionship
		                            INNER JOIN [ChampionshipGroup] ON [ChampionshipGroup].IdChampionship = [Team].IdChampionship
		                            INNER JOIN [ChampionshipGroup_RaceClass] ON ([ChampionshipGroup_RaceClass].IdChampionshipGroup = [ChampionshipGroup].Id AND [ChampionshipGroup_RaceClass].IdRaceClass = [Team].IdRaceClass)
                                    INNER JOIN [RaceClass] ON ( [ChampionshipGroup_RaceClass].IdRaceClass = [RaceClass].Id )
		                            WHERE [ChampionshipGroup].Id = {idChampionshipGroup}
	                            ), 0) [RegistrationCount],
	                            ISNULL(
	                            (
		                            SELECT COUNT(1)
		                            FROM [Team]
		                            INNER JOIN [Championship] ON [Championship].Id = [Team].IdChampionship
		                            INNER JOIN [ChampionshipGroup] ON [ChampionshipGroup].IdChampionship = [Team].IdChampionship
		                            INNER JOIN [ChampionshipGroup_RaceClass] ON ([ChampionshipGroup_RaceClass].IdChampionshipGroup = [ChampionshipGroup].Id AND [ChampionshipGroup_RaceClass].IdRaceClass = [Team].IdRaceClass)
                                    INNER JOIN [RaceClass] ON ( [ChampionshipGroup_RaceClass].IdRaceClass = [RaceClass].Id )
                                    WHERE [ChampionshipGroup].Id = {idChampionshipGroup} AND [Team].HasAccreditation = 1
	                            ), 0) [AccreditationCount],
	                            ISNULL(
	                            (
		                            SELECT COUNT(1)
		                            FROM [Team]
		                            INNER JOIN [Race] ON ( [Race].IdRaceClass = [Team].IdRaceClass AND [Race].IdChampionship = [Team].IdChampionship )
		                            INNER JOIN [Championship] ON [Championship].Id = [Team].IdChampionship
		                            INNER JOIN [ChampionshipGroup] ON [ChampionshipGroup].IdChampionship = [Championship].Id
		                            INNER JOIN [ChampionshipGroup_RaceClass] ON ([ChampionshipGroup_RaceClass].IdChampionshipGroup = [ChampionshipGroup].Id AND [ChampionshipGroup_RaceClass].IdRaceClass = [Team].IdRaceClass)
		                            WHERE [ChampionshipGroup].Id = {idChampionshipGroup}
	                            ), 0) [ChampionshipCount]";

            QueryBuilder.AddCommand(sql);

            return base.GetSingleResult<ChampionshipGroupTotals>(context);
        }

        private void CreateChampionshipGroup(ChampionshipGroup championshipGroup, ITransactionalContext? context = null)
        {
            string sql = @" INSERT INTO [ChampionshipGroup]
                                (
                                    IdChampionship, Name, 
                                    ChampionshipStartDate, ChampionshipEndDate,
                                    AccreditationStartDate, AccreditationEndDate,
                                    RegistrationStartDate, RegistrationEndDate 
                                )
                            VALUES
                                (
                                    @idChampionship, @name,
                                    @championshipStartDate,   @championshipEndDate,
                                    @accreditationStartDate, @accreditationEndDate,
                                    @registrationStartDate,  @registrationEndDate 
                                )";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("idChampionship", championshipGroup.Championship.Id);
            QueryBuilder.AddParameter("name", championshipGroup.Name);
            QueryBuilder.AddParameter("championshipStartDate", championshipGroup.ChampionshipStartDate);
            QueryBuilder.AddParameter("championshipEndDate", championshipGroup.ChampionshipEndDate);
            QueryBuilder.AddParameter("accreditationStartDate", championshipGroup.AccreditationStartDate);
            QueryBuilder.AddParameter("accreditationEndDate", championshipGroup.AccreditationEndDate);
            QueryBuilder.AddParameter("registrationStartDate", championshipGroup.RegistrationStartDate);
            QueryBuilder.AddParameter("registrationEndDate", championshipGroup.RegistrationEndDate);

            QueryBuilder.AddReturnLastInsertedId();

            championshipGroup.Id = base.Execute<int>(context);
        }

        private void UpdateChampionshipGroup(ChampionshipGroup championshipGroup, ITransactionalContext? context = null)
        {
            string sql = @" UPDATE [ChampionshipGroup] SET
                                Name = @name,
                                ChampionshipStartDate = @championshipStartDate, 
                                ChampionshipEndDate = @championshipEndDate,
                                AccreditationStartDate = @accreditationStartDate, 
                                AccreditationEndDate = @accreditationEndDate,
                                RegistrationStartDate = @registrationStartDate, 
                                RegistrationEndDate = @registrationEndDate";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("name", championshipGroup.Name);
            QueryBuilder.AddParameter("championshipStartDate", championshipGroup.ChampionshipStartDate);
            QueryBuilder.AddParameter("championshipEndDate", championshipGroup.ChampionshipEndDate);
            QueryBuilder.AddParameter("accreditationStartDate", championshipGroup.AccreditationStartDate);
            QueryBuilder.AddParameter("accreditationEndDate", championshipGroup.AccreditationEndDate);
            QueryBuilder.AddParameter("registrationStartDate", championshipGroup.RegistrationStartDate);
            QueryBuilder.AddParameter("registrationEndDate", championshipGroup.RegistrationEndDate);

            QueryBuilder.AddParameter("id", championshipGroup.Id);
            QueryBuilder.AddCondition("Id = @id");

            base.ExecuteAndGetRowsAffected(context);
        }

        private void CreateChampionshipGroupRaceClasses(int id, List<RaceClass> raceClasses, ITransactionalContext? context = null)
        {
            var bulkItems = new List<BulkChampionshipGroupRaceClass>();

            raceClasses.ForEach(x => { bulkItems.Add(new BulkChampionshipGroupRaceClass(id, x.Id)); });


            var sqlBulkSettings = new SqlBulkSettings<BulkChampionshipGroupRaceClass>();
            sqlBulkSettings.TableName = "ChampionshipGroup_RaceClass";
            sqlBulkSettings.Mappings.Add(new SqlBulkColumnMapping(nameof(BulkChampionshipGroupRaceClass.IdChampionshipGroup), "IdChampionshipGroup"));
            sqlBulkSettings.Mappings.Add(new SqlBulkColumnMapping(nameof(BulkChampionshipGroupRaceClass.IdRaceClass), "IdRaceClass"));
            sqlBulkSettings.Data = bulkItems;

            _bulkInsertHelper.PerformBulkInsert(sqlBulkSettings, context);
        }

        private int DeleteChampionshipGroupRaceClasses(int id, ITransactionalContext? context = null)
        {
            return base.Delete("[ChampionshipGroup_RaceClass]", id, "IdChampionshipGroup", context);
        }

        #endregion
    }
}

using Dapper;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data.Helpers.Interfaces;
using RaceBoard.Data.Helpers.SqlBulkHelper;
using RaceBoard.Data.Repositories.Base.Abstract;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;

namespace RaceBoard.Data.Repositories
{
    public class CommitteeBoatReturnRepository : AbstractRepository, ICommitteeBoatReturnRepository
    {
        public class BulkCompetitionCommitteeBoatReturnRaceClass
        {
            public int IdCommitteeBoatReturn { get; set; }
            public int IdRaceClass { get; set; }

            public BulkCompetitionCommitteeBoatReturnRaceClass(int idCompetitionFile, int idRaceClass)
            {
                this.IdCommitteeBoatReturn = idCompetitionFile;
                this.IdRaceClass = idRaceClass;
            }
        }


        #region Private Members

        private readonly Dictionary<string, string> _columnsMapping = new()
        {
            { "Id", "[CommitteeBoatReturn].Id" },
            { "ReturnTime", "[CommitteeBoatReturn].ReturnTime" },
            { "Name", "[CommitteeBoatReturn].Name" },
            { "Competition.Id", "[Competition].Id" },
            { "Competition.Name", "[Competition].Name"},
            { "RaceClass.Id", "[RaceClass].Id" },
            { "RaceClass.Name", "[RaceClass].Name"}
        };

        private readonly ISqlBulkInsertHelper _sqlbulkInsertHelper;

        #endregion

        #region Constructors

        public CommitteeBoatReturnRepository
            (
                IContextResolver contextResolver,
                IQueryBuilder queryBuilder,
                ISqlBulkInsertHelper sqlbulkInsertHelper
            ) : base(contextResolver, queryBuilder)
        {
            _sqlbulkInsertHelper = sqlbulkInsertHelper;
        }

        #endregion

        #region IRaceCommitteeBoatReturnRepository implementation

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

        public PaginatedResult<CommitteeBoatReturn> Get(CommitteeBoatReturnSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            return this.GetCommitteeBoatReturns(searchFilter, paginationFilter, sorting, context);
        }

        public void Create(CommitteeBoatReturn raceCommitteeBoatReturn, ITransactionalContext? context = null)
        {
            this.CreateCommitteeBoatReturn(raceCommitteeBoatReturn, context);
        }

        public void AssociateRaceClasses(CommitteeBoatReturn raceCommitteeBoatReturn, ITransactionalContext? context = null)
        {
            if (raceCommitteeBoatReturn.RaceClasses == null || raceCommitteeBoatReturn.RaceClasses.Count == 0)
                return;

            var bulkItems = new List<BulkCompetitionCommitteeBoatReturnRaceClass>();

            raceCommitteeBoatReturn.RaceClasses.ForEach(x => { bulkItems.Add(new BulkCompetitionCommitteeBoatReturnRaceClass(raceCommitteeBoatReturn.Id, x.Id)); });

            var sqlBulkSettings = new SqlBulkSettings<BulkCompetitionCommitteeBoatReturnRaceClass>();
            sqlBulkSettings.TableName = "CommitteeBoatReturn_RaceClass";
            sqlBulkSettings.Mappings.Add(new SqlBulkColumnMapping(nameof(BulkCompetitionCommitteeBoatReturnRaceClass.IdCommitteeBoatReturn), "IdCommitteeBoatReturn"));
            sqlBulkSettings.Mappings.Add(new SqlBulkColumnMapping(nameof(BulkCompetitionCommitteeBoatReturnRaceClass.IdRaceClass), "IdRaceClass"));
            sqlBulkSettings.Data = bulkItems;

            _sqlbulkInsertHelper.PerformBulkInsert(sqlBulkSettings, context);
        }

        public int Delete(int id, ITransactionalContext? context = null)
        {
            return base.Delete("[CommitteeBoatReturn]", id, "Id", context);
        }

        public int DeleteRaceClasses(int id, ITransactionalContext? context = null)
        {
            return base.Delete("[CommitteeBoatReturn_RaceClass]", id, "IdCommitteeBoatReturn", context);
        }

        #endregion

        #region Private Methods

        private PaginatedResult<CommitteeBoatReturn> GetCommitteeBoatReturns(CommitteeBoatReturnSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            string sql = $@"SELECT
                                [CommitteeBoatReturn].Id [Id],
                                [CommitteeBoatReturn].ReturnTime [ReturnTime],
                                [CommitteeBoatReturn].Name [Name],
                                [Competition].Id [Id],
                                [Competition].Name [Name],
                                [RaceClass].Id [Id],
                                [RaceClass].Name [Name]
                            FROM [CommitteeBoatReturn] [CommitteeBoatReturn]
                            INNER JOIN [Competition] [Competition] ON [Competition].Id = [CommitteeBoatReturn].IdCompetition                            
                            LEFT JOIN [CommitteeBoatReturn_RaceClass] [CommitteeBoatReturn_RaceClass] ON [CommitteeBoatReturn_RaceClass].IdCommitteeBoatReturn = [CommitteeBoatReturn].Id
                            LEFT JOIN [RaceClass] [RaceClass] ON [RaceClass].Id = [CommitteeBoatReturn_RaceClass].IdRaceClass";

            QueryBuilder.AddCommand(sql);

            ProcessSearchFilter(searchFilter);

            QueryBuilder.AddSorting(sorting, _columnsMapping);
            QueryBuilder.AddPagination(paginationFilter);

            var committeeBoatReturns = new List<CommitteeBoatReturn>();

            PaginatedResult<CommitteeBoatReturn> items = base.GetPaginatedResults<CommitteeBoatReturn>
                (
                    (reader) =>
                    {
                        return reader.Read<CommitteeBoatReturn, Competition, RaceClass, CommitteeBoatReturn>
                        (
                            (committeeBoatReturn, competition, raceClass) =>
                            {
                                var existingCommitteeBoatReturn = committeeBoatReturns.FirstOrDefault(x => x.Id == committeeBoatReturn.Id);
                                if (existingCommitteeBoatReturn == null)
                                {
                                    committeeBoatReturns.Add(committeeBoatReturn);
                                    committeeBoatReturn.Competition = competition;
                                }
                                else
                                {
                                    committeeBoatReturn = existingCommitteeBoatReturn;
                                }
                                committeeBoatReturn.RaceClasses.Add(raceClass);

                                return committeeBoatReturn;
                            },
                            splitOn: "Id, Id, Id, Id"
                        ).AsList();
                    },
                    context
                );

            items.Results = committeeBoatReturns;

            return items;
        }

        private void ProcessSearchFilter(CommitteeBoatReturnSearchFilter? searchFilter = null)
        {
            if (searchFilter == null)
                return;

            base.AddFilterCriteria(ConditionType.In, "CommitteeBoatReturn", "Id", "ids", searchFilter.Ids);
            base.AddFilterCriteria(ConditionType.Equal, "Competition", "Id", "idCompetition", searchFilter.Competition?.Id);
            base.AddFilterCriteria(ConditionType.In, "RaceClass", "Id", "idRaceClass", searchFilter.RaceClasses?.Select(x => x.Id));
        }

        private void CreateCommitteeBoatReturn(CommitteeBoatReturn raceCommitteeBoatReturn, ITransactionalContext? context = null)
        {
            string sql = @" INSERT INTO [CommitteeBoatReturn]
                                ( IdCompetition, ReturnTime, [Name] )
                            VALUES
                                ( @idCompetition, @returnTime, @name )";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("idCompetition", raceCommitteeBoatReturn.Competition.Id);
            QueryBuilder.AddParameter("returnTime", raceCommitteeBoatReturn.ReturnTime);
            QueryBuilder.AddParameter("name", raceCommitteeBoatReturn.Name);

            QueryBuilder.AddReturnLastInsertedId();

            raceCommitteeBoatReturn.Id = base.Execute<int>(context);
        }

        #endregion
    }
}
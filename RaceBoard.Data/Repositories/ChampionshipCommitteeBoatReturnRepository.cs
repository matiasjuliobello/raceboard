using Dapper;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data.Helpers.Interfaces;
using RaceBoard.Data.Helpers.SqlBulkHelper;
using RaceBoard.Data.Repositories.Base.Abstract;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;

namespace RaceBoard.Data.Repositories
{
    public class ChampionshipCommitteeBoatReturnRepository : AbstractRepository, IChampionshipCommitteeBoatReturnRepository
    {
        public class BulkChampionshipCommitteeBoatReturnRaceClass
        {
            public int IdCommitteeBoatReturn { get; set; }
            public int IdRaceClass { get; set; }

            public BulkChampionshipCommitteeBoatReturnRaceClass(int idChampionshipFile, int idRaceClass)
            {
                this.IdCommitteeBoatReturn = idChampionshipFile;
                this.IdRaceClass = idRaceClass;
            }
        }


        #region Private Members

        private readonly Dictionary<string, string> _columnsMapping = new()
        {
            { "Id", "[CommitteeBoatReturn].Id" },
            { "ReturnTime", "[CommitteeBoatReturn].ReturnTime" },
            { "Name", "[CommitteeBoatReturn].Name" },
            { "Championship.Id", "[Championship].Id" },
            { "Championship.Name", "[Championship].Name"},
            { "RaceClass.Id", "[RaceClass].Id" },
            { "RaceClass.Name", "[RaceClass].Name"}
        };

        private readonly ISqlBulkInsertHelper _sqlbulkInsertHelper;

        #endregion

        #region Constructors

        public ChampionshipCommitteeBoatReturnRepository
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

        public PaginatedResult<ChampionshipBoatReturn> Get(ChampionshipBoatReturnSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            return this.GetCommitteeBoatReturns(searchFilter, paginationFilter, sorting, context);
        }

        public void Create(ChampionshipBoatReturn raceCommitteeBoatReturn, ITransactionalContext? context = null)
        {
            this.CreateCommitteeBoatReturn(raceCommitteeBoatReturn, context);
        }

        public void AssociateRaceClasses(ChampionshipBoatReturn raceCommitteeBoatReturn, ITransactionalContext? context = null)
        {
            if (raceCommitteeBoatReturn.RaceClasses == null || raceCommitteeBoatReturn.RaceClasses.Count == 0)
                return;

            var bulkItems = new List<BulkChampionshipCommitteeBoatReturnRaceClass>();

            raceCommitteeBoatReturn.RaceClasses.ForEach(x => { bulkItems.Add(new BulkChampionshipCommitteeBoatReturnRaceClass(raceCommitteeBoatReturn.Id, x.Id)); });

            var sqlBulkSettings = new SqlBulkSettings<BulkChampionshipCommitteeBoatReturnRaceClass>();
            sqlBulkSettings.TableName = "CommitteeBoatReturn_RaceClass";
            sqlBulkSettings.Mappings.Add(new SqlBulkColumnMapping(nameof(BulkChampionshipCommitteeBoatReturnRaceClass.IdCommitteeBoatReturn), "IdCommitteeBoatReturn"));
            sqlBulkSettings.Mappings.Add(new SqlBulkColumnMapping(nameof(BulkChampionshipCommitteeBoatReturnRaceClass.IdRaceClass), "IdRaceClass"));
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

        private PaginatedResult<ChampionshipBoatReturn> GetCommitteeBoatReturns(ChampionshipBoatReturnSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            string sql = $@"SELECT
                                [CommitteeBoatReturn].Id [Id],
                                [CommitteeBoatReturn].ReturnTime [ReturnTime],
                                [CommitteeBoatReturn].Name [Name],
                                [Championship].Id [Id],
                                [Championship].Name [Name],
                                [RaceClass].Id [Id],
                                [RaceClass].Name [Name]
                            FROM [CommitteeBoatReturn] [CommitteeBoatReturn]
                            INNER JOIN [Championship] [Championship] ON [Championship].Id = [CommitteeBoatReturn].IdChampionship                            
                            LEFT JOIN [CommitteeBoatReturn_RaceClass] [CommitteeBoatReturn_RaceClass] ON [CommitteeBoatReturn_RaceClass].IdCommitteeBoatReturn = [CommitteeBoatReturn].Id
                            LEFT JOIN [RaceClass] [RaceClass] ON [RaceClass].Id = [CommitteeBoatReturn_RaceClass].IdRaceClass";

            QueryBuilder.AddCommand(sql);

            ProcessSearchFilter(searchFilter);

            QueryBuilder.AddSorting(sorting, _columnsMapping);
            QueryBuilder.AddPagination(paginationFilter);

            var committeeBoatReturns = new List<ChampionshipBoatReturn>();

            PaginatedResult<ChampionshipBoatReturn> items = base.GetPaginatedResults<ChampionshipBoatReturn>
                (
                    (reader) =>
                    {
                        return reader.Read<ChampionshipBoatReturn, Championship, RaceClass, ChampionshipBoatReturn>
                        (
                            (committeeBoatReturn, championship, raceClass) =>
                            {
                                var existingCommitteeBoatReturn = committeeBoatReturns.FirstOrDefault(x => x.Id == committeeBoatReturn.Id);
                                if (existingCommitteeBoatReturn == null)
                                {
                                    committeeBoatReturns.Add(committeeBoatReturn);
                                    committeeBoatReturn.Championship = championship;
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

        private void ProcessSearchFilter(ChampionshipBoatReturnSearchFilter? searchFilter = null)
        {
            if (searchFilter == null)
                return;

            base.AddFilterCriteria(ConditionType.In, "CommitteeBoatReturn", "Id", "ids", searchFilter.Ids);
            base.AddFilterCriteria(ConditionType.Equal, "Championship", "Id", "idChampionship", searchFilter.Championship?.Id);
            base.AddFilterCriteria(ConditionType.In, "RaceClass", "Id", "idRaceClass", searchFilter.RaceClasses?.Select(x => x.Id));
        }

        private void CreateCommitteeBoatReturn(ChampionshipBoatReturn raceCommitteeBoatReturn, ITransactionalContext? context = null)
        {
            string sql = @" INSERT INTO [CommitteeBoatReturn]
                                ( IdChampionship, ReturnTime, [Name] )
                            VALUES
                                ( @idChampionship, @returnTime, @name )";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("idChampionship", raceCommitteeBoatReturn.Championship.Id);
            QueryBuilder.AddParameter("returnTime", raceCommitteeBoatReturn.ReturnTime);
            QueryBuilder.AddParameter("name", raceCommitteeBoatReturn.Name);

            QueryBuilder.AddReturnLastInsertedId();

            raceCommitteeBoatReturn.Id = base.Execute<int>(context);
        }

        #endregion
    }
}
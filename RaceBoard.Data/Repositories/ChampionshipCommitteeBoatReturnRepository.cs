﻿using Dapper;
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

        public PaginatedResult<ChampionshipCommitteeBoatReturn> Get(ChampionshipCommitteeBoatReturnSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            return this.GetCommitteeBoatReturns(searchFilter, paginationFilter, sorting, context);
        }

        public void Create(ChampionshipCommitteeBoatReturn raceCommitteeBoatReturn, ITransactionalContext? context = null)
        {
            this.CreateCommitteeBoatReturn(raceCommitteeBoatReturn, context);
        }

        public void Update(ChampionshipCommitteeBoatReturn raceCommitteeBoatReturn, ITransactionalContext? context = null)
        {
            this.UpdateCommitteeBoatReturn(raceCommitteeBoatReturn, context);
        }

        public void AssociateRaceClasses(ChampionshipCommitteeBoatReturn raceCommitteeBoatReturn, ITransactionalContext? context = null)
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

        private PaginatedResult<ChampionshipCommitteeBoatReturn> GetCommitteeBoatReturns(ChampionshipCommitteeBoatReturnSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
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

            var committeeBoatReturns = new List<ChampionshipCommitteeBoatReturn>();

            PaginatedResult<ChampionshipCommitteeBoatReturn> items = base.GetPaginatedResults<ChampionshipCommitteeBoatReturn>
                (
                    (reader) =>
                    {
                        return reader.Read<ChampionshipCommitteeBoatReturn, Championship, RaceClass, ChampionshipCommitteeBoatReturn>
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

        private void ProcessSearchFilter(ChampionshipCommitteeBoatReturnSearchFilter? searchFilter = null)
        {
            if (searchFilter == null)
                return;

            base.AddFilterCriteria(ConditionType.In, "CommitteeBoatReturn", "Id", "ids", searchFilter.Ids);
            base.AddFilterCriteria(ConditionType.Equal, "Championship", "Id", "idChampionship", searchFilter.Championship?.Id);
            base.AddFilterCriteria(ConditionType.In, "RaceClass", "Id", "idRaceClass", searchFilter.RaceClasses?.Select(x => x.Id));

            if (searchFilter.ReturnTime.HasValue)
            {
                //base.AddFilterGroup(LogicalOperator.And);
                var date = searchFilter.ReturnTime.Value.Date;
                var offset = searchFilter.ReturnTime.Value.Offset;

                var startDate = new DateTimeOffset(date.Year, date.Month, date.Day, 0, 0, 0, offset);
                var endDate = new DateTimeOffset(date.Year, date.Month, date.Day, 23, 59, 59, offset);

                base.AddFilterCriteria(ConditionType.GreaterOrEqualThan, "CommitteeBoatReturn", "ReturnTime", "startDate", startDate);
                base.AddFilterCriteria(ConditionType.LessOrEqualThan, "CommitteeBoatReturn", "ReturnTime", "endDate", endDate);
            }
            //base.AddFilterCriteria(ConditionType.Like, "Boat", "Name", "name", searchFilter.Boat?.Name, LogicalOperator.Or);
            //base.AddFilterCriteria(ConditionType.Like, "Boat", "SailNumber", "sailNumber", searchFilter.Boat?.SailNumber, LogicalOperator.Or);
        }

        private void CreateCommitteeBoatReturn(ChampionshipCommitteeBoatReturn championshipCommitteeBoatReturn, ITransactionalContext? context = null)
        {
            string sql = @" INSERT INTO [CommitteeBoatReturn]
                                ( IdChampionship, ReturnTime, [Name] )
                            VALUES
                                ( @idChampionship, @returnTime, @name )";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("idChampionship", championshipCommitteeBoatReturn.Championship.Id);
            QueryBuilder.AddParameter("returnTime", championshipCommitteeBoatReturn.ReturnTime);
            QueryBuilder.AddParameter("name", championshipCommitteeBoatReturn.Name);

            QueryBuilder.AddReturnLastInsertedId();

            championshipCommitteeBoatReturn.Id = base.Execute<int>(context);
        }

        private void UpdateCommitteeBoatReturn(ChampionshipCommitteeBoatReturn championshipCommitteeBoatReturn, ITransactionalContext? context = null)
        {
            string sql = @" UPDATE [CommitteeBoatReturn] SET
                                Name = @name, ReturnTime = @returnTime";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("idChampionship", championshipCommitteeBoatReturn.Championship.Id);
            QueryBuilder.AddParameter("returnTime", championshipCommitteeBoatReturn.ReturnTime);
            QueryBuilder.AddParameter("name", championshipCommitteeBoatReturn.Name);

            QueryBuilder.AddParameter("id", championshipCommitteeBoatReturn.Id);
            QueryBuilder.AddCondition("Id = @id");

            base.ExecuteAndGetRowsAffected(context);
        }

        #endregion
    }
}
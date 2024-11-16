using Dapper;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data.Helpers.Interfaces;
using RaceBoard.Data.Repositories.Base.Abstract;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;

namespace RaceBoard.Data.Repositories
{
    public class CommitteeBoatReturnRepository : AbstractRepository, ICommitteeBoatReturnRepository
    {
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

        #endregion

        #region Constructors

        public CommitteeBoatReturnRepository(IContextResolver contextResolver, IQueryBuilder queryBuilder) : base(contextResolver, queryBuilder)
        {
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
            this.AddRaceClassesToCommitteeBoatReturn(raceCommitteeBoatReturn.Id, raceCommitteeBoatReturn.RaceClasses, context);
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
                            INNER JOIN [CommitteeBoatReturn_RaceClass] [CommitteeBoatReturn_RaceClass] ON [CommitteeBoatReturn_RaceClass].IdCommitteeBoatReturn = [CommitteeBoatReturn].Id
                            INNER JOIN [RaceClass] [RaceClass] ON [RaceClass].Id = [CommitteeBoatReturn_RaceClass].IdRaceClass";

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

        private void AddRaceClassesToCommitteeBoatReturn(int idCommitteeBoatReturn, List<RaceClass> raceClasses, ITransactionalContext? context = null)
        {
            if (raceClasses == null)
                return;

            QueryBuilder.Clear();

            foreach (var raceClass in raceClasses)
            {
                string sqlRaceClass = @"INSERT INTO [CommitteeBoatReturn_RaceClass]
                                        ( IdCommitteeBoatReturn, IdRaceClass )
                                    VALUES
                                        ( @idCommitteeBoatReturn, @idRaceClass )";

                QueryBuilder.AddCommand(sqlRaceClass);

                QueryBuilder.AddParameter("idCommitteeBoatReturn", idCommitteeBoatReturn);
                QueryBuilder.AddParameter("idRaceClass", raceClass.Id);

                QueryBuilder.AddReturnLastInsertedId();

                int id = base.Execute<int>(context);
            }
        }

        #endregion
    }
}
using Dapper;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data.Helpers.Interfaces;
using RaceBoard.Data.Repositories.Base.Abstract;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;

namespace RaceBoard.Data.Repositories
{
    public class RaceRepository : AbstractRepository, IRaceRepository
    {
        #region Private Members

        private readonly Dictionary<string, string> _columnsMapping = new()
        {
            { "Id", "[Race].Id" },
            { "Schedule", "[Race].Schedule" },
            { "RaceClass.Id", "[RaceClass].Id" },
            { "RaceClass.Name", "[RaceClass].Name"},
            { "Competition.Id", "[Competition].Id" },
            { "Competition.Name", "[Competition].Name"},
            { "Competition.StartDate", "[Competition].StartDate"},
            { "Competition.EndDate", "[Competition].EndDate"}
        };

        #endregion

        #region Constructors

        public RaceRepository(IContextResolver contextResolver, IQueryBuilder queryBuilder) : base(contextResolver, queryBuilder)
        {
        }

        #endregion

        #region IRaceRepository implementation

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

        public PaginatedResult<Race> Get(RaceSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            return this.GetRaces(searchFilter: searchFilter, paginationFilter: paginationFilter, sorting: sorting, context: context);
        }

        public Race? Get(int id, ITransactionalContext? context = null)
        {
            var searchFilter = new RaceSearchFilter() { Ids = new int[] { id } };

            return this.GetRaces(searchFilter: searchFilter, paginationFilter: null, sorting: null, context: context).Results.FirstOrDefault();
        }

        public void Create(Race race, ITransactionalContext? context = null)
        {
            this.CreateRace(race, context);
        }

        public void Update(Race race, ITransactionalContext? context = null)
        {
            this.UpdateRace(race, context);
        }

        public int Delete(int id, ITransactionalContext? context = null)
        {
            return base.Delete("[Race]", id, "Id", context);
        }

        #endregion

        #region Private Methods

        private PaginatedResult<Race> GetRaces(RaceSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            string sql = $@"SELECT
                                [Race].Id [Id],
                                [Race].Schedule [Schedule],
                                [RaceClass].Id [Id],
                                [RaceClass].Name [Name],
                                [Competition].Id [Id],
                                [Competition].Name [Name],
                                [Competition].StartDate [StartDate],
                                [Competition].EndDate [EndDate]
                            FROM [Race] [Race]
                            INNER JOIN [RaceClass] [RaceClass] ON [RaceClass].Id = [Race].IdRaceClass
                            INNER JOIN [Competition] [Competition] ON [Competition].Id = [Race].IdCompetition";

            QueryBuilder.AddCommand(sql);

            ProcessSearchFilter(searchFilter);

            QueryBuilder.AddSorting(sorting, _columnsMapping);
            QueryBuilder.AddPagination(paginationFilter);

            var races = new List<Race>();

            PaginatedResult<Race> items = base.GetPaginatedResults<Race>
                (
                    (reader) =>
                    {
                        return reader.Read<Race, RaceClass, Competition, Race>
                        (
                            (race, raceClass, competition) =>
                            {
                                race.RaceClass = raceClass;
                                race.Competition = competition;

                                races.Add(race);

                                return race;
                            },
                            splitOn: "Id, Id, Id"
                        ).AsList();
                    },
                    context
                );

            items.Results = races;

            return items;
        }

        private void ProcessSearchFilter(RaceSearchFilter? searchFilter = null)
        {
            if (searchFilter == null)
                return;

            base.AddFilterCriteria(ConditionType.In, "Race", "Id", "ids", searchFilter.Ids);
            base.AddFilterCriteria(ConditionType.Equal, "Competition", "Id", "idCompetition", searchFilter.Competition?.Id);
            base.AddFilterCriteria(ConditionType.Equal, "RaceClass", "Id", "idRaceClass", searchFilter.RaceClass?.Id);
        }

        private void CreateRace(Race race, ITransactionalContext? context = null)
        {
            string sql = @" INSERT INTO [Race]
                                ( IdRaceClass, IdCompetition, Schedule )
                            VALUES
                                ( @idRaceClass, @idCompetition, @schedule )";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("idRaceClass", race.RaceClass.Id);
            QueryBuilder.AddParameter("idCompetition", race.Competition.Id);
            QueryBuilder.AddParameter("schedule", race.Schedule);

            QueryBuilder.AddReturnLastInsertedId();

            race.Id = base.Execute<int>(context);
        }

        private void UpdateRace(Race race, ITransactionalContext? context = null)
        {
            string sql = @" UPDATE [Race] SET
                                IdRaceClass = @idRaceClass,
                                IdCompetition = @idCompetition
                                Schedule = @schedule";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("idRaceClass", race.RaceClass.Id);
            QueryBuilder.AddParameter("idCompetition", race.Competition.Id);
            QueryBuilder.AddParameter("schedule", race.Schedule);

            QueryBuilder.AddParameter("id", race.Id);
            QueryBuilder.AddCondition("Id = @id");

            base.ExecuteAndGetRowsAffected(context);
        }

        #endregion
    }
}
using Dapper;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data.Helpers.Interfaces;
using RaceBoard.Data.Repositories.Base.Abstract;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;
using System.Diagnostics;
using System;

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
            { "Championship.Id", "[Championship].Id" },
            { "Championship.Name", "[Championship].Name"},
            { "Championship.StartDate", "[Championship].StartDate"},
            { "Championship.EndDate", "[Championship].EndDate"}
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
                                [Championship].Id [Id],
                                [Championship].Name [Name],
                                [Championship].StartDate [StartDate],
                                [Championship].EndDate [EndDate]
                            FROM [Race] [Race]
                            INNER JOIN [Championship] [Championship] ON [Championship].Id = [Race].IdChampionship
                            LEFT JOIN [Race_RaceClass] [Race_RaceClass] ON [Race_RaceClass].IdRace = [Race].Id
                            LEFT JOIN [RaceClass] [RaceClass] ON [RaceClass].Id = [Race_RaceClass].IdRaceClass";

            QueryBuilder.AddCommand(sql);

            ProcessSearchFilter(searchFilter);

            QueryBuilder.AddSorting(sorting, _columnsMapping);
            QueryBuilder.AddPagination(paginationFilter);

            var races = new List<Race>();

            PaginatedResult<Race> items = base.GetPaginatedResults<Race>
                (
                    (reader) =>
                    {
                        return reader.Read<Race, RaceClass, Championship, Race>
                        (
                            (race, raceClass, championship) =>
                            {
                                var existingRace = races.FirstOrDefault(x => x.Id == race.Id);
                                if (existingRace == null)
                                {
                                    races.Add(race);
                                    race.Championship = championship;
                                }
                                else
                                {
                                    race = existingRace;
                                }
                                race.RaceClasses.Add(raceClass);

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
            base.AddFilterCriteria(ConditionType.Equal, "Championship", "Id", "idChampionship", searchFilter.Championship?.Id);
            base.AddFilterCriteria(ConditionType.Equal, "RaceClass", "Id", "idRaceClass", searchFilter.RaceClass?.Id);
        }

        private void CreateRace(Race race, ITransactionalContext? context = null)
        {
            QueryBuilder.Clear();

            string sql = @" INSERT INTO [Race]
                                ( IdChampionship, Schedule )
                            VALUES
                                ( @idChampionship, @schedule )";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("idChampionship", race.Championship.Id);
            QueryBuilder.AddParameter("schedule", race.Schedule);

            QueryBuilder.AddReturnLastInsertedId();

            race.Id = base.Execute<int>(context);

            int removedRaceClasses = this.RemoveRaceClassesFromRace(race.Id, context);
            this.AddRaceClassesToRace(race.Id, race.RaceClasses, context);
        }

        private void UpdateRace(Race race, ITransactionalContext? context = null)
        {
            QueryBuilder.Clear();

            string sql = @" UPDATE [Race] SET
                                IdChampionship = @idChampionship
                                Schedule = @schedule";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("idChampionship", race.Championship.Id);
            QueryBuilder.AddParameter("schedule", race.Schedule);

            QueryBuilder.AddParameter("id", race.Id);
            QueryBuilder.AddCondition("Id = @id");

            base.ExecuteAndGetRowsAffected(context);

            int removedRaceClasses = this.RemoveRaceClassesFromRace(race.Id, context);
            this.AddRaceClassesToRace(race.Id, race.RaceClasses, context);
        }

        private int RemoveRaceClassesFromRace(int idRace, ITransactionalContext? context = null)
        {
            return base.Delete("[Race_RaceClass]", idRace, "IdRace", context);
        }

        private void AddRaceClassesToRace(int idRace, List<RaceClass> raceClasses, ITransactionalContext? context = null)
        {
            if (raceClasses == null)
                return;

            QueryBuilder.Clear();

            foreach (var raceClass in raceClasses)
            {
                string sqlRaceClass = @"INSERT INTO [Race_RaceClass]
                                        ( IdRace, IdRaceClass )
                                    VALUES
                                        ( @idRace, @idRaceClass )";

                QueryBuilder.AddCommand(sqlRaceClass);

                QueryBuilder.AddParameter("idRace", idRace);
                QueryBuilder.AddParameter("idRaceClass", raceClass.Id);

                QueryBuilder.AddReturnLastInsertedId();

                int idRace_RaceClass = base.Execute<int>(context);
            }
        }

        #endregion
    }
}
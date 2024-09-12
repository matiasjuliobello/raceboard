using Dapper;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data.Helpers.Interfaces;
using RaceBoard.Data.Repositories.Base.Abstract;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;

namespace RaceBoard.Data.Repositories
{
    public class TeamRepository : AbstractRepository, ITeamRepository
    {
        #region Private Members

        private readonly Dictionary<string, string> _columnsMapping = new()
        {
            { "Id", "[Team].Id" },
            { "Name", "[Team].Name" },
            { "RaceClass.Id", "[RaceClass].Id" },
            { "RaceClass.Name", "[RaceClass].Name"},
            { "Competition.Id", "[Competition].Id" },
            { "Competition.Name", "[Competition].Name"},
            { "Competition.StartDate", "[Competition].StartDate"},
            { "Competition.EndDate", "[Competition].EndDate"}
        };

        #endregion

        #region Constructors

        public TeamRepository(IContextResolver contextResolver, IQueryBuilder queryBuilder) : base(contextResolver, queryBuilder)
        {
        }

        #endregion

        #region ITeamRepository implementation

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
            return base.Exists(id, "Team", "Id", context);
        }

        public bool ExistsDuplicate(Team team, ITransactionalContext? context = null)
        {
            string condition = "[Name] = @name AND [IdCompetition] = @idCompetition AND [IdRaceClass] = @idRaceClass";

            string existsQuery = base.GetExistsDuplicateQuery("[Team]", condition, "Id", "@id");

            QueryBuilder.AddCommand(existsQuery);
            QueryBuilder.AddParameter("name", team.Name);
            QueryBuilder.AddParameter("idCompetition", team.Competition.Id);
            QueryBuilder.AddParameter("idRaceClass", team.RaceClass.Id);
            QueryBuilder.AddParameter("id", team.Id);

            return base.Execute<bool>(context);
        }

        public PaginatedResult<Team> Get(TeamSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            return this.GetTeams(searchFilter: searchFilter, paginationFilter: paginationFilter, sorting: sorting, context: context);
        }
        public Team? Get(int id, ITransactionalContext? context = null)
        {
            var searchFilter = new TeamSearchFilter() { Ids = new int[] { id } };

            return this.GetTeams(searchFilter: searchFilter, paginationFilter: null, sorting: null, context: context).Results.FirstOrDefault();
        }

        public void Create(Team team, ITransactionalContext? context = null)
        {
            this.CreateTeam(team, context);
        }

        public void Update(Team team, ITransactionalContext? context = null)
        {
            this.UpdateTeam(team, context);
        }

        public int Delete(Team team, ITransactionalContext? context = null)
        {
            return this.DeleteTeam(team, context);
        }

        #endregion

        #region Private Methods

        private PaginatedResult<Team> GetTeams(TeamSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            string sql = $@"SELECT
                                [Team].Id [Id],
                                [Team].Name [Name],
                                [RaceClass].Id [Id],
                                [RaceClass].Name [Name],
                                [Competition].Id [Id],
                                [Competition].Name [Name],
                                [Competition].StartDate [StartDate],
                                [Competition].EndDate [EndDate]
                            FROM [Team] [Team]
                            INNER JOIN [Competition] [Competition] ON [Competition].Id = [Team].IdCompetition
                            INNER JOIN [RaceClass] [RaceClass] ON [RaceClass].Id = [Team].IdRaceClass";

            QueryBuilder.AddCommand(sql);

            ProcessSearchFilter(searchFilter);

            QueryBuilder.AddSorting(sorting, _columnsMapping);
            QueryBuilder.AddPagination(paginationFilter);

            var teams = new List<Team>();

            PaginatedResult<Team> items = base.GetPaginatedResults<Team>
                (
                    (reader) =>
                    {
                        return reader.Read<Team, RaceClass, Competition, Team>
                        (
                            (team, raceClass, competition) =>
                            {
                                team.RaceClass = raceClass;
                                team.Competition = competition;

                                teams.Add(team);

                                return team;
                            },
                            splitOn: "Id, Id, Id"
                        ).AsList();
                    },
                    context
                );

            items.Results = teams;

            return items;
        }

        private void ProcessSearchFilter(TeamSearchFilter? searchFilter = null)
        {
            if (searchFilter == null)
                return;

            base.AddFilterCriteria(ConditionType.In, "Team", "Id", "ids", searchFilter.Ids);
            base.AddFilterCriteria(ConditionType.Like, "Team", "Name", "name", searchFilter.Name);
            base.AddFilterCriteria(ConditionType.Equal, "Competition", "Id", "idCompetition", searchFilter.Competition?.Id);
            base.AddFilterCriteria(ConditionType.Equal, "RaceClass", "Id", "idRaceClass", searchFilter.RaceClass?.Id);
        }

        private void CreateTeam(Team team, ITransactionalContext? context = null)
        {
            string sql = @" INSERT INTO [Team]
                                ( Name, IdCompetition, IdRaceClass )
                            VALUES
                                ( @name, @idCompetition, @idRaceClass )";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("name", team.Name);
            QueryBuilder.AddParameter("idCompetition", team.Competition.Id);
            QueryBuilder.AddParameter("idRaceClass", team.RaceClass.Id);

            QueryBuilder.AddReturnLastInsertedId();

            team.Id = base.Execute<int>(context);
        }

        private void UpdateTeam(Team team, ITransactionalContext? context = null)
        {
            string sql = @" UPDATE [Team] SET
                                Name = @name,
                                IdCompetition = @idCompetition,
                                IdRaceClass = @idRaceClass";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("name", team.Name);
            QueryBuilder.AddParameter("idCompetition", team.Competition.Id);
            QueryBuilder.AddParameter("idRaceClass", team.RaceClass.Id);

            QueryBuilder.AddParameter("id", team.Id);
            QueryBuilder.AddCondition("Id = @id");

            base.ExecuteAndGetRowsAffected(context);
        }

        private int DeleteTeam(Team team, ITransactionalContext? context = null)
        {
            return base.Delete("[Team]", team.Id, "Id", context);
        }

        #endregion
    }
}
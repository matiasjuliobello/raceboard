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
            { "Organization.Id", "[Organization].Id" },
            { "Organization.Name", "[Organization].Name" },
            { "RaceClass.Id", "[RaceClass].Id" },
            { "RaceClass.Name", "[RaceClass].Name"},
            { "Championship.Id", "[Championship].Id" },
            { "Championship.Name", "[Championship].Name"},
            { "Championship.StartDate", "[Championship].StartDate"},
            { "Championship.EndDate", "[Championship].EndDate"}
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
            //string condition = "[IdChampionship] = @idChampionship AND [IdRaceClass] = @idRaceClass";

            //string existsQuery = base.GetExistsDuplicateQuery("[Team]", condition, "Id", "@id");

            //QueryBuilder.AddCommand(existsQuery);
            //QueryBuilder.AddParameter("idChampionship", team.Championship.Id);
            //QueryBuilder.AddParameter("idRaceClass", team.RaceClass.Id);
            //QueryBuilder.AddParameter("id", team.Id);

            //return base.Execute<bool>(context);

            return false;
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
                                [Organization].Id [Id],
                                [Organization].Name [Name],
                                [RaceClass].Id [Id],
                                [RaceClass].Name [Name],
                                [Championship].Id [Id],
                                [Championship].Name [Name],
                                [Championship].StartDate [StartDate],
                                [Championship].EndDate [EndDate],
	                            [ChampionshipOrganization].Id [Id],
	                            [ChampionshipOrganization].Name [Name],
                                [Team_Member].Id [Id]
                            FROM [Team] [Team]
                            INNER JOIN [Organization] [Organization] ON [Organization].Id = [Team].IdOrganization
                            INNER JOIN [Championship] [Championship] ON [Championship].Id = [Team].IdChampionship
                            INNER JOIN [Championship_Organization] [Championship_Organization] ON [Championship_Organization].IdChampionship = [Championship].Id
                            INNER JOIN [Organization] [ChampionshipOrganization] ON [ChampionshipOrganization].Id = [Championship_Organization].IdOrganization
                            INNER JOIN [RaceClass] [RaceClass] ON [RaceClass].Id = [Team].IdRaceClass
                            LEFT JOIN [Team_Member] ON [Team_Member].IdTeam = [Team].Id";

            QueryBuilder.AddCommand(sql);

            ProcessSearchFilter(searchFilter);

            QueryBuilder.AddSorting(sorting, _columnsMapping);
            QueryBuilder.AddPagination(paginationFilter);

            var teams = new List<Team>();

            PaginatedResult<Team> items = base.GetPaginatedResults<Team>
                (
                    (reader) =>
                    {
                        return reader.Read<Team, Organization, RaceClass, Championship, Organization, TeamMember, Team>
                        (
                            (team, organization, raceClass, championship, championshipOrganization, member) =>
                            {
                                var t = teams.FirstOrDefault(x => x.Id == team.Id);
                                if (t == null)
                                    teams.Add(team);
                                else
                                    team = t;

                                team.Organization = organization;
                                team.RaceClass = raceClass;
                                team.Championship = championship;

                                team.Championship.Organizations.Add(championshipOrganization);

                                if (member != null)
                                    team.Members.Add(member);

                                return team;
                            },
                            splitOn: "Id, Id, Id, Id, Id, Id"
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
            base.AddFilterCriteria(ConditionType.Equal, "Organization", "Id", "idOrganization", searchFilter.Organization?.Id); 
            base.AddFilterCriteria(ConditionType.Equal, "Championship", "Id", "idChampionship", searchFilter.Championship?.Id);
            base.AddFilterCriteria(ConditionType.Equal, "RaceClass", "Id", "idRaceClass", searchFilter.RaceClass?.Id);
        }

        private void CreateTeam(Team team, ITransactionalContext? context = null)
        {
            string sql = @" INSERT INTO [Team]
                                ( IdOrganization, IdChampionship, IdRaceClass )
                            VALUES
                                ( @idOrganization, @idChampionship, @idRaceClass )";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("idOrganization", team.Organization.Id); 
            QueryBuilder.AddParameter("idChampionship", team.Championship.Id);
            QueryBuilder.AddParameter("idRaceClass", team.RaceClass.Id);

            QueryBuilder.AddReturnLastInsertedId();

            team.Id = base.Execute<int>(context);
        }

        private void UpdateTeam(Team team, ITransactionalContext? context = null)
        {
            string sql = @" UPDATE [Team] SET
                                IdChampionship = @idChampionship,
                                IdRaceClass = @idRaceClass";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("idChampionship", team.Championship.Id);
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
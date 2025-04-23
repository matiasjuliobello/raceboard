using Dapper;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data.Helpers.Interfaces;
using RaceBoard.Data.Repositories.Base.Abstract;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;
using static RaceBoard.Data.Helpers.SqlQueryBuilder;

namespace RaceBoard.Data.Repositories
{
    public class TeamBoatRepository : AbstractRepository, ITeamBoatRepository
    {
        #region Private Members

        private readonly Dictionary<string, string> _columnsMapping = new()
        {
            { "Id", "[Team_Boat].Id" },
            { "Team.Id", "[Team].Id" },            
            { "Boat.Id", "[Boat].Id" },
            { "Boat.Name", "[Boat].Name"},
            { "Boat.SailNumber", "[Boat].SailNumber"},
            { "Boat.HullNumber", "[Boat].HullNumber"},
            { "Boat.RaceClass.Id", "[RaceClass].Id" },
            { "Boat.RaceClass.Name", "[RaceClass].Name"},
            { "Boat.RaceClass.RaceCategory.Id", "[RaceCategory].Id"},
            { "Boat.RaceClass.RaceCategory.Name", "[RaceCategory].Name"}
        };

        #endregion

        #region Constructors

        public TeamBoatRepository(IContextResolver contextResolver, IQueryBuilder queryBuilder) : base(contextResolver, queryBuilder)
        {
        }

        #endregion

        #region ITeamBoatRepository implementation

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
            return base.Exists(id, "Team_Boat", "Id", context);
        }

        public bool ExistsDuplicate(TeamBoat teamBoat, ITransactionalContext? context = null)
        {
            string query = @"SELECT
                             IIF
                             (
                              EXISTS
                               (

                                SELECT 1 FROM 
                                (
                                 SELECT [Team_Boat].Id FROM [Team_Boat] [Team_Boat]
                                 INNER JOIN [Team] [Team] ON [Team].Id = [Team_Boat].IdTeam
                                 INNER JOIN [Championship] [Championship] ON [Championship].Id = [Team].IdChampionship
                                 WHERE
                                  [Team_Boat].IdBoat = @idBoat 
                                 AND [Team].IdChampionship = (SELECT IdChampionship FROM [Team] WHERE Id = @idTeam )
                                 AND IIF([Team_Boat].Id = @idTeamBoat, 0, 1) = 1
                                ) [x]
                               ), 1, 0)";

            QueryBuilder.AddCommand(query);
            QueryBuilder.AddParameter("idTeamBoat", teamBoat.Id);
            QueryBuilder.AddParameter("idBoat", teamBoat.Boat.Id);
            QueryBuilder.AddParameter("idTeam", teamBoat.Team.Id);

            return base.Execute<bool>(context);
        }

        //public PaginatedResult<TeamBoat> SearchTeamBoats(TeamBoatSearchFilter searchFilter, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        //{
        //    return this.SearchBoats(searchFilter, paginationFilter, sorting, context);
        //}

        public PaginatedResult<TeamBoat> Get(TeamBoatSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            return this.GetTeamBoats(searchFilter: searchFilter, paginationFilter: paginationFilter, sorting: sorting, context: context);
        }

        public TeamBoat? Get(int id, ITransactionalContext? context = null)
        {
            var searchFilter = new TeamBoatSearchFilter() { Ids = new int[] { id } };

            return this.GetTeamBoats(searchFilter: searchFilter, paginationFilter: null, sorting: null, context: context).Results.FirstOrDefault();
        }

        public void Create(TeamBoat teamBoat, ITransactionalContext? context = null)
        {
            this.CreateTeamBoat(teamBoat, context);
        }

        public void Update(TeamBoat teamBoat, ITransactionalContext? context = null)
        {
            this.UpdateTeamBoat(teamBoat, context);
        }

        public void Delete(TeamBoat teamBoat, ITransactionalContext? context = null)
        {
            this.DeleteTeamBoat(teamBoat, context);
        }

        #endregion

        #region Private Methods

        //private PaginatedResult<TeamBoat> SearchBoats(TeamBoatSearchFilter searchFilter, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        //{
        //    string sql = $@"SELECT
        //                        [Team_Boat].Id [Id],
        //                        [Team].Id [Id],
        //                        [Boat].Id [Id],
        //                        [Boat].Name [Name],
        //                        [Boat].SailNumber [SailNumber],
        //                        [RaceClass].Id [Id],
        //                        [RaceClass].Name [Name],
        //                        [RaceCategory].Id [Id],
        //                        [RaceCategory].Name [Name]
        //                    FROM [Team_Boat] [Team_Boat]
        //                    INNER JOIN [Team] [Team] ON [Team].Id = [Team_Boat].IdTeam
        //                    INNER JOIN [Boat] [Boat] ON [Boat].Id = [Team_Boat].IdBoat
        //                    INNER JOIN [RaceClass] [RaceClass] ON [RaceClass].Id = [Boat].IdRaceClass
        //                    INNER JOIN [RaceCategory] [RaceCategory] ON [RaceCategory].Id = [RaceClass].IdRaceCategory";

        //    QueryBuilder.AddCommand(sql);

        //    QueryBuilder.AddCondition("[Boat].Name LIKE '%' + @searchTerm + '%'", LogicalOperator.Or);
        //    QueryBuilder.AddCondition("[Boat].SailNumber LIKE '%' + @searchTerm + '%'", LogicalOperator.Or);
        //    QueryBuilder.AddParameter("searchTerm", searchTerm);

        //    QueryBuilder.AddSorting(sorting, _columnsMapping);
        //    QueryBuilder.AddPagination(paginationFilter);

        //    var teamBoats = new List<TeamBoat>();

        //    PaginatedResult<TeamBoat> items = base.GetPaginatedResults<TeamBoat>
        //        (
        //            (reader) =>
        //            {
        //                return reader.Read<TeamBoat, Team, Boat, RaceClass, RaceCategory, TeamBoat>
        //                (
        //                    (teamBoat, team, boat, raceClass, raceCategory) =>
        //                    {
        //                        raceClass.RaceCategory = raceCategory;
        //                        team.RaceClass = raceClass;
        //                        boat.RaceClass = raceClass;

        //                        teamBoat.Team = team;
        //                        teamBoat.Boat = boat;

        //                        teamBoats.Add(teamBoat);

        //                        return teamBoat;
        //                    },
        //                    splitOn: "Id, Id, Id, Id, Id"
        //                ).AsList();
        //            },
        //            context
        //        );

        //    items.Results = teamBoats;

        //    return items;
        //}

        private PaginatedResult<TeamBoat> GetTeamBoats(TeamBoatSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            string sql = $@"SELECT
                                [Team_Boat].Id [Id],
                                [Team].Id [Id],
                                [Boat].Id [Id],
                                [Boat].Name [Name],
                                [Boat].SailNumber [SailNumber],
                                [Boat].HullNumber [HullNumber],
                                [RaceClass].Id [Id],
                                [RaceClass].Name [Name],
                                [RaceCategory].Id [Id],
                                [RaceCategory].Name [Name],
                                [Championship].Id [Id],
	                            [Championship].Name [Name]
                            FROM [Team_Boat] [Team_Boat]
                            INNER JOIN [Team] [Team] ON [Team].Id = [Team_Boat].IdTeam
                            INNER JOIN [Boat] [Boat] ON [Boat].Id = [Team_Boat].IdBoat
                            INNER JOIN [RaceClass] [RaceClass] ON [RaceClass].Id = [Boat].IdRaceClass
                            INNER JOIN [RaceCategory] [RaceCategory] ON [RaceCategory].Id = [RaceClass].IdRaceCategory
                            INNER JOIN [Championship] [Championship] ON [Championship].Id = [Team].IdChampionship";

            QueryBuilder.AddCommand(sql);

            ProcessSearchFilter(searchFilter);

            QueryBuilder.AddSorting(sorting, _columnsMapping);
            QueryBuilder.AddPagination(paginationFilter);

            var teamBoats = new List<TeamBoat>();

            PaginatedResult<TeamBoat> items = base.GetPaginatedResults<TeamBoat>
                (
                    (reader) =>
                    {
                        return reader.Read<TeamBoat, Team, Boat, RaceClass, RaceCategory, Championship, TeamBoat>
                        (
                            (teamBoat, team, boat, raceClass, raceCategory, championship) =>
                            {
                                raceClass.RaceCategory = raceCategory;
                                boat.RaceClass = raceClass;
                                team.RaceClass = raceClass;
                                
                                team.Championship = championship;

                                teamBoat.Team = team;
                                teamBoat.Boat = boat;

                                teamBoats.Add(teamBoat);

                                return teamBoat;
                            },
                            splitOn: "Id, Id, Id, Id, Id, Id"
                        ).AsList();
                    },
                    context
                );

            items.Results = teamBoats;

            return items;
        }

        private void ProcessSearchFilter(TeamBoatSearchFilter? searchFilter)
        {
            if (searchFilter == null)
                return;

            base.AddFilterGroup(LogicalOperator.And);
            base.AddFilterCriteria(ConditionType.In, "Team_Boat", "Id", "ids", searchFilter.Ids);
            base.AddFilterCriteria(ConditionType.Equal, "Team", "Id", "idTeam", searchFilter.Team?.Id);
            base.AddFilterCriteria(ConditionType.Equal, "Championship", "Id", "idChampionship", searchFilter.Championship?.Id);
            base.AddFilterCriteria(ConditionType.Equal, "RaceClass", "Id", "idRaceClass", searchFilter.RaceClass?.Id);

            base.AddFilterGroup(LogicalOperator.And);
            base.AddFilterCriteria(ConditionType.Like, "Boat", "Name", "name", searchFilter.Boat?.Name, LogicalOperator.Or);
            base.AddFilterCriteria(ConditionType.Like, "Boat", "SailNumber", "sailNumber", searchFilter.Boat?.SailNumber, LogicalOperator.Or);
            base.AddFilterCriteria(ConditionType.Like, "Boat", "HullNumber", "hullNumber", searchFilter.Boat?.HullNumber, LogicalOperator.Or);
        }

        private void CreateTeamBoat(TeamBoat teamBoat, ITransactionalContext? context = null)
        {
            string sql = @" INSERT INTO [Team_Boat]
                            ( IdTeam, IdBoat )
                        VALUES
                            ( @idTeam, @idBoat )";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("idTeam", teamBoat.Team.Id);
            QueryBuilder.AddParameter("idBoat", teamBoat.Boat.Id);

            QueryBuilder.AddReturnLastInsertedId();

            base.Execute<int>(context);
        }

        private void UpdateTeamBoat(TeamBoat teamBoat, ITransactionalContext? context = null)
        {
            string sql = @" UPDATE [Team_Boat] SET
                                IdBoat = @idBoat";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("idBoat", teamBoat.Boat.Id);
            QueryBuilder.AddParameter("id", teamBoat.Id);
            QueryBuilder.AddCondition("Id = @id");

            base.ExecuteAndGetRowsAffected(context);
        }

        private int DeleteTeamBoat(TeamBoat teamBoat, ITransactionalContext? context = null)
        {
            return base.Delete("[Team_Boat]", teamBoat.Id, "Id", context);
        }

        #endregion
    }
}
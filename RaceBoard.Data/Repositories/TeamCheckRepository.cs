using Dapper;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data.Helpers.Interfaces;
using RaceBoard.Data.Repositories.Base.Abstract;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;
using RaceBoard.Domain._Enums;
using System.Text;

namespace RaceBoard.Data.Repositories
{
    public class TeamCheckRepository : AbstractRepository, ITeamCheckRepository
    {
        #region Private Members

        private readonly Dictionary<string, string> _columnsMapping = new()
        {
            //[TeamContestant_Check].Id[Id],
            //[TeamContestant_Check].CheckTime[CheckTime],
            //[CheckType].Id[Id],
            //[CheckType].Name[CheckType],
            //[Team_Contestant].Id[Id],
            //[ContestantRole].[Id],
            //[Team_Contestant].IdPerson[Id],
            //[Team].Id[Id],
            //[Team].Name[Team Name],
            //[Person].Id[Id],
            //[Person].Firstname[Firstname],
            //[Person].Lastname[Lastname]

            { "Id", "[Team_Check].Id" },
            { "Team.Id", "[Team].Id" },
            { "Team.Name", "[Team].Name" },
            { "Check.Id", "[Check].Id" },
            { "Check.Name", "[Check].Name"},
            { "Check.SailNumber", "[Check].SailNumber"},
            { "Check.RaceClass.Id", "[RaceClass].Id" },
            { "Check.RaceClass.Name", "[RaceClass].Name"},
            { "Check.RaceClass.RaceCategory.Id", "[RaceCategory].Id"},
            { "Check.RaceClass.RaceCategory.Name", "[RaceCategory].Name"}
        };

        #endregion

        #region Constructors

        public TeamCheckRepository(IContextResolver contextResolver, IQueryBuilder queryBuilder) : base(contextResolver, queryBuilder)
        {
        }

        #endregion

        #region ITeamCheckRepository implementation

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
            return base.Exists(id, "TeamContestant_Check", "Id", context);
        }

        public bool ExistsDuplicate(TeamContestantCheck teamCheck, ITransactionalContext? context = null)
        {
            return false;
        }

        public PaginatedResult<TeamContestantCheck> Get(TeamCheckSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            return this.GetTeamContestantChecks(searchFilter: searchFilter, paginationFilter: paginationFilter, sorting: sorting, context: context);
        }
      
        public void Create(TeamContestantCheck teamContestantCheck, ITransactionalContext? context = null)
        {
            this.CreateTeamCheck(teamContestantCheck, context);
        }

        #endregion

        #region Private Methods

        private PaginatedResult<TeamContestantCheck> GetTeamContestantChecks(TeamCheckSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            string sql = $@"SELECT
	                            [TeamContestant_Check].Id [Id],
	                            [TeamContestant_Check].CheckTime [CheckTime],
	                            [CheckType].Id [Id],
	                            [CheckType].Name [CheckType],
	                            [Team].Id [Id],
	                            [Team].Name [Name],
	                            [Team_Contestant].Id [Id],
	                            [ContestantRole].[Id],
	                            [ContestantRole].[Name],
	                            [Person].Id [Id],
	                            [Person].Firstname [Firstname],
	                            [Person].Lastname [Lastname]
                            FROM [TeamContestant_Check] [TeamContestant_Check]
                            INNER JOIN [CheckType] [CheckType] ON [CheckType].Id = [TeamContestant_Check].IdCheckType
                            LEFT JOIN [Team_Contestant] [Team_Contestant] ON [Team_Contestant].Id = [TeamContestant_Check].IdTeamContestant
                            LEFT JOIN [Team] [Team] ON [Team].Id = [Team_Contestant].IdTeam
                            LEFT JOIN [Competition] [Competition] ON [Competition].Id = [Team].IdCompetition                            
                            LEFT JOIN [ContestantRole] [ContestantRole] ON [ContestantRole].Id = [Team_Contestant].IdContestantRole
                            LEFT JOIN [Person] [Person] ON [Person].Id = [Team_Contestant].IdPerson";

            QueryBuilder.AddCommand(sql);

            ProcessSearchFilter(searchFilter);

            //QueryBuilder.AddSorting(sorting, _columnsMapping);
            QueryBuilder.AddSorting("[TeamContestant_Check].CheckTime ASC");
            QueryBuilder.AddPagination(paginationFilter);

            var teamContestantChecks = new List<TeamContestantCheck>();

            PaginatedResult<TeamContestantCheck> items = base.GetPaginatedResults<TeamContestantCheck>
                (
                    (reader) =>
                    {
                        return reader.Read<TeamContestantCheck, CheckType, Team, TeamContestant, ContestantRole, Person, TeamContestantCheck>
                        (
                            (teamContestantCheck, checkType, team, teamContestant, contestantRole, person) =>
                            {
                                teamContestant.Person = person;
                                teamContestant.Role = contestantRole;
                                teamContestant.Team = team;

                                teamContestantCheck.TeamContestant = teamContestant;
                                teamContestantCheck.CheckType = checkType;

                                teamContestantChecks.Add(teamContestantCheck);

                                return teamContestantCheck;
                            },
                            splitOn: "Id, Id, Id, Id, Id, Id"
                        ).AsList();
                    },
                    context
                );

            items.Results = teamContestantChecks;

            return items;
        }

        private void ProcessSearchFilter(TeamCheckSearchFilter? searchFilter)
        {
            if (searchFilter == null)
                return;

            base.AddFilterCriteria(ConditionType.Equal, "[Competition]", "Id", "idCompetition", searchFilter.Competition?.Id);
            base.AddFilterCriteria(ConditionType.Equal, "[Team]", "Id", "idTeam", searchFilter.Team?.Id);
            base.AddFilterCriteria(ConditionType.In, "[Team]", "IdRaceClass", "idsRaceClasses", searchFilter?.RaceClasses?.Select(x => x.Id));
            base.AddFilterCriteria(ConditionType.GreaterOrEqualThan, "[TeamContestant_Check]", "CheckTime", "dateFrom", searchFilter?.DateFrom?.UtcDateTime);
            base.AddFilterCriteria(ConditionType.LessOrEqualThan, "[TeamContestant_Check]", "CheckTime", "dateTo", searchFilter?.DateTo?.UtcDateTime);
        }

        private void CreateTeamCheck(TeamContestantCheck teamContestantCheck, ITransactionalContext? context = null)
        {
            var sb = new StringBuilder();

            sb.AppendLine("SELECT @idContestant = Id FROM [Team_Contestant] WHERE IdPerson = @idPerson;");

            sb.AppendLine(@" INSERT INTO [TeamContestant_Check]
                            ( IdTeamContestant, IdCheckType, CheckTime )
                        VALUES
                            ( @idContestant, @idCheck, @checkTime )");

            QueryBuilder.AddCommand(sb.ToString());

            QueryBuilder.AddParameter("idContestant", 0);
            QueryBuilder.AddParameter("idPerson", teamContestantCheck.TeamContestant.Person.Id);
            QueryBuilder.AddParameter("idCheck", (int)teamContestantCheck.CheckType);
            QueryBuilder.AddParameter("checkTime", teamContestantCheck.CheckTime);

            QueryBuilder.AddReturnLastInsertedId();

            teamContestantCheck.Id = base.Execute<int>(context);
        }

        #endregion
    }
}
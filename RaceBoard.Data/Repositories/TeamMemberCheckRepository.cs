using Dapper;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data.Helpers.Interfaces;
using RaceBoard.Data.Repositories.Base.Abstract;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;
using RaceBoard.Domain.Enums;
using System.Text;

namespace RaceBoard.Data.Repositories
{
    public class TeamMemberCheckRepository : AbstractRepository, ITeamMemberCheckRepository
    {
        #region Private Members

        private readonly Dictionary<string, string> _columnsMapping = new()
        {
            //[Team_MemberCheck].Id[Id],
            //[Team_MemberCheck].CheckTime[CheckTime],
            //[TeamMemberCheckType].Id[Id],
            //[TeamMemberCheckType].Name[TeamMemberCheckType],
            //[Team_TeamMember].Id[Id],
            //[TeamMemberRole].[Id],
            //[Team_Member].IdPerson[Id],
            //[Team].Id[Id],
            //[Person].Id[Id],
            //[Person].Firstname[Firstname],
            //[Person].Lastname[Lastname]

            { "Id", "[Team_Check].Id" },
            { "Team.Id", "[Team].Id" },
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

        public TeamMemberCheckRepository(IContextResolver contextResolver, IQueryBuilder queryBuilder) : base(contextResolver, queryBuilder)
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
            return base.Exists(id, "TeamMember_Check", "Id", context);
        }

        public bool ExistsDuplicate(TeamMemberCheck teamMemberCheck, ITransactionalContext? context = null)
        {
            return false;
        }

        public PaginatedResult<TeamMemberCheck> Get(TeamCheckSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            return this.GetTeamMemberChecks(searchFilter: searchFilter, paginationFilter: paginationFilter, sorting: sorting, context: context);
        }
      
        public void Create(TeamMemberCheck teamMemberCheck, ITransactionalContext? context = null)
        {
            this.CreateTeamCheck(teamMemberCheck, context);
        }

        #endregion

        #region Private Methods

        private PaginatedResult<TeamMemberCheck> GetTeamMemberChecks(TeamCheckSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            string sql = $@"SELECT
	                            [Team_MemberCheck].Id [Id],
	                            [Team_MemberCheck].CheckTime [CheckTime],
	                            [TeamMemberCheckType].Id [Id],
	                            [TeamMemberCheckType].Name [CheckType],
	                            [Team].Id [Id],
	                            [Team_Member].Id [Id],
	                            [TeamMemberRole].[Id],
	                            [TeamMemberRole].[Name],
	                            [Person].Id [Id],
	                            [Person].Firstname [Firstname],
	                            [Person].Lastname [Lastname]
                            FROM [Team_MemberCheck] [Team_MemberCheck]
                            INNER JOIN [TeamMemberCheckType] [TeamMemberCheckType] ON [TeamMemberCheckType].Id = [Team_MemberCheck].IdCheckType
                            LEFT JOIN [Team_Member] [Team_Member] ON [Team_Member].Id = [Team_MemberCheck].IdTeamMember
                            LEFT JOIN [Team] [Team] ON [Team].Id = [Team_Member].IdTeam
                            LEFT JOIN [Championship] [Championship] ON [Championship].Id = [Team].IdChampionship                            
                            LEFT JOIN [TeamMemberRole] [TeamMemberRole] ON [TeamMemberRole].Id = [Team_Member].IdTeamMemberRole
                            LEFT JOIN [Person] [Person] ON [Person].Id = [Team_Member].IdPerson";

            QueryBuilder.AddCommand(sql);

            ProcessSearchFilter(searchFilter);

            //QueryBuilder.AddSorting(sorting, _columnsMapping);
            QueryBuilder.AddSorting("[Team_MemberCheck].CheckTime ASC");
            QueryBuilder.AddPagination(paginationFilter);

            var teamMemberChecks = new List<TeamMemberCheck>();

            PaginatedResult<TeamMemberCheck> items = base.GetPaginatedResults<TeamMemberCheck>
                (
                    (reader) =>
                    {
                        return reader.Read<TeamMemberCheck, TeamMemberCheckType, Team, TeamMember, Domain.TeamMemberRole, Person, TeamMemberCheck>
                        (
                            (teamMemberCheck, checkType, team, teamMember, teamMemberRole, person) =>
                            {
                                teamMember.Person = person;
                                teamMember.Role = teamMemberRole;
                                teamMember.Team = team;

                                teamMemberCheck.TeamMember = teamMember;
                                teamMemberCheck.CheckType = checkType;

                                teamMemberChecks.Add(teamMemberCheck);

                                return teamMemberCheck;
                            },
                            splitOn: "Id, Id, Id, Id, Id, Id"
                        ).AsList();
                    },
                    context
                );

            items.Results = teamMemberChecks;

            return items;
        }

        private void ProcessSearchFilter(TeamCheckSearchFilter? searchFilter)
        {
            if (searchFilter == null)
                return;

            base.AddFilterCriteria(ConditionType.Equal, "[Championship]", "Id", "idChampionship", searchFilter.Championship?.Id);
            base.AddFilterCriteria(ConditionType.Equal, "[Team]", "Id", "idTeam", searchFilter.Team?.Id);
            base.AddFilterCriteria(ConditionType.In, "[Team]", "IdRaceClass", "idsRaceClasses", searchFilter?.RaceClasses?.Select(x => x.Id));
            base.AddFilterCriteria(ConditionType.GreaterOrEqualThan, "[Team_MemberCheck]", "CheckTime", "dateFrom", searchFilter?.DateFrom?.UtcDateTime);
            base.AddFilterCriteria(ConditionType.LessOrEqualThan, "[Team_MemberCheck]", "CheckTime", "dateTo", searchFilter?.DateTo?.UtcDateTime);
        }

        private void CreateTeamCheck(TeamMemberCheck teamMemberCheck, ITransactionalContext? context = null)
        {
            var sb = new StringBuilder();

            sb.AppendLine("SELECT @idTeamMember = Id FROM [Team_TeamMember] WHERE IdPerson = @idPerson;");

            sb.AppendLine(@" INSERT INTO [Team_MemberCheck]
                            ( IdTeamMember, IdTeamMemberCheckType, CheckTime )
                        VALUES
                            ( @idTeamMember, @idCheck, @checkTime )");

            QueryBuilder.AddCommand(sb.ToString());

            QueryBuilder.AddParameter("idTeamMember", 0);
            QueryBuilder.AddParameter("idPerson", teamMemberCheck.TeamMember.Person.Id);
            QueryBuilder.AddParameter("idCheck", (int)teamMemberCheck.CheckType);
            QueryBuilder.AddParameter("checkTime", teamMemberCheck.CheckTime);

            QueryBuilder.AddReturnLastInsertedId();

            teamMemberCheck.Id = base.Execute<int>(context);
        }

        #endregion
    }
}
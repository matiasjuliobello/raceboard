using Dapper;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data.Helpers.Interfaces;
using RaceBoard.Data.Repositories.Base.Abstract;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;
using System.Text;

namespace RaceBoard.Data.Repositories
{
    public class CoachTeamRepository : AbstractRepository, ICoachTeamRepository
    {
        #region Private Members

        private readonly Dictionary<string, string> _columnsMapping = new()
        {
            { "Id", "[Coach_Team].Id" },
            { "StartDate", "[Coach_Team].StartDate" },
            { "EndDate", "[Coach_Team].EndDate" },
            { "IsActive", "[Coach_Team].IsActive" },
            { "Coach.Id", "[Coach].Id" },
            { "Person.Id", "[Person].Id" },
            { "Person.Firstname", "[Person].Firstname" },
            { "Person.Lastname", "[Person].Lastname" },
            { "Team.Id", "[Team].Id" },
            { "Organization.Id", "[Organization].Id" },
            { "Organization.Name", "[Organization].Name" },
            { "Championship.Id", "[Championship].Id" },
            { "Championship.Name", "[Championship].Name" },
            { "RaceClass.Id", "[RaceClass].Id" },
            { "RaceClass.Name", "[RaceClass].Name" }
        };

        #endregion

        #region Constructors

        public CoachTeamRepository(IContextResolver contextResolver, IQueryBuilder queryBuilder) : base(contextResolver, queryBuilder)
        {
        }

        #endregion

        #region ICoachTeamRepository implementation

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
            return base.Exists(id, "Coach_Team", "Id", context);
        }

        public bool ExistsDuplicate(CoachTeam coachTeam, ITransactionalContext? context = null)
        {
            var sb = new StringBuilder();
            sb.AppendLine("[IdCoach] = @idCoach AND ");
            sb.AppendLine("[IdTeam] = @idTeam AND ");
            sb.AppendLine("[StartDate] = @startDate");

            string existsQuery = base.GetExistsDuplicateQuery("[Coach_Team]", sb.ToString(), "Id", "@id");

            QueryBuilder.AddCommand(existsQuery);
            QueryBuilder.AddParameter("idCoach", coachTeam.Coach.Id);
            QueryBuilder.AddParameter("idTeam", coachTeam.Team.Id);
            QueryBuilder.AddParameter("startDate", coachTeam.StartDate);
            QueryBuilder.AddParameter("id", coachTeam.Id);

            return base.Execute<bool>(context);
        }

        public PaginatedResult<CoachTeam> Get(CoachTeamSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            return this.GetCoachTeams(searchFilter, paginationFilter, sorting, context);
        }

        public CoachTeam? Get(int id, ITransactionalContext? context = null)
        {
            var searchFilter = new CoachTeamSearchFilter()
            {
                Ids = new int[] { id }
            };

            return this.GetCoachTeams(searchFilter, paginationFilter: null, sorting: null, context).Results.FirstOrDefault();
        }

        public void Create(CoachTeam coachTeam, ITransactionalContext? context = null)
        {
            this.CreateCoachTeam(coachTeam, context);
        }

        public void Update(CoachTeam coachTeam, ITransactionalContext? context = null)
        {
            this.UpdateCoachTeam(coachTeam, context);
        }

        #endregion

        #region Private Methods

        private PaginatedResult<CoachTeam> GetCoachTeams(CoachTeamSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            string sql = $@"SELECT
                                [Coach_Team].Id [Id],
                                [Coach_Team].StartDate [StartDate],
                                [Coach_Team].EndDate [EndDate],
                                [Coach_Team].IsActive [IsActive],
                                (SELECT COUNT(1) FROM [Team_Member] WHERE IdTeam = [Team].Id) [TeamMemberCount],
                                [Coach].Id [Id],
                                [Person].Id [Id],
                                [Person].Firstname [Firstname],
                                [Person].Lastname [Lastname],
                                [Team].Id [Id],
                                [Championship].Id [Id],
                                [Championship].Name [Name],
                                [RaceClass].Id [Id],
                                [RaceClass].Name [Name]
                            FROM [Coach_Team] [Coach_Team]
                            INNER JOIN [Coach] [Coach] ON [Coach].Id = [Coach_Team].IdCoach
                            INNER JOIN [Person] [Person] ON [Person].Id = [Coach].IdPerson
                            INNER JOIN [Team] [Team] ON [Team].Id = [Coach_Team].IdTeam
                            INNER JOIN [Championship] [Championship] ON [Championship].Id = [Team].IdChampionship
                            INNER JOIN [RaceClass] [RaceClass] ON [RaceClass].Id = [Team].IdRaceClass";

            QueryBuilder.AddCommand(sql);

            ProcessSearchFilter(searchFilter);

            QueryBuilder.AddSorting(sorting, _columnsMapping);
            QueryBuilder.AddPagination(paginationFilter);

            var coachTeams = new List<CoachTeam>();

            var items = base.GetPaginatedResults<CoachTeam>
                (
                    (reader) =>
                    {
                        return reader.Read<CoachTeam, Coach, Person, Team, Championship, RaceClass, CoachTeam>
                        (
                            (coachTeam, coach, person, team, championship, raceClass) =>
                            {
                                team.RaceClass = raceClass;
                                team.Championship = championship;
                                coachTeam.Team = team;

                                coach.Person = person;
                                coachTeam.Coach = coach;

                                coachTeams.Add(coachTeam);

                                return coachTeam;
                            },
                            splitOn: "Id, Id, Id, Id, Id, Id"
                        ).AsList();
                    },
                    context
                );

            items.Results = coachTeams;

            return items;
        }

        private void ProcessSearchFilter(CoachTeamSearchFilter? searchFilter = null)
        {
            if (searchFilter == null)
                return;

            base.AddFilterCriteria(ConditionType.In, "[Coach_Team]", "Id", "ids", searchFilter.Ids);
            base.AddFilterCriteria(ConditionType.GreaterOrEqualThan, "[Coach_Team]", "StartDate", "startDate", searchFilter.StartDate);
            base.AddFilterCriteria(ConditionType.LessOrEqualThan, "[Coach_Team]", "EndDate", "endDate", searchFilter.EndDate);
            base.AddFilterCriteria(ConditionType.Equal, "[Coach_Team]", "IsActive", "isActive", searchFilter.IsActive);
            base.AddFilterCriteria(ConditionType.Equal, "[Coach]", "Id", "idCoach", searchFilter.Coach?.Id);
            base.AddFilterCriteria(ConditionType.Equal, "[Person]", "Id", "idPerson", searchFilter.Coach?.Person?.Id);
            base.AddFilterCriteria(ConditionType.Equal, "[Team]", "Id", "idTeam", searchFilter.Team?.Id);
        }

        private void CreateCoachTeam(CoachTeam coachTeam, ITransactionalContext? context = null)
        {
            string sql = @" INSERT INTO [Coach_Team]
                                ( IdCoach, IdTeam, StartDate, EndDate, IsActive )
                            VALUES
                                ( @idCoach, @idTeam, @startDate, @endDate, @isActive )";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("idCoach", coachTeam.Coach.Id);
            QueryBuilder.AddParameter("idTeam", coachTeam.Team.Id);
            QueryBuilder.AddParameter("startDate", coachTeam.StartDate);
            QueryBuilder.AddParameter("endDate", coachTeam.EndDate);
            QueryBuilder.AddParameter("isActive", coachTeam.IsActive);

            QueryBuilder.AddReturnLastInsertedId();

            coachTeam.Id = base.Execute<int>(context);
        }

        private void UpdateCoachTeam(CoachTeam coachTeam, ITransactionalContext? context = null)
        {
            string sql = @" UPDATE [Coach_Team] SET EndDate = @endDate, IsActive = 0";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("endDate", coachTeam.EndDate);
            QueryBuilder.AddParameter("id", coachTeam.Id);
            QueryBuilder.AddCondition("Id = @id");

            base.ExecuteAndGetRowsAffected(context);
        }

        #endregion
    }
}
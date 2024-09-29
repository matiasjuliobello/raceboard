using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data.Helpers.Interfaces;
using RaceBoard.Data.Repositories.Base.Abstract;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Data;
using RaceBoard.Domain;
using Dapper;

public class TeamContestantRepository : AbstractRepository, ITeamContestantRepository
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

    public TeamContestantRepository(IContextResolver contextResolver, IQueryBuilder queryBuilder) : base(contextResolver, queryBuilder)
    {
    }

    #endregion

    #region ITeamContestantRepository implementation

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
        return base.Exists(id, "Team_Contestant", "Id", context);
    }

    public bool ExistsDuplicate(TeamContestant teamContestant, ITransactionalContext? context = null)
    {
        string condition = "[IdTeam] = @idTeam AND [IdPerson] = @idPerson";

        string existsQuery = base.GetExistsDuplicateQuery("[Team_Contestant]", condition, "Id", "@id");

        QueryBuilder.AddCommand(existsQuery);
        QueryBuilder.AddParameter("id", teamContestant.Id);
        QueryBuilder.AddParameter("idTeam", teamContestant.Team.Id);
        QueryBuilder.AddParameter("idPerson", teamContestant.Contestant.Id);

        return base.Execute<bool>(context);
    }

    public bool HasContestantInAnotherCompetitionTeam(TeamContestant teamContestant, ITransactionalContext? context = null)
    {
        // string query = @"	SELECT 
        //               1
        //              FROM [Team_Contestant] [Team_Contestant] 
        //              INNER JOIN [Team] [Team] ON [Team].Id = [Team_Contestant].IdTeam
        //              INNER JOIN [Competition] [Competition] ON [Competition].Id = [Team].IdCompetition
        //              WHERE 
        //               [Competition].Id =	(	
        //	                    SELECT IdCompetition FROM [Team_Contestant] 
        //	                    INNER JOIN [Competition] [Competition] ON [Competition].Id = [Team].IdCompetition
        //	                    WHERE [Team_Contestant].Id = @idTeamContestant AND [Team_Contestant].IdPerson = @idPerson
        //)";

        // string existsQuery = base.GetExistsDuplicateQuery("[Team_Contestant]", condition, "Id", "@id");

        // QueryBuilder.AddCommand(existsQuery);
        // QueryBuilder.AddParameter("id", teamContestant.Id);
        // QueryBuilder.AddParameter("idTeam", teamContestant.Team.Id);
        // QueryBuilder.AddParameter("idPerson", teamContestant.Contestant.Id);

        // return base.Execute<bool>(context);
        return false;
    }

    public bool HasParticipationOnRace(TeamContestant teamContestant, ITransactionalContext? context = null)
    {
        string condition = @"[Race_Complaint].IdTeamContestant = 
                            (
	                            SELECT [Team_Contestant].Id 
	                            FROM [Team_Contestant] [Team_Contestant] 
	                            INNER JOIN [Team] ON [Team].Id = [Team_Contestant].IdTeam
	                            WHERE [Team_Contestant].IdPerson = @idPerson AND [Team].Id = @idTeam
                            )";

        string existsQuery = base.GetExistsQuery("[Race_Complaint]", condition);

        QueryBuilder.AddCommand(existsQuery);
        QueryBuilder.AddParameter("idTeam", teamContestant.Team.Id);
        QueryBuilder.AddParameter("idPerson", teamContestant.Contestant.Id);

        return base.Execute<bool>(context);
    }

    public bool HasDuplicatedRole(TeamContestant teamContestant, ITransactionalContext? context = null)
    {
        string condition = base.GetExistsQuery("[Team_Contestant]", "[IdTeam] = @idTeam AND [IdContestantRole] = @idContestantRole");

        string query = $"{base.GetExcludeSameRecordCondition("Id", "id")} AND ({condition})";

        QueryBuilder.AddCommand(query);
        QueryBuilder.AddParameter("id", teamContestant.Id);
        QueryBuilder.AddParameter("idTeam", teamContestant.Team.Id);
        QueryBuilder.AddParameter("idContestantRole", teamContestant.Role.Id);

        return base.Execute<bool>(context);
    }

    public PaginatedResult<TeamContestant> Get(TeamContestantSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
    {
        return this.GetTeamContestants(searchFilter: searchFilter, paginationFilter: paginationFilter, sorting: sorting, context: context);
    }

    public TeamContestant? Get(int id, ITransactionalContext? context = null)
    {
        var searchFilter = new TeamContestantSearchFilter() { Ids = new int[] { id } };

        return this.GetTeamContestants(searchFilter: searchFilter, paginationFilter: null, sorting: null, context: context).Results.FirstOrDefault();
    }

    public void Create(TeamContestant teamContestant, ITransactionalContext? context = null)
    {
        this.CreateTeamContestant(teamContestant, context);
    }

    public void Update(TeamContestant teamContestant, ITransactionalContext? context = null)
    {
        this.UpdateTeamContestant(teamContestant, context);
    }

    public void Delete(TeamContestant teamContestant, ITransactionalContext? context = null)
    {
        this.DeleteTeamContestant(teamContestant, context);
    }

    #endregion

    #region Private Methods

    private PaginatedResult<TeamContestant> GetTeamContestants(TeamContestantSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
    {
        string sql = $@"SELECT
                                [Team_Contestant].Id [Id],
                                [Team].Id [Id],
                                [Team].Name [Name],
                                [Person].Id [Id],
                                [Person].Firstname [Firstname],
                                [Person].Lastname [Lastname],
                                [ContestantRole].Id [Id],
                                [ContestantRole].Name [Name]
                            FROM [Team_Contestant] [Team_Contestant]
                            INNER JOIN [Team] [Team] ON [Team].Id = [Team_Contestant].IdTeam
                            INNER JOIN [Person] [Person] ON [Person].Id = [Team_Contestant].IdPerson
                            INNER JOIN [ContestantRole] [ContestantRole] ON [ContestantRole].Id = [Team_Contestant].IdContestantRole";

        QueryBuilder.AddCommand(sql);

        ProcessSearchFilter(searchFilter);

        QueryBuilder.AddSorting(sorting, _columnsMapping);
        QueryBuilder.AddPagination(paginationFilter);

        var teamContestants = new List<TeamContestant>();

        PaginatedResult<TeamContestant> items = base.GetPaginatedResults<TeamContestant>
            (
                (reader) =>
                {
                    return reader.Read<TeamContestant, Team, Person, ContestantRole, TeamContestant>
                    (
                        (teamContestant, team, person, contestantRole) =>
                        {
                            teamContestant.Team = team;
                            teamContestant.Contestant = person;
                            teamContestant.Role = contestantRole;

                            teamContestants.Add(teamContestant);

                            return teamContestant;
                        },
                        splitOn: "Id, Id, Id, Id"
                    ).AsList();
                },
                context
            );

        items.Results = teamContestants;

        return items;
    }

    private void ProcessSearchFilter(TeamContestantSearchFilter? searchFilter)
    {
        if (searchFilter == null)
            return;

        base.AddFilterCriteria(ConditionType.In, "Team_Contestant", "Id", "ids", searchFilter.Ids);
        base.AddFilterCriteria(ConditionType.Equal, "Team_Contestant", "IdTeam", "idTeam", searchFilter.Team?.Id);
        base.AddFilterCriteria(ConditionType.Equal, "Team_Contestant", "IdPerson", "idPerson", searchFilter.Contestant?.Id);
        base.AddFilterCriteria(ConditionType.Equal, "Team_Contestant", "IdContestantRole", "idRole", searchFilter.Role?.Id);
    }

    private void CreateTeamContestant(TeamContestant teamContestant, ITransactionalContext? context = null)
    {
        string sql = @" INSERT INTO [Team_Contestant]
                                ( IdTeam, IdPerson, IdContestantRole )
                            VALUES
                                ( @idTeam, @idPerson, @idContestantRole )";

        QueryBuilder.AddCommand(sql);

        QueryBuilder.AddParameter("idTeam", teamContestant.Team.Id);
        QueryBuilder.AddParameter("idPerson", teamContestant.Contestant.Id);
        QueryBuilder.AddParameter("idContestantRole", teamContestant.Role.Id);

        QueryBuilder.AddReturnLastInsertedId();

        base.Execute<int>(context);
    }

    private void UpdateTeamContestant(TeamContestant teamContestant, ITransactionalContext? context = null)
    {
        string sql = @" UPDATE [Team_Contestant] SET
                                IdPerson = @idPerson,
                                IdContestantRole = @idContestantRole";

        QueryBuilder.AddCommand(sql);

        QueryBuilder.AddParameter("idContestantRole", teamContestant.Role.Id);
        QueryBuilder.AddParameter("idPerson", teamContestant.Contestant.Id);
        QueryBuilder.AddParameter("id", teamContestant.Id);
        QueryBuilder.AddCondition("Id = @id");

        base.ExecuteAndGetRowsAffected(context);
    }

    private int DeleteTeamContestant(TeamContestant teamContestant, ITransactionalContext? context = null)
    {
        return base.Delete("[Team_Contestant]", teamContestant.Id, "Id", context);
    }

    #endregion
}
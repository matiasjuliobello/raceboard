using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data.Helpers.Interfaces;
using RaceBoard.Data.Repositories.Base.Abstract;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Data;
using RaceBoard.Domain;
using Dapper;

public class TeamMemberRepository : AbstractRepository, ITeamMemberRepository
{
    #region Private Members

    private readonly Dictionary<string, string> _columnsMapping = new()
        {
            { "Id", "[Team].Id" },
            { "RaceClass.Id", "[RaceClass].Id" },
            { "RaceClass.Name", "[RaceClass].Name"},
            { "Competition.Id", "[Competition].Id" },
            { "Competition.Name", "[Competition].Name"},
            { "Competition.StartDate", "[Competition].StartDate"},
            { "Competition.EndDate", "[Competition].EndDate"}
        };

    #endregion

    #region Constructors

    public TeamMemberRepository(IContextResolver contextResolver, IQueryBuilder queryBuilder) : base(contextResolver, queryBuilder)
    {
    }

    #endregion

    #region ITeamMemberRepository implementation

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
        return base.Exists(id, "Team_Member", "Id", context);
    }

    public bool ExistsDuplicate(TeamMember teamMember, ITransactionalContext? context = null)
    {
        string condition = "[IdTeam] = @idTeam AND [IdPerson] = @idPerson";

        string existsQuery = base.GetExistsDuplicateQuery("[Team_Member]", condition, "Id", "@id");

        QueryBuilder.AddCommand(existsQuery);
        QueryBuilder.AddParameter("id", teamMember.Id);
        QueryBuilder.AddParameter("idTeam", teamMember.Team.Id);
        QueryBuilder.AddParameter("idPerson", teamMember.Person.Id);

        return base.Execute<bool>(context);
    }

    public bool HasMemberInAnotherCompetitionTeam(TeamMember teamMember, ITransactionalContext? context = null)
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

    public bool HasParticipationOnRace(TeamMember teamMember, ITransactionalContext? context = null)
    {
        //string condition = @"[Race_Protest].IdTeamContestant = 
        //                    (
        //                     SELECT [Team_Contestant].Id 
        //                     FROM [Team_Contestant] [Team_Contestant] 
        //                     INNER JOIN [Team] ON [Team].Id = [Team_Contestant].IdTeam
        //                     WHERE [Team_Contestant].IdPerson = @idPerson AND [Team].Id = @idTeam
        //                    )";

        //string existsQuery = base.GetExistsQuery("[Race_Protest]", condition);

        //QueryBuilder.AddCommand(existsQuery);
        //QueryBuilder.AddParameter("idTeam", teamContestant.Team.Id);
        //QueryBuilder.AddParameter("idPerson", teamContestant.Person.Id);

        //return base.Execute<bool>(context);

        return false;
    }

    public bool HasDuplicatedRole(TeamMember teamMember, ITransactionalContext? context = null)
    {
        string condition = base.GetExistsQuery("[Team_Member]", "[IdTeam] = @idTeam AND [IdRole] = @idTeamMemberRole");

        string query = $"{base.GetExcludeSameRecordCondition("Id", "id")} AND ({condition})";

        QueryBuilder.AddCommand(query);
        QueryBuilder.AddParameter("id", teamMember.Id);
        QueryBuilder.AddParameter("idTeam", teamMember.Team.Id);
        QueryBuilder.AddParameter("idTeamMemberRole", teamMember.Role.Id);

        return base.Execute<bool>(context);
    }

    public PaginatedResult<TeamMember> Get(TeamMemberSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
    {
        return this.GetTeamMembers(searchFilter: searchFilter, paginationFilter: paginationFilter, sorting: sorting, context: context);
    }

    public TeamMember? Get(int id, ITransactionalContext? context = null)
    {
        var searchFilter = new TeamMemberSearchFilter() { Ids = new int[] { id } };

        return this.GetTeamMembers(searchFilter: searchFilter, paginationFilter: null, sorting: null, context: context).Results.FirstOrDefault();
    }

    //public void Create(TeamMember teamMember, ITransactionalContext? context = null)
    //{
    //    this.CreateTeamMember(teamMember, context);
    //}
    public void CreateInvitation(TeamMemberInvitation teamMemberInvitation, ITransactionalContext? context = null)
    {
        this.CreateMemberRequest(teamMemberInvitation, context);
        this.CreateMemberRequestInvitation(teamMemberInvitation, context);
    }

    public void Update(TeamMember teamMember, ITransactionalContext? context = null)
    {
        this.UpdateTeamMember(teamMember, context);
    }

    public void Delete(TeamMember teamMember, ITransactionalContext? context = null)
    {
        this.DeleteTeamMember(teamMember, context);
    }

    #endregion

    #region Private Methods

    private PaginatedResult<TeamMember> GetTeamMembers(TeamMemberSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
    {
        string sql = $@"SELECT
                                [Team_Member].Id [Id],
                                [Team].Id [Id],
                                [Person].Id [Id],
                                [Person].Firstname [Firstname],
                                [Person].Lastname [Lastname],
                                [TeamMemberRole].Id [Id],
                                [TeamMemberRole].Name [Name]
                            FROM [Team_Member] [Team_Member]
                            INNER JOIN [Team] [Team] ON [Team].Id = [Team_Member].IdTeam
                            INNER JOIN [TeamMemberRole] [TeamMemberRole] ON [TeamMemberRole].Id = [Team_Member].IdRole
                            INNER JOIN [User] [User] ON [User].Id = [Team_Member].IdUser
                            INNER JOIN [User_Person] [User_Person] ON [User_Person].IdUser = [User].Id
                            INNER JOIN [Person] [Person] ON [Person].Id = [User_Person].IdPerson";;

        QueryBuilder.AddCommand(sql);

        ProcessSearchFilter(searchFilter);

        QueryBuilder.AddSorting(sorting, _columnsMapping);
        QueryBuilder.AddPagination(paginationFilter);

        var teamMembers = new List<TeamMember>();

        PaginatedResult<TeamMember> items = base.GetPaginatedResults<TeamMember>
            (
                (reader) =>
                {
                    return reader.Read<TeamMember, Team, Person, TeamMemberRole, TeamMember>
                    (
                        (teamMember, team, person, teamMemberRole) =>
                        {
                            teamMember.Team = team;
                            teamMember.Person = person;
                            teamMember.Role = teamMemberRole;

                            teamMembers.Add(teamMember);

                            return teamMember;
                        },
                        splitOn: "Id, Id, Id, Id"
                    ).AsList();
                },
                context
            );

        items.Results = teamMembers;

        return items;
    }

    private void ProcessSearchFilter(TeamMemberSearchFilter? searchFilter)
    {
        if (searchFilter == null)
            return;

        base.AddFilterCriteria(ConditionType.In,    "Team_Member", "Id", "ids", searchFilter.Ids);
        base.AddFilterCriteria(ConditionType.Equal, "Team_Member", "IdTeam", "idTeam", searchFilter.Team?.Id);
        base.AddFilterCriteria(ConditionType.Equal, "Team_Member", "IdPerson", "idPerson", searchFilter.Member?.Id);
        base.AddFilterCriteria(ConditionType.Equal, "Team_Member", "IdRole", "idTeamMemberRole", searchFilter.Role?.Id);
    }

    //private void CreateTeamMember(TeamMember teamMember, ITransactionalContext? context = null)
    //{
    //    string sql = @" INSERT INTO [Team_Member]
    //                            ( IdTeam, IdPerson, IdTeamMemberRole )
    //                        VALUES
    //                            ( @idTeam, @idPerson, @idTeamMemberRole )";

    //    QueryBuilder.AddCommand(sql);

    //    QueryBuilder.AddParameter("idTeam", teamMember.Team.Id);
    //    QueryBuilder.AddParameter("idPerson", teamMember.Person.Id);
    //    QueryBuilder.AddParameter("idTeamMemberRole", teamMember.Role.Id);

    //    QueryBuilder.AddReturnLastInsertedId();

    //    base.Execute<int>(context);
    //}
    private void CreateMemberRequest(TeamMemberInvitation teamMemberInvitation, ITransactionalContext? context = null)
    {
        string sql = @" INSERT INTO [Team_MemberRequest]
                            ( IdTeam, IdUser, IdRole, IdRequestUser, requestDate, IsPending )
                        VALUES
                            ( @idTeam, @idUser, @idRole, @idRequestUser, @requestDate, @isPending )";

        QueryBuilder.AddCommand(sql);

        QueryBuilder.AddParameter("idTeam", teamMemberInvitation.Team.Id);
        QueryBuilder.AddParameter("idUser", teamMemberInvitation.User?.Id);
        QueryBuilder.AddParameter("idRole", teamMemberInvitation.Role.Id);
        QueryBuilder.AddParameter("idRequestUser", teamMemberInvitation.RequestUser.Id);
        QueryBuilder.AddParameter("requestDate", teamMemberInvitation.RequestDate);
        QueryBuilder.AddParameter("isPending", true);

        QueryBuilder.AddReturnLastInsertedId();

        teamMemberInvitation.Id = base.Execute<int>(context);
    }

    private void CreateMemberRequestInvitation(TeamMemberInvitation teamMemberInvitation, ITransactionalContext? context = null)
    {
        string sql = @" INSERT INTO [Team_MemberRequestInvitation]
                            ( IdTeamMemberRequest, EmailAddress, Token, IsExpired )
                        VALUES
                            ( @idTeamMemberRequest, @emailAddress, @token, @isExpired )";

        QueryBuilder.AddCommand(sql);

        QueryBuilder.AddParameter("idTeamMemberRequest", teamMemberInvitation.Id);
        QueryBuilder.AddParameter("emailAddress", teamMemberInvitation.Invitation.EmailAddress);
        QueryBuilder.AddParameter("token", teamMemberInvitation.Invitation.Token);
        QueryBuilder.AddParameter("isExpired", false);

        QueryBuilder.AddReturnLastInsertedId();

        int id = base.Execute<int>(context);
    }


    private void UpdateTeamMember(TeamMember teamMember, ITransactionalContext? context = null)
    {
        string sql = @" UPDATE [Team_Member] SET
                                IdPerson = @idPerson,
                                IdRole = @idTeamMemberRole";

        QueryBuilder.AddCommand(sql);

        QueryBuilder.AddParameter("idTeamMemberRole", teamMember.Role.Id);
        QueryBuilder.AddParameter("idPerson", teamMember.Person.Id);
        QueryBuilder.AddParameter("id", teamMember.Id);
        QueryBuilder.AddCondition("Id = @id");

        base.ExecuteAndGetRowsAffected(context);
    }

    private int DeleteTeamMember(TeamMember teamMember, ITransactionalContext? context = null)
    {
        return base.Delete("[Team_Member]", teamMember.Id, "Id", context);
    }

    #endregion
}
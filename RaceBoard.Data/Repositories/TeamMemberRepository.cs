using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data.Helpers.Interfaces;
using RaceBoard.Data.Repositories.Base.Abstract;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Data;
using RaceBoard.Domain;
using Dapper;
using Microsoft.AspNetCore.Rewrite;

public class TeamMemberRepository : AbstractRepository, ITeamMemberRepository
{
    #region Private Members

    private readonly Dictionary<string, string> _columnsMapping = new()
        {
            { "Id", "[Team_Member].Id" },
            { "IsActive", "[Team_Member].IsActive" },
            { "JoinDate", "[Team_Member].JoinDate" },
            { "Team.Id", "[Team].Id" },
            { "User.Id", "[User].Id" },
            { "Person.Id", "[Person].Id" },
            { "Person.Firstname", "[Person].Firstname" },
            { "Person.Lastname", "[Person].Lastname" },
            { "Role.Id", "[Role].Id" },
            { "Role.Name", "[Role].Name" }
        };

    private readonly Dictionary<string, string> _invitationColumnsMapping = new()
        {
            { "Id", "[Team_MemberRequest].Id" },
            { "IsPending", "[Team_MemberRequest].IsPending" },
            { "RequestDate", "[Team_MemberRequest].RequestDate" },
            { "Team.Id", "[Team].Id" },
            { "User.Id", "[User].Id" },
            { "Person.Id", "[Person].Id" },
            { "Person.Firstname", "[Person].Firstname" },
            { "Person.Lastname", "[Person].Lastname" },
            { "Role.Id", "[Role].Id" },
            { "Role.Name", "[Role].Name" }
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

    public bool HasMemberInAnotherChampionshipTeam(TeamMember teamMember, ITransactionalContext? context = null)
    {
        // string query = @"	SELECT 
        //               1
        //              FROM [Team_Contestant] [Team_Contestant] 
        //              INNER JOIN [Team] [Team] ON [Team].Id = [Team_Contestant].IdTeam
        //              INNER JOIN [Championship] [Championship] ON [Championship].Id = [Team].IdChampionship
        //              WHERE 
        //               [Championship].Id =	(	
        //	                    SELECT IdChampionship FROM [Team_Contestant] 
        //	                    INNER JOIN [Championship] [Championship] ON [Championship].Id = [Team].IdChampionship
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

    public PaginatedResult<TeamMember> Get(TeamMemberSearchFilter searchFilter, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
    {
        return this.GetTeamMembers(searchFilter, paginationFilter, sorting, context);
    }

    public PaginatedResult<TeamMemberInvitation> GetInvitations(TeamMemberInvitationSearchFilter searchFilter, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
    {
        return this.GetTeamMemberInvitations(searchFilter, paginationFilter, sorting, context);
    }

    public void Add(TeamMember teamMember, ITransactionalContext? context = null)
    {
        this.AddMemberToTeam(teamMember, context);
    }

    public int Remove(int id, ITransactionalContext? context = null)
    {
        return this.RemoveMemberFromTeam(id, context);
    }

    public void CreateInvitation(TeamMemberInvitation teamMemberInvitation, ITransactionalContext? context = null)
    {
        this.CreateMemberRequest(teamMemberInvitation, context);
        this.CreateMemberRequestInvitation(teamMemberInvitation, context);
    }

    public void UpdateInvitation(TeamMemberInvitation teamMemberInvitation, ITransactionalContext? context = null)
    {
        this.UpdateMemberRequest(teamMemberInvitation, context);
        this.UpdateMemberRequestInvitation(teamMemberInvitation, context);
    }

    public void RemoveInvitation(TeamMemberInvitation teamMemberInvitation, ITransactionalContext? context = null)
    {
        this.RemoveMemberRequest(teamMemberInvitation, context);
        this.RemoveMemberRequestInvitation(teamMemberInvitation, context);
    }

    #endregion

    #region Private Methods

    private PaginatedResult<TeamMember> GetTeamMembers(TeamMemberSearchFilter searchFilter, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
    {
        string sql = $@"SELECT
	                            [Team_Member].Id [Id],
	                            [Team_Member].IsActive [IsActive],
	                            [Team_Member].JoinDate [JoinDate],
	                            [Team].Id [Id],
	                            [User].Id [Id],
	                            [Person].Id [Id],
	                            [Person].Firstname [Firstname],
	                            [Person].Lastname [Lastname],
	                            [Role].Id [Id],
	                            [Role].[Name] [Name]
                            FROM [Team_Member] [Team_Member]
                            INNER JOIN [Team] [Team] ON [Team].Id = [Team_Member].IdTeam
                            INNER JOIN [TeamMemberRole] [Role] ON [Role].Id = [Team_Member].IdRole
                            INNER JOIN [User] [User] ON [User].Id = [Team_Member].IdUser
                            INNER JOIN [User_Person] [User_Person] ON [User_Person].IdUser = [User].Id
                            INNER JOIN [Person] [Person] ON [Person].Id = [User_Person].IdPerson";

        QueryBuilder.AddCommand(sql);

        this.ProcessSearchFilter(searchFilter);

        QueryBuilder.AddSorting(sorting, _columnsMapping);
        QueryBuilder.AddPagination(paginationFilter);

        var teamMembers = new List<TeamMember>();

        PaginatedResult<TeamMember> items = base.GetPaginatedResults<TeamMember>
            (
                (reader) =>
                {
                    return reader.Read<TeamMember, Team, User, Person, TeamMemberRole, TeamMember>
                    (
                        (teamMember, team, user, person, role) =>
                        {
                            teamMember.Team = team;
                            teamMember.User = user;
                            teamMember.Person = person;
                            teamMember.Role = role;

                            teamMembers.Add(teamMember);

                            return teamMember;
                        },
                        splitOn: "Id, Id, Id, Id, Id"
                    );
                },
                context
            );

        base.__FixPaginationResults(ref items, teamMembers, paginationFilter);

        return items;
    }

    private PaginatedResult<TeamMemberInvitation> GetTeamMemberInvitations(TeamMemberInvitationSearchFilter searchFilter, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
    {
        string sql = $@"SELECT
	                        [Team_MemberRequest].Id [Id],
	                        [Team_MemberRequest].IsPending [IsPending],
	                        [Team_MemberRequest].RequestDate [RequestDate],
	                        [Team_MemberRequestInvitation].Id [Id],
	                        [Team_MemberRequestInvitation].EmailAddress [EmailAddress],
                            [Team_MemberRequestInvitation].IsExpired [IsExpired],
	                        [Team].Id [Id],
	                        [User].Id [Id],
	                        [Person].Id [Id],
	                        [Person].Firstname [Firstname],
	                        [Person].Lastname [Lastname],
	                        [Role].Id [Id],
	                        [Role].[Name] [Name]
                        FROM [Team_MemberRequest] [Team_MemberRequest]
                        INNER JOIN [Team_MemberRequestInvitation] [Team_MemberRequestInvitation] ON [Team_MemberRequestInvitation].IdTeamMemberRequest = [Team_MemberRequest].Id
                        INNER JOIN [Team] [Team] ON [Team].Id = [Team_MemberRequest].IdTeam
                        INNER JOIN [TeamMemberRole] [Role] ON [Role].Id = [Team_MemberRequest].IdRole
                        LEFT JOIN [User] [User] ON [User].Id = [Team_MemberRequest].IdUser
                        LEFT JOIN [User_Person] [User_Person] ON [User_Person].IdUser = [User].Id
                        LEFT JOIN [Person] [Person] ON [Person].Id = [User_Person].IdPerson";

        QueryBuilder.AddCommand(sql);

        this.ProcessInvitationSearchFilter(searchFilter);

        QueryBuilder.AddSorting(sorting, _invitationColumnsMapping);
        QueryBuilder.AddPagination(paginationFilter);

        var teamMemberInvitations = new List<TeamMemberInvitation>();

        PaginatedResult<TeamMemberInvitation> items = base.GetPaginatedResults<TeamMemberInvitation>
            (
                (reader) =>
                {
                    return reader.Read<TeamMemberInvitation, Invitation, Team, User, Person, TeamMemberRole, TeamMemberInvitation>
                    (
                        (teamMemberInvitation, invitation, team, user, person, role) =>
                        {
                            teamMemberInvitation.Invitation = invitation;
                            teamMemberInvitation.Team = team;
                            teamMemberInvitation.User = user;
                            teamMemberInvitation.Person = person;
                            teamMemberInvitation.Role = role;

                            teamMemberInvitations.Add(teamMemberInvitation);

                            return teamMemberInvitation;
                        },
                        splitOn: "Id, Id, Id, Id, Id, Id"
                    );
                },
                context
            );

        base.__FixPaginationResults(ref items, teamMemberInvitations, paginationFilter);

        return items;
    }

    private void AddMemberToTeam(TeamMember teamMember, ITransactionalContext? context = null)
    {
        string sql = @" INSERT INTO [Team_Member]
                                ( IdTeam, IdUser, IdRole, IsActive, JoinDate )
                            VALUES
                                ( @idTeam, @idUser, @idRole, @isActive, @joinDate )";

        QueryBuilder.AddCommand(sql);

        QueryBuilder.AddParameter("idTeam", teamMember.Team.Id);
        QueryBuilder.AddParameter("idUser", teamMember.User.Id);
        QueryBuilder.AddParameter("idRole", teamMember.Role.Id);
        QueryBuilder.AddParameter("isActive", teamMember.IsActive);
        QueryBuilder.AddParameter("joinDate", teamMember.JoinDate);

        QueryBuilder.AddReturnLastInsertedId();

        teamMember.Id = base.Execute<int>(context);
    }

    private int RemoveMemberFromTeam(int id, ITransactionalContext? context = null)
    {
        string sql = @" UPDATE [Team_Member] SET IsActive = @isActive";

        QueryBuilder.AddCommand(sql);

        QueryBuilder.AddParameter("isActive", false);
        QueryBuilder.AddParameter("id", id);

        QueryBuilder.AddCondition("Id = @id");

        return base.ExecuteAndGetRowsAffected(context);
    }

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

    private void UpdateMemberRequest(TeamMemberInvitation teamMemberInvitation, ITransactionalContext? context = null)
    {
        string sql = @" UPDATE [Team_MemberRequest] SET IdUser = @idUser, IsPending = @isPending";

        QueryBuilder.AddCommand(sql);

        QueryBuilder.AddParameter("idUser", teamMemberInvitation.User!.Id);
        QueryBuilder.AddParameter("isPending", false);
        QueryBuilder.AddParameter("id", teamMemberInvitation.Id);
        QueryBuilder.AddCondition("Id = @id");

        base.Execute<int>(context);
    }

    private void RemoveMemberRequest(TeamMemberInvitation teamMemberInvitation, ITransactionalContext? context = null)
    {
        string sql = @" UPDATE [Team_MemberRequest] SET IsPending = @isPending";

        QueryBuilder.AddCommand(sql);

        QueryBuilder.AddParameter("isPending", false);
        //QueryBuilder.AddParameter("idTeam", teamMemberInvitation.Team.Id);
        //QueryBuilder.AddParameter("idRole", teamMemberInvitation.Role.Id);
        //QueryBuilder.AddParameter("idUser", teamMemberInvitation.User!.Id);
        QueryBuilder.AddParameter("id", teamMemberInvitation.Id);

        //QueryBuilder.AddCondition("IdTeam = @idTeam AND IdRole = @idRole AND IdUser = @idUser");
        QueryBuilder.AddCondition("Id = @id");

        base.Execute<int>(context);
    }

    private void RemoveMemberRequestInvitation(TeamMemberInvitation teamMemberInvitation, ITransactionalContext? context = null)
    {
        string sql = @" UPDATE [Team_MemberRequestInvitation] SET IsExpired = @isExpired";

        QueryBuilder.AddCommand(sql);

        QueryBuilder.AddParameter("isExpired", true);
        QueryBuilder.AddParameter("id", teamMemberInvitation.Invitation.Id);

        QueryBuilder.AddCondition("Id = @id");

        base.Execute<int>(context);
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

    private void UpdateMemberRequestInvitation(TeamMemberInvitation teamMemberInvitation, ITransactionalContext? context = null)
    {
        string sql = @" UPDATE [Team_MemberRequestInvitation] SET IsExpired = @isExpired";

        QueryBuilder.AddCommand(sql);

        QueryBuilder.AddParameter("isExpired", true);
        //QueryBuilder.AddParameter("idTeamMemberRequest", teamMemberInvitation.Id);
        //QueryBuilder.AddCondition("IdTeamMemberRequest = @idTeamMemberRequest");
        QueryBuilder.AddParameter("id", teamMemberInvitation.Invitation.Id);
        QueryBuilder.AddCondition("id = @id");

        base.Execute<int>(context);
    }

    private void ProcessSearchFilter(TeamMemberSearchFilter searchFilter)
    {
        base.AddFilterCriteria(ConditionType.In, "Team_Member", "Id", "id", searchFilter.Ids);
        base.AddFilterCriteria(ConditionType.Equal, "Team_Member", "IdTeam", "idTeam", searchFilter.Team?.Id);
        base.AddFilterCriteria(ConditionType.Equal, "Team_Member", "idUser", "idUser", searchFilter.User?.Id);
        base.AddFilterCriteria(ConditionType.Equal, "Team_Member", "IsActive", "isActive", searchFilter.IsActive);

    }

    private void ProcessInvitationSearchFilter(TeamMemberInvitationSearchFilter searchFilter)
    {
        if (searchFilter == null)
            return;

        base.AddFilterCriteria(ConditionType.In, "Team_MemberRequest", "Id", "id", searchFilter.Ids);
        base.AddFilterCriteria(ConditionType.Equal, "Team_MemberRequest", "IdTeam", "idTeam", searchFilter.Team?.Id);
        base.AddFilterCriteria(ConditionType.Equal, "Team_MemberRequest", "IdRole", "idRole", searchFilter.Role?.Id);
        base.AddFilterCriteria(ConditionType.Equal, "Team_MemberRequest", "IdUser", "idUser", searchFilter.User?.Id);
        base.AddFilterCriteria(ConditionType.Equal, "Team_MemberRequest", "IsPending", "isPending", searchFilter.IsPending);
        base.AddFilterCriteria(ConditionType.Equal, "Team_MemberRequestInvitation", "Token", "token", searchFilter.Token);
        base.AddFilterCriteria(ConditionType.Equal, "Team_MemberRequestInvitation", "IsExpired", "isExpired", searchFilter.IsExpired);
    }

    #endregion

}


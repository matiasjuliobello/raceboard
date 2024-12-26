using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data.Helpers.Interfaces;
using RaceBoard.Data.Repositories.Base.Abstract;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;

namespace RaceBoard.Data.Repositories
{
    public class CompetitionMemberRepository : AbstractRepository, ICompetitionMemberRepository
    {
        #region Private Members

        private readonly Dictionary<string, string> _columnsMapping = new()
        {
            { "Id", "[Competition_Member].Id" },
            { "IsActive", "[Competition_Member].IsActive" },
            { "JoinDate", "[Competition_Member].JoinDate" },
            { "Competition.Id", "[Competition].Id" },
            { "User.Id", "[User].Id" },
            { "Person.Id", "[Person].Id" },
            { "Person.Firstname", "[Person].Firstname" },
            { "Person.Lastname", "[Person].Lastname" },
            { "Role.Id", "[Role].Id" },
            { "Role.Name", "[Role].Name" }
        };

        private readonly Dictionary<string, string> _invitationColumnsMapping = new()
        {
            { "Id", "[Competition_MemberRequest].Id" },
            { "IsPending", "[Competition_MemberRequest].IsPending" },
            { "RequestDate", "[Competition_MemberRequest].RequestDate" },
            { "Competition.Id", "[Competition].Id" },
            { "User.Id", "[User].Id" },
            { "Person.Id", "[Person].Id" },
            { "Person.Firstname", "[Person].Firstname" },
            { "Person.Lastname", "[Person].Lastname" },
            { "Role.Id", "[Role].Id" },
            { "Role.Name", "[Role].Name" }
        };

        #endregion

        #region Constructors

        public CompetitionMemberRepository(IContextResolver contextResolver, IQueryBuilder queryBuilder) : base(contextResolver, queryBuilder)
        {
        }

        #endregion

        #region IAuthorizationRepository implementation

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
            return base.Exists(id, "Competition_Member", "Id", context);
        }

        public bool ExistsDuplicate(CompetitionMember competitionMember, ITransactionalContext? context = null)
        {
            //string existsQuery = base.GetExistsDuplicateQuery("[Competition_Member]", "[Name] = @name AND IdCity = @idCity", "Id", "@id");

            //QueryBuilder.AddCommand(existsQuery);
            //QueryBuilder.AddParameter("name", competitionMember.Name);
            //QueryBuilder.AddParameter("idCity", competitionMember.City.Id);
            ////QueryBuilder.AddParameter("idsCompetition", competition.Competitions.Select(x => x.Id));
            //QueryBuilder.AddParameter("id", competitionMember.Id);

            //return base.Execute<bool>(context);

            return false;
        }

        public PaginatedResult<CompetitionMember> Get(CompetitionMemberSearchFilter searchFilter, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            return this.GetCompetitionMembers(searchFilter, paginationFilter, sorting, context);
        }

        public CompetitionMember? Get(int id, ITransactionalContext? context = null)
        {
            var searchFilter = new CompetitionMemberSearchFilter() { Ids = new int[] { id } };

            return this.GetCompetitionMembers(searchFilter: searchFilter, paginationFilter: null, sorting: null, context: context).Results.FirstOrDefault();
        }

        public PaginatedResult<CompetitionMemberInvitation> GetInvitations(CompetitionMemberInvitationSearchFilter searchFilter, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            return this.GetCompetitionMemberInvitations(searchFilter, paginationFilter, sorting, context);
        }

        public void Add(CompetitionMember competitionMember, ITransactionalContext? context = null)
        {
            this.AddMemberToCompetition(competitionMember, context);
        }

        public int Remove(int id, ITransactionalContext? context = null)
        {
            return this.RemoveMemberFromCompetition(id, context);
        }

        public void CreateInvitation(CompetitionMemberInvitation competitionMemberInvitation, ITransactionalContext? context = null)
        {
            this.CreateMemberRequest(competitionMemberInvitation, context);
            this.CreateMemberRequestInvitation(competitionMemberInvitation, context);
        }

        public void UpdateInvitation(CompetitionMemberInvitation competitionMemberInvitation, ITransactionalContext? context = null)
        {
            this.UpdateMemberRequest(competitionMemberInvitation, context);
            this.UpdateMemberRequestInvitation(competitionMemberInvitation, context);
        }

        public void RemoveInvitation(CompetitionMemberInvitation competitionMemberInvitation, ITransactionalContext? context = null)
        {
            this.RemoveMemberRequest(competitionMemberInvitation, context);
            this.RemoveMemberRequestInvitation(competitionMemberInvitation, context);
        }

        #endregion

        #region Private Methods

        private PaginatedResult<CompetitionMember> GetCompetitionMembers(CompetitionMemberSearchFilter searchFilter, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            string sql = $@"SELECT
	                            [Competition_Member].Id [Id],
	                            [Competition_Member].IsActive [IsActive],
	                            [Competition_Member].JoinDate [JoinDate],
	                            [Competition].Id [Id],
	                            [User].Id [Id],
	                            [Person].Id [Id],
	                            [Person].Firstname [Firstname],
	                            [Person].Lastname [Lastname],
	                            [Role].Id [Id],
	                            [Role].[Name] [Name]
                            FROM [Competition_Member] [Competition_Member]
                            INNER JOIN [Competition] [Competition] ON [Competition].Id = [Competition_Member].IdCompetition
                            INNER JOIN [Role] [Role] ON [Role].Id = [Competition_Member].IdRole
                            INNER JOIN [User] [User] ON [User].Id = [Competition_Member].IdUser
                            INNER JOIN [User_Person] [User_Person] ON [User_Person].IdUser = [User].Id
                            INNER JOIN [Person] [Person] ON [Person].Id = [User_Person].IdPerson";

            QueryBuilder.AddCommand(sql);

            this.ProcessSearchFilter(searchFilter);

            QueryBuilder.AddSorting(sorting, _columnsMapping);
            QueryBuilder.AddPagination(paginationFilter);

            var competitionMembers = new List<CompetitionMember>();

            PaginatedResult<CompetitionMember> items = base.GetPaginatedResults<CompetitionMember>
                (
                    (reader) =>
                    {
                        return reader.Read<CompetitionMember, Competition, User, Person, Role, CompetitionMember>
                        (
                            (competitionMember, competition, user, person, role) =>
                            {
                                competitionMember.Competition = competition;
                                competitionMember.User = user;
                                competitionMember.Person = person;
                                competitionMember.Role = role;

                                competitionMembers.Add(competitionMember);

                                return competitionMember;
                            },
                            splitOn: "Id, Id, Id, Id, Id"
                        );
                    },
                    context
                );

            base.__FixPaginationResults(ref items, competitionMembers, paginationFilter);

            return items;
        }

        private PaginatedResult<CompetitionMemberInvitation> GetCompetitionMemberInvitations(CompetitionMemberInvitationSearchFilter searchFilter, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            string sql = $@"SELECT
	                        [Competition_MemberRequest].Id [Id],
	                        [Competition_MemberRequest].IsPending [IsPending],
	                        [Competition_MemberRequest].RequestDate [RequestDate],
	                        [Competition_MemberRequestInvitation].Id [Id],
	                        [Competition_MemberRequestInvitation].EmailAddress [EmailAddress],
                            [Competition_MemberRequestInvitation].IsExpired [IsExpired],
	                        [Competition].Id [Id],
	                        [User].Id [Id],
	                        [Person].Id [Id],
	                        [Person].Firstname [Firstname],
	                        [Person].Lastname [Lastname],
	                        [Role].Id [Id],
	                        [Role].[Name] [Name]
                        FROM [Competition_MemberRequest] [Competition_MemberRequest]
                        INNER JOIN [Competition_MemberRequestInvitation] [Competition_MemberRequestInvitation] ON [Competition_MemberRequestInvitation].IdCompetitionMemberRequest = [Competition_MemberRequest].Id
                        INNER JOIN [Competition] [Competition] ON [Competition].Id = [Competition_MemberRequest].IdCompetition
                        INNER JOIN [Role] [Role] ON [Role].Id = [Competition_MemberRequest].IdRole
                        LEFT JOIN [User] [User] ON [User].Id = [Competition_MemberRequest].IdUser
                        LEFT JOIN [User_Person] [User_Person] ON [User_Person].IdUser = [User].Id
                        LEFT JOIN [Person] [Person] ON [Person].Id = [User_Person].IdPerson";

            QueryBuilder.AddCommand(sql);

            this.ProcessInvitationSearchFilter(searchFilter);

            QueryBuilder.AddSorting(sorting, _invitationColumnsMapping);
            QueryBuilder.AddPagination(paginationFilter);

            var competitionMemberInvitations = new List<CompetitionMemberInvitation>();

            PaginatedResult<CompetitionMemberInvitation> items = base.GetPaginatedResults<CompetitionMemberInvitation>
                (
                    (reader) =>
                    {
                        return reader.Read<CompetitionMemberInvitation, Invitation, Competition, User, Person, Role, CompetitionMemberInvitation>
                        (
                            (competitionMemberInvitation, invitation, competition, user, person, role) =>
                            {
                                competitionMemberInvitation.Invitation = invitation;
                                competitionMemberInvitation.Competition = competition;
                                competitionMemberInvitation.User = user;
                                competitionMemberInvitation.Person = person;
                                competitionMemberInvitation.Role = role;

                                competitionMemberInvitations.Add(competitionMemberInvitation);

                                return competitionMemberInvitation;
                            },
                            splitOn: "Id, Id, Id, Id, Id, Id"
                        );
                    },
                    context
                );

            base.__FixPaginationResults(ref items, competitionMemberInvitations, paginationFilter);

            return items;
        }

        private void AddMemberToCompetition(CompetitionMember competitionMember, ITransactionalContext? context = null)
        {
            string sql = @" INSERT INTO [Competition_Member]
                                ( IdCompetition, IdUser, IdRole, IsActive, JoinDate )
                            VALUES
                                ( @idCompetition, @idUser, @idRole, @isActive, @joinDate )";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("idCompetition", competitionMember.Competition.Id);
            QueryBuilder.AddParameter("idUser", competitionMember.User.Id);
            QueryBuilder.AddParameter("idRole", competitionMember.Role.Id);
            QueryBuilder.AddParameter("isActive", competitionMember.IsActive);
            QueryBuilder.AddParameter("joinDate", competitionMember.JoinDate);

            QueryBuilder.AddReturnLastInsertedId();

            competitionMember.Id = base.Execute<int>(context);
        }

        private int RemoveMemberFromCompetition(int id, ITransactionalContext? context = null)
        {
            string sql = @" UPDATE [Competition_Member] SET IsActive = @isActive";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("isActive", false);
            QueryBuilder.AddParameter("id", id);

            QueryBuilder.AddCondition("Id = @id");

            return base.ExecuteAndGetRowsAffected(context);
        }

        private void CreateMemberRequest(CompetitionMemberInvitation competitionMemberInvitation, ITransactionalContext? context = null)
        {
            string sql = @" INSERT INTO [Competition_MemberRequest]
                                ( IdCompetition, IdUser, IdRole, IdRequestUser, requestDate, IsPending )
                            VALUES
                                ( @idCompetition, @idUser, @idRole, @idRequestUser, @requestDate, @isPending )";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("idCompetition", competitionMemberInvitation.Competition.Id);
            QueryBuilder.AddParameter("idUser", competitionMemberInvitation.User?.Id);
            QueryBuilder.AddParameter("idRole", competitionMemberInvitation.Role.Id);
            QueryBuilder.AddParameter("idRequestUser", competitionMemberInvitation.RequestUser.Id);
            QueryBuilder.AddParameter("requestDate", competitionMemberInvitation.RequestDate);
            QueryBuilder.AddParameter("isPending", true);

            QueryBuilder.AddReturnLastInsertedId();

            competitionMemberInvitation.Id = base.Execute<int>(context);
        }

        private void UpdateMemberRequest(CompetitionMemberInvitation competitionMemberInvitation, ITransactionalContext? context = null)
        {
            string sql = @" UPDATE [Competition_MemberRequest] SET IdUser = @idUser, IsPending = @isPending";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("idUser", competitionMemberInvitation.User!.Id);
            QueryBuilder.AddParameter("isPending", false);
            QueryBuilder.AddParameter("id", competitionMemberInvitation.Id);
            QueryBuilder.AddCondition("Id = @id");

            base.Execute<int>(context);
        }

        private void RemoveMemberRequest(CompetitionMemberInvitation competitionMemberInvitation, ITransactionalContext? context = null)
        {
            string sql = @" UPDATE [Competition_MemberRequest] SET IsPending = @isPending";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("isPending", false);
            //QueryBuilder.AddParameter("idCompetition", competitionMemberInvitation.Competition.Id);
            //QueryBuilder.AddParameter("idRole", competitionMemberInvitation.Role.Id);
            //QueryBuilder.AddParameter("idUser", competitionMemberInvitation.User!.Id);
            QueryBuilder.AddParameter("id", competitionMemberInvitation.Id);

            //QueryBuilder.AddCondition("IdCompetition = @idCompetition AND IdRole = @idRole AND IdUser = @idUser");
            QueryBuilder.AddCondition("Id = @id");

            base.Execute<int>(context);
        }

        private void RemoveMemberRequestInvitation(CompetitionMemberInvitation competitionMemberInvitation, ITransactionalContext? context = null)
        {
            string sql = @" UPDATE [Competition_MemberRequestInvitation] SET IsExpired = @isExpired";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("isExpired", true);
            QueryBuilder.AddParameter("id", competitionMemberInvitation.Invitation.Id);

            QueryBuilder.AddCondition("Id = @id");

            base.Execute<int>(context);
        }

        private void CreateMemberRequestInvitation(CompetitionMemberInvitation competitionMemberInvitation, ITransactionalContext? context = null)
        {
            string sql = @" INSERT INTO [Competition_MemberRequestInvitation]
                                ( IdCompetitionMemberRequest, EmailAddress, Token, IsExpired )
                            VALUES
                                ( @idCompetitionMemberRequest, @emailAddress, @token, @isExpired )";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("idCompetitionMemberRequest", competitionMemberInvitation.Id);
            QueryBuilder.AddParameter("emailAddress", competitionMemberInvitation.Invitation.EmailAddress);
            QueryBuilder.AddParameter("token", competitionMemberInvitation.Invitation.Token);
            QueryBuilder.AddParameter("isExpired", false);

            QueryBuilder.AddReturnLastInsertedId();

            int id = base.Execute<int>(context);
        }

        private void UpdateMemberRequestInvitation(CompetitionMemberInvitation competitionMemberInvitation, ITransactionalContext? context = null)
        {
            string sql = @" UPDATE [Competition_MemberRequestInvitation] SET IsExpired = @isExpired";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("isExpired", true);
            //QueryBuilder.AddParameter("idCompetitionMemberRequest", competitionMemberInvitation.Id);
            //QueryBuilder.AddCondition("IdCompetitionMemberRequest = @idCompetitionMemberRequest");
            QueryBuilder.AddParameter("id", competitionMemberInvitation.Invitation.Id);
            QueryBuilder.AddCondition("id = @id");

            base.Execute<int>(context);
        }

        private void ProcessSearchFilter(CompetitionMemberSearchFilter searchFilter)
        {
            base.AddFilterCriteria(ConditionType.In, "Competition_Member", "Id", "id", searchFilter.Ids);
            base.AddFilterCriteria(ConditionType.Equal, "Competition_Member", "IdCompetition", "idCompetition", searchFilter.IdCompetition);
        }

        private void ProcessInvitationSearchFilter(CompetitionMemberInvitationSearchFilter searchFilter)
        {
            if (searchFilter == null)
                return;

            base.AddFilterCriteria(ConditionType.In, "Competition_MemberRequest", "Id", "id", searchFilter.Ids);
            base.AddFilterCriteria(ConditionType.Equal, "Competition_MemberRequest", "IdCompetition", "idCompetition", searchFilter.IdCompetition);
            base.AddFilterCriteria(ConditionType.Equal, "Competition_MemberRequest", "IdRole", "idRole", searchFilter.IdRole);
            base.AddFilterCriteria(ConditionType.Equal, "Competition_MemberRequest", "IdUser", "idUser", searchFilter.IdUser);
            base.AddFilterCriteria(ConditionType.Equal, "Competition_MemberRequest", "IsPending", "isPending", searchFilter.IsPending);
            base.AddFilterCriteria(ConditionType.Equal, "Competition_MemberRequestInvitation", "Token", "token", searchFilter.Token);
            base.AddFilterCriteria(ConditionType.Equal, "Competition_MemberRequestInvitation", "IsExpired", "isExpired", searchFilter.IsExpired);
        }

        #endregion

    }
}

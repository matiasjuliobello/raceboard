using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data.Helpers.Interfaces;
using RaceBoard.Data.Repositories.Base.Abstract;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;

namespace RaceBoard.Data.Repositories
{
    public class ChampionshipMemberRepository : AbstractRepository, IChampionshipMemberRepository
    {
        #region Private Members

        private readonly Dictionary<string, string> _columnsMapping = new()
        {
            { "Id", "[Championship_Member].Id" },
            { "IsActive", "[Championship_Member].IsActive" },
            { "JoinDate", "[Championship_Member].JoinDate" },
            { "Championship.Id", "[Championship].Id" },
            { "User.Id", "[User].Id" },
            { "Person.Id", "[Person].Id" },
            { "Person.Firstname", "[Person].Firstname" },
            { "Person.Lastname", "[Person].Lastname" },
            { "Role.Id", "[Role].Id" },
            { "Role.Name", "[Role].Name" }
        };

        private readonly Dictionary<string, string> _invitationColumnsMapping = new()
        {
            { "Id", "[Championship_MemberRequest].Id" },
            { "IsPending", "[Championship_MemberRequest].IsPending" },
            { "RequestDate", "[Championship_MemberRequest].RequestDate" },
            { "Championship.Id", "[Championship].Id" },
            { "User.Id", "[User].Id" },
            { "Person.Id", "[Person].Id" },
            { "Person.Firstname", "[Person].Firstname" },
            { "Person.Lastname", "[Person].Lastname" },
            { "Role.Id", "[Role].Id" },
            { "Role.Name", "[Role].Name" }
        };

        #endregion

        #region Constructors

        public ChampionshipMemberRepository(IContextResolver contextResolver, IQueryBuilder queryBuilder) : base(contextResolver, queryBuilder)
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
            return base.Exists(id, "Championship_Member", "Id", context);
        }

        public bool ExistsDuplicate(ChampionshipMember championshipMember, ITransactionalContext? context = null)
        {
            //string existsQuery = base.GetExistsDuplicateQuery("[Championship_Member]", "[Name] = @name AND IdCity = @idCity", "Id", "@id");

            //QueryBuilder.AddCommand(existsQuery);
            //QueryBuilder.AddParameter("name", championshipMember.Name);
            //QueryBuilder.AddParameter("idCity", championshipMember.City.Id);
            ////QueryBuilder.AddParameter("idsChampionship", championship.Championships.Select(x => x.Id));
            //QueryBuilder.AddParameter("id", championshipMember.Id);

            //return base.Execute<bool>(context);

            return false;
        }

        public PaginatedResult<ChampionshipMember> Get(ChampionshipMemberSearchFilter searchFilter, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            return this.GetChampionshipMembers(searchFilter, paginationFilter, sorting, context);
        }

        public ChampionshipMember? Get(int id, ITransactionalContext? context = null)
        {
            var searchFilter = new ChampionshipMemberSearchFilter() { Ids = new int[] { id } };

            return this.GetChampionshipMembers(searchFilter: searchFilter, paginationFilter: null, sorting: null, context: context).Results.FirstOrDefault();
        }

        public PaginatedResult<ChampionshipMemberInvitation> GetInvitations(ChampionshipMemberInvitationSearchFilter searchFilter, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            return this.GetChampionshipMemberInvitations(searchFilter, paginationFilter, sorting, context);
        }

        public void Add(ChampionshipMember championshipMember, ITransactionalContext? context = null)
        {
            this.AddMemberToChampionship(championshipMember, context);
        }

        public int Remove(int id, ITransactionalContext? context = null)
        {
            return this.RemoveMemberFromChampionship(id, context);
        }

        public void CreateInvitation(ChampionshipMemberInvitation championshipMemberInvitation, ITransactionalContext? context = null)
        {
            this.CreateMemberRequest(championshipMemberInvitation, context);
            this.CreateMemberRequestInvitation(championshipMemberInvitation, context);
        }

        public void UpdateInvitation(ChampionshipMemberInvitation championshipMemberInvitation, ITransactionalContext? context = null)
        {
            this.UpdateMemberRequest(championshipMemberInvitation, context);
            this.UpdateMemberRequestInvitation(championshipMemberInvitation, context);
        }

        public void RemoveInvitation(ChampionshipMemberInvitation championshipMemberInvitation, ITransactionalContext? context = null)
        {
            this.RemoveMemberRequest(championshipMemberInvitation, context);
            this.RemoveMemberRequestInvitation(championshipMemberInvitation, context);
        }

        #endregion

        #region Private Methods

        private PaginatedResult<ChampionshipMember> GetChampionshipMembers(ChampionshipMemberSearchFilter searchFilter, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            string sql = $@"SELECT
	                            [Championship_Member].Id [Id],
	                            [Championship_Member].IsActive [IsActive],
	                            [Championship_Member].JoinDate [JoinDate],
	                            [Championship].Id [Id],
	                            [User].Id [Id],
	                            [Person].Id [Id],
	                            [Person].Firstname [Firstname],
	                            [Person].Lastname [Lastname],
	                            [Role].Id [Id],
	                            [Role].[Name] [Name]
                            FROM [Championship_Member] [Championship_Member]
                            INNER JOIN [Championship] [Championship] ON [Championship].Id = [Championship_Member].IdChampionship
                            INNER JOIN [Role] [Role] ON [Role].Id = [Championship_Member].IdRole
                            INNER JOIN [User] [User] ON [User].Id = [Championship_Member].IdUser
                            INNER JOIN [User_Person] [User_Person] ON [User_Person].IdUser = [User].Id
                            INNER JOIN [Person] [Person] ON [Person].Id = [User_Person].IdPerson";

            QueryBuilder.AddCommand(sql);

            this.ProcessSearchFilter(searchFilter);

            QueryBuilder.AddSorting(sorting, _columnsMapping);
            QueryBuilder.AddPagination(paginationFilter);

            var championshipMembers = new List<ChampionshipMember>();

            PaginatedResult<ChampionshipMember> items = base.GetPaginatedResults<ChampionshipMember>
                (
                    (reader) =>
                    {
                        return reader.Read<ChampionshipMember, Championship, User, Person, Role, ChampionshipMember>
                        (
                            (championshipMember, championship, user, person, role) =>
                            {
                                championshipMember.Championship = championship;
                                championshipMember.User = user;
                                championshipMember.Person = person;
                                championshipMember.Role = role;

                                championshipMembers.Add(championshipMember);

                                return championshipMember;
                            },
                            splitOn: "Id, Id, Id, Id, Id"
                        );
                    },
                    context
                );

            base.__FixPaginationResults(ref items, championshipMembers, paginationFilter);

            return items;
        }

        private PaginatedResult<ChampionshipMemberInvitation> GetChampionshipMemberInvitations(ChampionshipMemberInvitationSearchFilter searchFilter, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            string sql = $@"SELECT
	                        [Championship_MemberRequest].Id [Id],
	                        [Championship_MemberRequest].IsPending [IsPending],
	                        [Championship_MemberRequest].RequestDate [RequestDate],
	                        [Championship_MemberRequestInvitation].Id [Id],
	                        [Championship_MemberRequestInvitation].EmailAddress [EmailAddress],
                            [Championship_MemberRequestInvitation].IsExpired [IsExpired],
	                        [Championship].Id [Id],
	                        [User].Id [Id],
	                        [Person].Id [Id],
	                        [Person].Firstname [Firstname],
	                        [Person].Lastname [Lastname],
	                        [Role].Id [Id],
	                        [Role].[Name] [Name]
                        FROM [Championship_MemberRequest] [Championship_MemberRequest]
                        INNER JOIN [Championship_MemberRequestInvitation] [Championship_MemberRequestInvitation] ON [Championship_MemberRequestInvitation].IdChampionshipMemberRequest = [Championship_MemberRequest].Id
                        INNER JOIN [Championship] [Championship] ON [Championship].Id = [Championship_MemberRequest].IdChampionship
                        INNER JOIN [Role] [Role] ON [Role].Id = [Championship_MemberRequest].IdRole
                        LEFT JOIN [User] [User] ON [User].Id = [Championship_MemberRequest].IdUser
                        LEFT JOIN [User_Person] [User_Person] ON [User_Person].IdUser = [User].Id
                        LEFT JOIN [Person] [Person] ON [Person].Id = [User_Person].IdPerson";

            QueryBuilder.AddCommand(sql);

            this.ProcessInvitationSearchFilter(searchFilter);

            QueryBuilder.AddSorting(sorting, _invitationColumnsMapping);
            QueryBuilder.AddPagination(paginationFilter);

            var championshipMemberInvitations = new List<ChampionshipMemberInvitation>();

            PaginatedResult<ChampionshipMemberInvitation> items = base.GetPaginatedResults<ChampionshipMemberInvitation>
                (
                    (reader) =>
                    {
                        return reader.Read<ChampionshipMemberInvitation, Invitation, Championship, User, Person, Role, ChampionshipMemberInvitation>
                        (
                            (championshipMemberInvitation, invitation, championship, user, person, role) =>
                            {
                                championshipMemberInvitation.Invitation = invitation;
                                championshipMemberInvitation.Championship = championship;
                                championshipMemberInvitation.User = user;
                                championshipMemberInvitation.Person = person;
                                championshipMemberInvitation.Role = role;

                                championshipMemberInvitations.Add(championshipMemberInvitation);

                                return championshipMemberInvitation;
                            },
                            splitOn: "Id, Id, Id, Id, Id, Id"
                        );
                    },
                    context
                );

            base.__FixPaginationResults(ref items, championshipMemberInvitations, paginationFilter);

            return items;
        }

        private void AddMemberToChampionship(ChampionshipMember championshipMember, ITransactionalContext? context = null)
        {
            string sql = @" INSERT INTO [Championship_Member]
                                ( IdChampionship, IdUser, IdRole, IsActive, JoinDate )
                            VALUES
                                ( @idChampionship, @idUser, @idRole, @isActive, @joinDate )";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("idChampionship", championshipMember.Championship.Id);
            QueryBuilder.AddParameter("idUser", championshipMember.User.Id);
            QueryBuilder.AddParameter("idRole", championshipMember.Role.Id);
            QueryBuilder.AddParameter("isActive", championshipMember.IsActive);
            QueryBuilder.AddParameter("joinDate", championshipMember.JoinDate);

            QueryBuilder.AddReturnLastInsertedId();

            championshipMember.Id = base.Execute<int>(context);
        }

        private int RemoveMemberFromChampionship(int id, ITransactionalContext? context = null)
        {
            string sql = @" UPDATE [Championship_Member] SET IsActive = @isActive";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("isActive", false);
            QueryBuilder.AddParameter("id", id);

            QueryBuilder.AddCondition("Id = @id");

            return base.ExecuteAndGetRowsAffected(context);
        }

        private void CreateMemberRequest(ChampionshipMemberInvitation championshipMemberInvitation, ITransactionalContext? context = null)
        {
            string sql = @" INSERT INTO [Championship_MemberRequest]
                                ( IdChampionship, IdUser, IdRole, IdRequestUser, requestDate, IsPending )
                            VALUES
                                ( @idChampionship, @idUser, @idRole, @idRequestUser, @requestDate, @isPending )";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("idChampionship", championshipMemberInvitation.Championship.Id);
            QueryBuilder.AddParameter("idUser", championshipMemberInvitation.User?.Id);
            QueryBuilder.AddParameter("idRole", championshipMemberInvitation.Role.Id);
            QueryBuilder.AddParameter("idRequestUser", championshipMemberInvitation.RequestUser.Id);
            QueryBuilder.AddParameter("requestDate", championshipMemberInvitation.RequestDate);
            QueryBuilder.AddParameter("isPending", true);

            QueryBuilder.AddReturnLastInsertedId();

            championshipMemberInvitation.Id = base.Execute<int>(context);
        }

        private void UpdateMemberRequest(ChampionshipMemberInvitation championshipMemberInvitation, ITransactionalContext? context = null)
        {
            string sql = @" UPDATE [Championship_MemberRequest] SET IdUser = @idUser, IsPending = @isPending";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("idUser", championshipMemberInvitation.User!.Id);
            QueryBuilder.AddParameter("isPending", false);
            QueryBuilder.AddParameter("id", championshipMemberInvitation.Id);
            QueryBuilder.AddCondition("Id = @id");

            base.Execute<int>(context);
        }

        private void RemoveMemberRequest(ChampionshipMemberInvitation championshipMemberInvitation, ITransactionalContext? context = null)
        {
            string sql = @" UPDATE [Championship_MemberRequest] SET IsPending = @isPending";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("isPending", false);
            //QueryBuilder.AddParameter("idChampionship", championshipMemberInvitation.Championship.Id);
            //QueryBuilder.AddParameter("idRole", championshipMemberInvitation.Role.Id);
            //QueryBuilder.AddParameter("idUser", championshipMemberInvitation.User!.Id);
            QueryBuilder.AddParameter("id", championshipMemberInvitation.Id);

            //QueryBuilder.AddCondition("IdChampionship = @idChampionship AND IdRole = @idRole AND IdUser = @idUser");
            QueryBuilder.AddCondition("Id = @id");

            base.Execute<int>(context);
        }

        private void RemoveMemberRequestInvitation(ChampionshipMemberInvitation championshipMemberInvitation, ITransactionalContext? context = null)
        {
            string sql = @" UPDATE [Championship_MemberRequestInvitation] SET IsExpired = @isExpired";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("isExpired", true);
            QueryBuilder.AddParameter("id", championshipMemberInvitation.Invitation.Id);

            QueryBuilder.AddCondition("Id = @id");

            base.Execute<int>(context);
        }

        private void CreateMemberRequestInvitation(ChampionshipMemberInvitation championshipMemberInvitation, ITransactionalContext? context = null)
        {
            string sql = @" INSERT INTO [Championship_MemberRequestInvitation]
                                ( IdChampionshipMemberRequest, EmailAddress, Token, IsExpired )
                            VALUES
                                ( @idChampionshipMemberRequest, @emailAddress, @token, @isExpired )";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("idChampionshipMemberRequest", championshipMemberInvitation.Id);
            QueryBuilder.AddParameter("emailAddress", championshipMemberInvitation.Invitation.EmailAddress);
            QueryBuilder.AddParameter("token", championshipMemberInvitation.Invitation.Token);
            QueryBuilder.AddParameter("isExpired", false);

            QueryBuilder.AddReturnLastInsertedId();

            int id = base.Execute<int>(context);
        }

        private void UpdateMemberRequestInvitation(ChampionshipMemberInvitation championshipMemberInvitation, ITransactionalContext? context = null)
        {
            string sql = @" UPDATE [Championship_MemberRequestInvitation] SET IsExpired = @isExpired";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("isExpired", true);
            //QueryBuilder.AddParameter("idChampionshipMemberRequest", championshipMemberInvitation.Id);
            //QueryBuilder.AddCondition("IdChampionshipMemberRequest = @idChampionshipMemberRequest");
            QueryBuilder.AddParameter("id", championshipMemberInvitation.Invitation.Id);
            QueryBuilder.AddCondition("id = @id");

            base.Execute<int>(context);
        }

        private void ProcessSearchFilter(ChampionshipMemberSearchFilter searchFilter)
        {
            base.AddFilterCriteria(ConditionType.In, "Championship_Member", "Id", "id", searchFilter.Ids);
            base.AddFilterCriteria(ConditionType.Equal, "Championship_Member", "IdChampionship", "idChampionship", searchFilter.Championship?.Id);
            base.AddFilterCriteria(ConditionType.Equal, "Championship_Member", "IdUser", "idUser", searchFilter.User?.Id);
            base.AddFilterCriteria(ConditionType.Equal, "Championship_Member", "IsActive", "isActive", searchFilter.IsActive);
        }

        private void ProcessInvitationSearchFilter(ChampionshipMemberInvitationSearchFilter searchFilter)
        {
            if (searchFilter == null)
                return;

            base.AddFilterCriteria(ConditionType.In, "Championship_MemberRequest", "Id", "id", searchFilter.Ids);
            base.AddFilterCriteria(ConditionType.Equal, "Championship_MemberRequest", "IdChampionship", "idChampionship", searchFilter.Championship?.Id);
            base.AddFilterCriteria(ConditionType.Equal, "Championship_MemberRequest", "IdRole", "idRole", searchFilter.Role?.Id);
            base.AddFilterCriteria(ConditionType.Equal, "Championship_MemberRequest", "IdUser", "idUser", searchFilter.User?.Id);
            base.AddFilterCriteria(ConditionType.Equal, "Championship_MemberRequest", "IsPending", "isPending", searchFilter.IsPending);
            base.AddFilterCriteria(ConditionType.Equal, "Championship_MemberRequestInvitation", "Token", "token", searchFilter.Token);
            base.AddFilterCriteria(ConditionType.Equal, "Championship_MemberRequestInvitation", "IsExpired", "isExpired", searchFilter.IsExpired);
        }

        #endregion
    }
}

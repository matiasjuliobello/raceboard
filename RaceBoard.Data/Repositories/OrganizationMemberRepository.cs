using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data.Helpers.Interfaces;
using RaceBoard.Data.Repositories.Base.Abstract;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;


namespace RaceBoard.Data.Repositories
{
    public class OrganizationMemberRepository : AbstractRepository, IOrganizationMemberRepository
    {
        #region Private Members

        private readonly Dictionary<string, string> _columnsMapping = new()
        {
            { "Id", "[Organization_Member].Id" },
            { "IsActive", "[Organization_Member].IsActive" },
            { "JoinDate", "[Organization_Member].JoinDate" },
            { "Organization.Id", "[Organization].Id" },
            { "User.Id", "[User].Id" },
            { "Person.Id", "[Person].Id" },
            { "Person.Firstname", "[Person].Firstname" },
            { "Person.Lastname", "[Person].Lastname" },
            { "Role.Id", "[Role].Id" },
            { "Role.Name", "[Role].Name" }
        };

        private readonly Dictionary<string, string> _invitationColumnsMapping = new()
        {
            { "Id", "[Organization_MemberRequest].Id" },
            { "IsPending", "[Organization_MemberRequest].IsPending" },
            { "RequestDate", "[Organization_MemberRequest].RequestDate" },
            { "Organization.Id", "[Organization].Id" },
            { "User.Id", "[User].Id" },
            { "Person.Id", "[Person].Id" },
            { "Person.Firstname", "[Person].Firstname" },
            { "Person.Lastname", "[Person].Lastname" },
            { "Role.Id", "[Role].Id" },
            { "Role.Name", "[Role].Name" }
        };

        #endregion

        #region Constructors

        public OrganizationMemberRepository(IContextResolver contextResolver, IQueryBuilder queryBuilder) : base(contextResolver, queryBuilder)
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
            return base.Exists(id, "Organization_Member", "Id", context);
        }

        public bool ExistsDuplicate(OrganizationMember organizationMember, ITransactionalContext? context = null)
        {
            //string existsQuery = base.GetExistsDuplicateQuery("[Organization_Member]", "[Name] = @name AND IdCity = @idCity", "Id", "@id");

            //QueryBuilder.AddCommand(existsQuery);
            //QueryBuilder.AddParameter("name", organizationMember.Name);
            //QueryBuilder.AddParameter("idCity", organizationMember.City.Id);
            ////QueryBuilder.AddParameter("idsOrganization", championship.Organizations.Select(x => x.Id));
            //QueryBuilder.AddParameter("id", organizationMember.Id);

            //return base.Execute<bool>(context);

            return false;
        }

        public PaginatedResult<OrganizationMember> Get(OrganizationMemberSearchFilter searchFilter, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            return this.GetOrganizationMembers(searchFilter, paginationFilter, sorting, context);
        }

        public PaginatedResult<OrganizationMemberInvitation> GetInvitations(OrganizationMemberInvitationSearchFilter searchFilter, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            return this.GetOrganizationMemberInvitations(searchFilter, paginationFilter, sorting, context);
        }

        public void Add(OrganizationMember organizationMember, ITransactionalContext? context = null)
        {
            this.AddMemberToOrganization(organizationMember, context);
        }

        public int Remove(int id, ITransactionalContext? context = null)
        {
            return this.RemoveMemberFromOrganization(id, context);
        }

        public void CreateInvitation(OrganizationMemberInvitation organizationMemberInvitation, ITransactionalContext? context = null)
        {
            this.CreateMemberRequest(organizationMemberInvitation, context);
            this.CreateMemberRequestInvitation(organizationMemberInvitation, context);
        }

        public void UpdateInvitation(OrganizationMemberInvitation organizationMemberInvitation, ITransactionalContext? context = null)
        {
            this.UpdateMemberRequest(organizationMemberInvitation, context);
            this.UpdateMemberRequestInvitation(organizationMemberInvitation, context);
        }

        public void RemoveInvitation(OrganizationMemberInvitation organizationMemberInvitation, ITransactionalContext? context = null)
        {
            this.RemoveMemberRequest(organizationMemberInvitation, context);
            this.RemoveMemberRequestInvitation(organizationMemberInvitation, context);
        }

        #endregion

        #region Private Methods

        private PaginatedResult<OrganizationMember> GetOrganizationMembers(OrganizationMemberSearchFilter searchFilter, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            string sql = $@"SELECT
	                            [Organization_Member].Id [Id],
	                            [Organization_Member].IsActive [IsActive],
	                            [Organization_Member].JoinDate [JoinDate],
	                            [Organization].Id [Id],
	                            [User].Id [Id],
	                            [Person].Id [Id],
	                            [Person].Firstname [Firstname],
	                            [Person].Lastname [Lastname],
	                            [Role].Id [Id],
	                            [Role].[Name] [Name]
                            FROM [Organization_Member] [Organization_Member]
                            INNER JOIN [Organization] [Organization] ON [Organization].Id = [Organization_Member].IdOrganization
                            INNER JOIN [Role] [Role] ON [Role].Id = [Organization_Member].IdRole
                            INNER JOIN [User] [User] ON [User].Id = [Organization_Member].IdUser
                            INNER JOIN [User_Person] [User_Person] ON [User_Person].IdUser = [User].Id
                            INNER JOIN [Person] [Person] ON [Person].Id = [User_Person].IdPerson";

            QueryBuilder.AddCommand(sql);

            this.ProcessSearchFilter(searchFilter);

            QueryBuilder.AddSorting(sorting, _columnsMapping);
            QueryBuilder.AddPagination(paginationFilter);

            var organizationMembers = new List<OrganizationMember>();

            PaginatedResult<OrganizationMember> items = base.GetPaginatedResults<OrganizationMember>
                (
                    (reader) =>
                    {
                        return reader.Read<OrganizationMember, Organization, User, Person, Role, OrganizationMember>
                        (
                            (organizationMember, organization, user, person, role) =>
                            {
                                organizationMember.Organization = organization;
                                organizationMember.User = user;
                                organizationMember.Person = person;
                                organizationMember.Role = role;

                                organizationMembers.Add(organizationMember);

                                return organizationMember;
                            },
                            splitOn: "Id, Id, Id, Id, Id"
                        );
                    },
                    context
                );

            base.__FixPaginationResults(ref items, organizationMembers, paginationFilter);

            return items;
        }

        private PaginatedResult<OrganizationMemberInvitation> GetOrganizationMemberInvitations(OrganizationMemberInvitationSearchFilter searchFilter, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            string sql = $@"SELECT
	                        [Organization_MemberRequest].Id [Id],
	                        [Organization_MemberRequest].IsPending [IsPending],
	                        [Organization_MemberRequest].RequestDate [RequestDate],
	                        [Organization_MemberRequestInvitation].Id [Id],
	                        [Organization_MemberRequestInvitation].EmailAddress [EmailAddress],
                            [Organization_MemberRequestInvitation].IsExpired [IsExpired],
	                        [Organization].Id [Id],
	                        [User].Id [Id],
	                        [Person].Id [Id],
	                        [Person].Firstname [Firstname],
	                        [Person].Lastname [Lastname],
	                        [Role].Id [Id],
	                        [Role].[Name] [Name]
                        FROM [Organization_MemberRequest] [Organization_MemberRequest]
                        INNER JOIN [Organization_MemberRequestInvitation] [Organization_MemberRequestInvitation] ON [Organization_MemberRequestInvitation].IdOrganizationMemberRequest = [Organization_MemberRequest].Id
                        INNER JOIN [Organization] [Organization] ON [Organization].Id = [Organization_MemberRequest].IdOrganization
                        INNER JOIN [Role] [Role] ON [Role].Id = [Organization_MemberRequest].IdRole
                        LEFT JOIN [User] [User] ON [User].Id = [Organization_MemberRequest].IdUser
                        LEFT JOIN [User_Person] [User_Person] ON [User_Person].IdUser = [User].Id
                        LEFT JOIN [Person] [Person] ON [Person].Id = [User_Person].IdPerson";

            QueryBuilder.AddCommand(sql);

            this.ProcessInvitationSearchFilter(searchFilter);

            QueryBuilder.AddSorting(sorting, _invitationColumnsMapping);
            QueryBuilder.AddPagination(paginationFilter);

            var organizationMemberInvitations = new List<OrganizationMemberInvitation>();

            PaginatedResult<OrganizationMemberInvitation> items = base.GetPaginatedResults<OrganizationMemberInvitation>
                (
                    (reader) =>
                    {
                        return reader.Read<OrganizationMemberInvitation, Invitation, Organization, User, Person, Role, OrganizationMemberInvitation>
                        (
                            (organizationMemberInvitation, invitation, organization, user, person, role) =>
                            {
                                organizationMemberInvitation.Invitation = invitation;
                                organizationMemberInvitation.Organization = organization;
                                organizationMemberInvitation.User = user;
                                organizationMemberInvitation.Person = person;
                                organizationMemberInvitation.Role = role;

                                organizationMemberInvitations.Add(organizationMemberInvitation);

                                return organizationMemberInvitation;
                            },
                            splitOn: "Id, Id, Id, Id, Id, Id"
                        );
                    },
                    context
                );

            base.__FixPaginationResults(ref items, organizationMemberInvitations, paginationFilter);

            return items;
        }

        private void AddMemberToOrganization(OrganizationMember organizationMember, ITransactionalContext? context = null)
        {
            string sql = @" INSERT INTO [Organization_Member]
                                ( IdOrganization, IdUser, IdRole, IsActive, JoinDate )
                            VALUES
                                ( @idOrganization, @idUser, @idRole, @isActive, @joinDate )";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("idOrganization", organizationMember.Organization.Id);
            QueryBuilder.AddParameter("idUser", organizationMember.User.Id);
            QueryBuilder.AddParameter("idRole", organizationMember.Role.Id);
            QueryBuilder.AddParameter("isActive", organizationMember.IsActive);
            QueryBuilder.AddParameter("joinDate", organizationMember.JoinDate);

            QueryBuilder.AddReturnLastInsertedId();

            organizationMember.Id = base.Execute<int>(context);
        }

        private int RemoveMemberFromOrganization(int id, ITransactionalContext? context = null)
        {
            string sql = @" UPDATE [Organization_Member] SET IsActive = @isActive";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("isActive", false);
            QueryBuilder.AddParameter("id", id);

            QueryBuilder.AddCondition("Id = @id");

            return base.ExecuteAndGetRowsAffected(context);
        }

        private void CreateMemberRequest(OrganizationMemberInvitation organizationMemberInvitation, ITransactionalContext? context = null)
        {
            string sql = @" INSERT INTO [Organization_MemberRequest]
                                ( IdOrganization, IdUser, IdRole, IdRequestUser, requestDate, IsPending )
                            VALUES
                                ( @idOrganization, @idUser, @idRole, @idRequestUser, @requestDate, @isPending )";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("idOrganization", organizationMemberInvitation.Organization.Id);
            QueryBuilder.AddParameter("idUser", organizationMemberInvitation.User?.Id);
            QueryBuilder.AddParameter("idRole", organizationMemberInvitation.Role.Id);
            QueryBuilder.AddParameter("idRequestUser", organizationMemberInvitation.RequestUser.Id);
            QueryBuilder.AddParameter("requestDate", organizationMemberInvitation.RequestDate);
            QueryBuilder.AddParameter("isPending", true);

            QueryBuilder.AddReturnLastInsertedId();

            organizationMemberInvitation.Id = base.Execute<int>(context);
        }

        private void UpdateMemberRequest(OrganizationMemberInvitation organizationMemberInvitation, ITransactionalContext? context = null)
        {
            string sql = @" UPDATE [Organization_MemberRequest] SET IdUser = @idUser, IsPending = @isPending";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("idUser", organizationMemberInvitation.User!.Id);
            QueryBuilder.AddParameter("isPending", false);
            QueryBuilder.AddParameter("id", organizationMemberInvitation.Id);
            QueryBuilder.AddCondition("Id = @id");

            base.Execute<int>(context);
        }

        private void RemoveMemberRequest(OrganizationMemberInvitation organizationMemberInvitation, ITransactionalContext? context = null)
        {
            string sql = @" UPDATE [Organization_MemberRequest] SET IsPending = @isPending";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("isPending", false);
            //QueryBuilder.AddParameter("idOrganization", organizationMemberInvitation.Organization.Id);
            //QueryBuilder.AddParameter("idRole", organizationMemberInvitation.Role.Id);
            //QueryBuilder.AddParameter("idUser", organizationMemberInvitation.User!.Id);
            QueryBuilder.AddParameter("id", organizationMemberInvitation.Id);

            //QueryBuilder.AddCondition("IdOrganization = @idOrganization AND IdRole = @idRole AND IdUser = @idUser");
            QueryBuilder.AddCondition("Id = @id");

            base.Execute<int>(context);
        }

        private void RemoveMemberRequestInvitation(OrganizationMemberInvitation organizationMemberInvitation, ITransactionalContext? context = null)
        {
            string sql = @" UPDATE [Organization_MemberRequestInvitation] SET IsExpired = @isExpired";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("isExpired", true);
            QueryBuilder.AddParameter("id", organizationMemberInvitation.Invitation.Id);

            QueryBuilder.AddCondition("Id = @id");

            base.Execute<int>(context);
        }

        private void CreateMemberRequestInvitation(OrganizationMemberInvitation organizationMemberInvitation, ITransactionalContext? context = null)
        {
            string sql = @" INSERT INTO [Organization_MemberRequestInvitation]
                                ( IdOrganizationMemberRequest, EmailAddress, Token, IsExpired )
                            VALUES
                                ( @idOrganizationMemberRequest, @emailAddress, @token, @isExpired )";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("idOrganizationMemberRequest", organizationMemberInvitation.Id);
            QueryBuilder.AddParameter("emailAddress", organizationMemberInvitation.Invitation.EmailAddress);
            QueryBuilder.AddParameter("token", organizationMemberInvitation.Invitation.Token);
            QueryBuilder.AddParameter("isExpired", false);

            QueryBuilder.AddReturnLastInsertedId();

            int id = base.Execute<int>(context);
        }

        private void UpdateMemberRequestInvitation(OrganizationMemberInvitation organizationMemberInvitation, ITransactionalContext? context = null)
        {
            string sql = @" UPDATE [Organization_MemberRequestInvitation] SET IsExpired = @isExpired";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("isExpired", true);
            //QueryBuilder.AddParameter("idOrganizationMemberRequest", organizationMemberInvitation.Id);
            //QueryBuilder.AddCondition("IdOrganizationMemberRequest = @idOrganizationMemberRequest");
            QueryBuilder.AddParameter("id", organizationMemberInvitation.Invitation.Id);
            QueryBuilder.AddCondition("id = @id");

            base.Execute<int>(context);
        }

        private void ProcessSearchFilter(OrganizationMemberSearchFilter searchFilter)
        {
            base.AddFilterCriteria(ConditionType.In, "Organization_Member", "Id", "id", searchFilter.Ids);
            base.AddFilterCriteria(ConditionType.Equal, "Organization_Member", "IdOrganization", "idOrganization", searchFilter.Organization?.Id);
            base.AddFilterCriteria(ConditionType.Equal, "Organization_Member", "IdUser", "idUser", searchFilter.User?.Id);
            base.AddFilterCriteria(ConditionType.Equal, "Organization_Member", "IsActive", "isActive", searchFilter.IsActive);
        }

        private void ProcessInvitationSearchFilter(OrganizationMemberInvitationSearchFilter searchFilter)
        {
            if (searchFilter == null)
                return;

            base.AddFilterCriteria(ConditionType.In, "Organization_MemberRequest", "Id", "id", searchFilter.Ids);
            base.AddFilterCriteria(ConditionType.Equal, "Organization_MemberRequest", "IdOrganization", "idOrganization", searchFilter.Organization?.Id);
            base.AddFilterCriteria(ConditionType.Equal, "Organization_MemberRequest", "IdRole", "idRole", searchFilter.Role?.Id);
            base.AddFilterCriteria(ConditionType.Equal, "Organization_MemberRequest", "IdUser", "idUser", searchFilter.User?.Id);
            base.AddFilterCriteria(ConditionType.Equal, "Organization_MemberRequest", "IsPending", "isPending", searchFilter.IsPending);
            base.AddFilterCriteria(ConditionType.Equal, "Organization_MemberRequestInvitation", "Token", "token", searchFilter.Token);
            base.AddFilterCriteria(ConditionType.Equal, "Organization_MemberRequestInvitation", "IsExpired", "isExpired", searchFilter.IsExpired);
        }

        #endregion
    }
}

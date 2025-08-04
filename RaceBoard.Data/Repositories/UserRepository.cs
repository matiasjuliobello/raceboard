using Dapper;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data.Helpers.Interfaces;
using RaceBoard.Data.Repositories.Base.Abstract;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;

namespace RaceBoard.Data.Repositories
{
    public class UserRepository : AbstractRepository, IUserRepository
    {
        #region Private Members

        private readonly Dictionary<string, string> _columnsMapping = new()
        {
            { "Id", "[User].Id" },
            { "Username", "[User].Username" },
            { "Password", "[User].Password"},
            { "Firstname", "[User].Firstname"},
            { "Middlename", "[User].Middlename"},
            { "Lastname", "[User].Lastname"},
            { "Email", "[User].Email"},
            { "BirthDate", "[User].BirthDate"},
            { "IsActive", "[User].IsActive"}
        };

        #endregion

        #region Constructors

        public UserRepository(IContextResolver contextResolver, IQueryBuilder queryBuilder) : base(contextResolver, queryBuilder)
        {
        }

        #endregion

        #region IUserRepository implementation

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
            return base.Exists(id, "User", "Id", context);
        }

        public bool ExistsDuplicate(User user, ITransactionalContext? context = null)
        {
            string existsQuery = base.GetExistsDuplicateQuery("[User]", "[Username] = @username", "Id", "@id");

            QueryBuilder.AddCommand(existsQuery);
            QueryBuilder.AddParameter("username", user.Username);
            QueryBuilder.AddParameter("id", user.Id);

            return base.Execute<bool>(context);
        }

        public PaginatedResult<User> Get(UserSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            return this.GetUsers(searchFilter: searchFilter, paginationFilter: paginationFilter, sorting: sorting, context: context);
        }

        public User GetById(int id, ITransactionalContext? context = null)
        {
            var paginatedResult = this.GetUsers(searchFilter: new UserSearchFilter() { Ids = new int[] { id } }, paginationFilter: null, sorting: null, context: context);

            return paginatedResult.Results.FirstOrDefault();
        }

        public User GetByUsername(string username, ITransactionalContext? context = null)
        {
            var paginatedResult = this.GetUsers(searchFilter: new UserSearchFilter() { Username = username }, paginationFilter: null, sorting: null, context: context);

            return paginatedResult.Results.FirstOrDefault();
        }

        public User GetByEmailAddress(string emailAddress, ITransactionalContext? context = null)
        {
            var paginatedResult = this.GetUsers(searchFilter: new UserSearchFilter() { EmailAddress = emailAddress }, paginationFilter: null, sorting: null, context: context);

            return paginatedResult.Results.FirstOrDefault();
        }

        public void Create(User user, ITransactionalContext? context = null)
        {
            this.CreateUser(user, context);
        }

        public void Update(User user, ITransactionalContext? context = null)
        {
            this.UpdateUser(user, context);
        }

        public int Delete(int id, ITransactionalContext? context = null)
        {
            return base.Delete("[User]", id, "Id", context);
        }

        public void SavePassword(UserPassword userPassword, ITransactionalContext? context = null)
        {
            string sql = "UPDATE [User] SET [Password] = @password";

            QueryBuilder.AddCommand(sql);
            QueryBuilder.AddParameter("idUser", userPassword.IdUser);
            QueryBuilder.AddParameter("password", userPassword.Password);
            QueryBuilder.AddCondition("Id = @idUser");

            base.ExecuteAndGetRowsAffected(context);
        }

        #endregion

        #region Private Methods

        private PaginatedResult<User> GetUsers(UserSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            string sql = $@"SELECT
                                [User].Id [Id],
                                [User].Username [Username],
                                [User].Password [Password],
                                [User].Email [Email],
                                [User].IsActive [IsActive],
                                [User_Role].Id [Id],
                                [Role].Id [Id],
                                [Role].Name [Name]
                            FROM [User] [User]
                            LEFT JOIN [User_Role] [User_Role] ON [User_Role].IdUser = [User].Id
                            LEFT JOIN [Role] [Role] ON [Role].Id = [User_Role].IdRole";

            QueryBuilder.AddCommand(sql);

            ProcessSearchFilter(searchFilter);

            QueryBuilder.AddSorting(sorting, _columnsMapping);
            QueryBuilder.AddPagination(paginationFilter);

            var users = new List<User>();

            PaginatedResult<User> items = base.GetPaginatedResults<User>
                (
                    (reader) =>
                    {
                        return reader.Read<User, UserRole, Role, User>
                        (
                            (user, userRole, role) =>
                            {
                                userRole.Role = role;
                                
                                user.UserRole = userRole;

                                users.Add(user);

                                return user;
                            },
                            splitOn: "Id, Id, Id"
                        ).AsList();
                    },
                    context
                );

            items.Results = users;

            return items;
        }

        private void ProcessSearchFilter(UserSearchFilter? searchFilter = null)
        {
            if (searchFilter == null)
                return;

            base.AddFilterCriteria(ConditionType.In, "[User]", "Id", "ids", searchFilter.Ids);
            base.AddFilterCriteria(ConditionType.Like, "[User]", "Username", "username", searchFilter.Username);
            base.AddFilterCriteria(ConditionType.Like, "[User]", "Email", "email", searchFilter.EmailAddress);
        }

        private void CreateUser(User user, ITransactionalContext? context = null)
        {
            string sql = @" INSERT INTO [User]
                                ( Username, [Password], Email, IsActive )
                            VALUES
                                ( @username, @password, @email, @isActive )";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("username", user.Username);
            QueryBuilder.AddParameter("password", user.Password);
            QueryBuilder.AddParameter("email", user.Email);
            QueryBuilder.AddParameter("isActive", user.IsActive);

            QueryBuilder.AddReturnLastInsertedId();

            user.Id = base.Execute<int>(context);
        }

        private void UpdateUser(User user, ITransactionalContext? context = null)
        {
            string sql = @" UPDATE [User] SET
                                IsActive = @isActive";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("isActive", user.IsActive);

            QueryBuilder.AddParameter("id", user.Id);
            QueryBuilder.AddCondition("Id = @id");

            base.ExecuteAndGetRowsAffected(context);
        }

        #endregion
    }
}
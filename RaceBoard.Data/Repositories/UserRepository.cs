using Dapper;
using Microsoft.Extensions.Configuration;
using RaceBoard.Common.Helpers.Interfaces;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data.Helpers.Interfaces;
using RaceBoard.Data.Repositories.Base.Abstract;
using RaceBoard.Data.Repositories.Base.Interfaces;
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

        public PaginatedResult<User> Get(UserSearchFilter searchFilter, PaginationFilter paginationFilter, Sorting sorting, ITransactionalContext? context = null)
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

        public bool Exists(int id, ITransactionalContext? context = null)
        {
            string existsQuery = base.GetExistsQuery("[User]", "[Id] = @id");

            QueryBuilder.AddCommand(existsQuery);
            QueryBuilder.AddParameter("id", id);

            return base.Execute<bool>(context);
        }

        public bool ExistsDuplicate(User user, ITransactionalContext? context = null)
        {
            string existsQuery = base.GetExistsDuplicateQuery("[User]", "[Username] = @username", "Id", "@id");

            QueryBuilder.AddCommand(existsQuery);
            QueryBuilder.AddParameter("username", user.Username);
            QueryBuilder.AddParameter("id", user.Id);

            return base.Execute<bool>(context);
        }

        #endregion

        #region Private Methods

        private PaginatedResult<User> GetUsers(UserSearchFilter searchFilter, PaginationFilter paginationFilter, Sorting sorting, ITransactionalContext? context = null)
        {
            string sql = $@"SELECT
                                [User].Id [Id],
                                [User].Username [Username],
                                [User].Password [Password],
                                [User].Email [Email]
                            FROM [User] [User]";

            QueryBuilder.AddCommand(sql);

            ProcessSearchFilter(searchFilter);

            QueryBuilder.AddSorting(sorting, _columnsMapping);
            QueryBuilder.AddPagination(paginationFilter);

            return base.GetMultipleResultsWithPagination<User>(context);
        }

        private void ProcessSearchFilter(UserSearchFilter searchFilter)
        {
            if (searchFilter.Ids != null && searchFilter.Ids.Length > 0)
            {
                QueryBuilder.AddCondition($"[User].Id IN @ids");
                QueryBuilder.AddParameter("ids", searchFilter.Ids);
            }

            if (!string.IsNullOrEmpty(searchFilter.Username))
            {
                QueryBuilder.AddCondition($"[User].Username LIKE {AddLikeWildcards("@username")}");
                QueryBuilder.AddParameter("username", searchFilter.Username);
            }

            if (!string.IsNullOrEmpty(searchFilter.EmailAddress))
            {
                QueryBuilder.AddCondition($"[User].Email LIKE {AddLikeWildcards("@email")}");
                QueryBuilder.AddParameter("email", searchFilter.EmailAddress);
            }
        }

        private void CreateUser(User user, ITransactionalContext? context = null)
        {
            string sql = @" INSERT INTO [User]
                                ( Username, [Password], Firstname, Middlename, Lastname, Email, BirthDate, IsActive )
                            VALUES
                                ( @username, @password, @firstname, @middlename, @lastname, @email, @birthDate, @isActive )";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("username", user.Username);
            QueryBuilder.AddParameter("password", user.Password);
            QueryBuilder.AddParameter("firstname", user.Firstname);
            QueryBuilder.AddParameter("middlename", user.Middlename);
            QueryBuilder.AddParameter("lastname", user.Lastname);
            QueryBuilder.AddParameter("email", user.Email);
            QueryBuilder.AddParameter("birthDate", user.BirthDate.UtcDateTime.Date);
            QueryBuilder.AddParameter("isActive", user.IsActive);

            QueryBuilder.AddReturnLastInsertedId();

            user.Id = base.Execute<int>(context);
        }

        private void UpdateUser(User user, ITransactionalContext? context = null)
        {
            string sql = @" UPDATE [User] SET
                                Firstname = @firstname, 
                                Middlename = @middlename,
                                Lastname = @lastname,
                                BirthDate = @birthDate,
                                IsActive = @isActive";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("firstname", user.Firstname);
            QueryBuilder.AddParameter("middlename", user.Middlename);
            QueryBuilder.AddParameter("lastname", user.Lastname);
            QueryBuilder.AddParameter("birthDate", user.BirthDate);
            QueryBuilder.AddParameter("isActive", user.IsActive);

            QueryBuilder.AddParameter("id", user.Id);
            QueryBuilder.AddCondition("Id = @id");

            base.ExecuteAndGetRowsAffected(context);
        }


        #endregion
    }
}
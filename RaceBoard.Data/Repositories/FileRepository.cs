using Dapper;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data.Helpers.Interfaces;
using RaceBoard.Data.Repositories.Base.Abstract;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;

namespace RaceBoard.Data.Repositories
{
    public class FileRepository : AbstractRepository, IFileRepository
    {
        #region Private Members

        private readonly Dictionary<string, string> _columnsMapping = new()
        {
            { "Id", "[File].Id" },
            { "Description", "[File].Description" },
            { "Name", "[File].Name" },
            { "CreationDate", "[File].CreationDate"}
        };

        #endregion

        #region Constructors

        public FileRepository(IContextResolver contextResolver, IQueryBuilder queryBuilder) : base(contextResolver, queryBuilder)
        {
        }

        #endregion

        #region IFileRepository implementation

        public PaginatedResult<Domain.File> Get(FileSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            return this.GetFiles(searchFilter: searchFilter, paginationFilter: paginationFilter, sorting: sorting, context: context);
        }

        public Domain.File? Get(int id, ITransactionalContext? context = null)
        {
            var searchFilter = new FileSearchFilter() {  Ids = new int[] { id } };

            return this.GetFiles(searchFilter, paginationFilter: null, sorting: null, context: context).Results.FirstOrDefault();
        }

        public void Create(Domain.File file, ITransactionalContext? context = null)
        {
            this.CreateFile(file, context);
        }

        public int Delete(int id, ITransactionalContext? context = null)
        {
            return this.DeleteFile(id, context);
        }

        public int Delete(int[] ids, ITransactionalContext? context = null)
        {
            return this.DeleteFiles(ids, context);
        }

        #endregion

        #region Private Methods

        private PaginatedResult<Domain.File> GetFiles(FileSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            string sql = $@"SELECT
	                            [File].Id [Id],	  
	                            [File].[Description] [Description],
	                            [File].[Name] [Name],
                                [File].Path [Path],
	                            [File].CreationDate [CreationDate],
	                            [User].Id [Id],
                                [Person].Id [Id],
	                            [Person].Firstname [Firstname],
	                            [Person].Lastname [Lastname]
                            FROM [File] [File]
                            INNER JOIN [User] [User] ON [User].Id = [File].IdCreationUser
                            INNER JOIN [User_Person] [User_Person] ON [User_Person].IdUser = [User].Id
                            INNER JOIN [Person] [Person] ON [Person].Id = [User_Person].IdPerson";

            QueryBuilder.AddCommand(sql);

            this.ProcessSearchFilter(searchFilter);

            //QueryBuilder.AddSorting(sorting, _columnsMapping);
            QueryBuilder.AddPagination(paginationFilter);

            var files = new List<Domain.File>();

            PaginatedResult<Domain.File> items = base.GetPaginatedResults<Domain.File>
                (
                    (reader) =>
                    {
                        return reader.Read<Domain.File, User, Person, Domain.File>
                        (
                            (file, user, person) =>
                            {
                                person.User = user;

                                file.CreationUser = user;

                                files.Add(file);

                                return file;
                            },
                            splitOn: "Id, Id, Id"
                        ).AsList();
                    },
                    context
                );

            items.Results = files;

            return items;
        }

        private void CreateFile(Domain.File file, ITransactionalContext? context = null)
        {
            string sql = @" INSERT INTO [File]
                                ( [Description], [Name], [Path], IdCreationUser, CreationDate )
                            VALUES
                                ( @description, @name, @path, @idCreationUser, @creationDate )";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("description", file.Description);
            QueryBuilder.AddParameter("name", file.Name);
            QueryBuilder.AddParameter("path", file.Path);
            QueryBuilder.AddParameter("idCreationUser", file.CreationUser.Id);
            QueryBuilder.AddParameter("creationDate", file.CreationDate);

            QueryBuilder.AddReturnLastInsertedId();

            file.Id = base.Execute<int>(context);
        }

        private int DeleteFile(int id, ITransactionalContext? context = null)
        {
            return base.Delete("[File]", id, idColumnName: "Id", context: context);
        }
        private int DeleteFiles(int[] ids, ITransactionalContext? context = null)
        {
            return base.Delete("[File]", ids, idColumnName: "Id", context: context);
        }

        private void ProcessSearchFilter(FileSearchFilter? searchFilter)
        {
            if (searchFilter == null)
                return;

            //if (searchFilter.Provider != null)
            //{
            //    string name = searchFilter.Provider.Firstname;

            //QueryBuilder.AddCondition($"([User].Firstname LIKE {AddLikeWildcards("@provider")} OR [User].Lastname LIKE {AddLikeWildcards("@provider")})");
            //    QueryBuilder.AddParameter("provider", name);
            //}

            //if (searchFilter.Type != null)
            //{
            //    QueryBuilder.AddCondition($"[File].IdType = @idType");
            //    QueryBuilder.AddParameter("idType", searchFilter.Type.Id);
            //}

            //if (searchFilter.StartDate.HasValue)
            //{
            //    //QueryBuilder.AddCondition($"[File].StartDate >= @startDate");
            //    QueryBuilder.AddCondition(GetBetweenDatesCondition("@startDate", "StartDate", "EndDate", false, true));
            //    QueryBuilder.AddParameter("startDate", searchFilter.StartDate.Value.Date);
            //}

            //if (searchFilter.EndDate.HasValue)
            //{
            //    QueryBuilder.AddCondition($"[File].EndDate <= @endDate");
            //    QueryBuilder.AddParameter("endDate", searchFilter.EndDate.Value.Date);
            //}

            base.AddFilterCriteria(ConditionType.In, "[File]", "Id", "id", searchFilter.Ids);
        }

        #endregion
    }
}

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
            { "PhysicalName", "[File].PhysicalName" },
            { "IdCreationUser", "[File].IdCreationUser"},
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
            return this.GetFiles(id: null, searchFilter: searchFilter, paginationFilter: paginationFilter, sorting: sorting, context: context);
        }

        public Domain.File? Get(int id, ITransactionalContext? context = null)
        {
            return this.GetFiles(id: id, searchFilter: null, paginationFilter: null, sorting: null, context: context).Results.FirstOrDefault();
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

        private PaginatedResult<Domain.File> GetFiles(int? id, FileSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            string sql = $@"SELECT
	                            [File].Id [Id],	  
	                            [File].[UniqueFileName] [UniqueFileName],
	                            [File].[FileName] [FileName],
                                [File].Path [Path],
	                            [File].StartDate [StartDate],
	                            [File].EndDate [EndDate],
	                            [User].Id [Id],
	                            [User].Firstname [Firstname],
	                            [User].Lastname [Lastname],
	                            [FileType].Id [Id],
	                            [FileType].Name [Name],
	                            [Role].Id [Id],
	                            [Role].Name [Name]
                            FROM [File] [File]
                            INNER JOIN [FileType] [FileType] ON [FileType].Id = [File].IdType
                            INNER JOIN [Role] [Role] ON [Role].Id = [File].IdRole";

            QueryBuilder.AddCommand(sql);

            if (id.HasValue)
            {
                QueryBuilder.AddCondition($"[File].Id = @id");
                QueryBuilder.AddParameter("id", id.Value);
            }

            this.ProcessSearchFilter(searchFilter);

            //QueryBuilder.AddSorting(sorting, _columnsMapping);
            QueryBuilder.AddPagination(paginationFilter);

            var files = new List<Domain.File>();

            PaginatedResult<Domain.File> items = base.GetPaginatedResults<Domain.File>
                (
                    (reader) =>
                    {
                        return reader.Read<Domain.File, User, FileType, Domain.File>
                        (
                            (file, user, fileType) =>
                            {
                                //var userFile = files.FirstOrDefault(x => x.User.Id == user.Id && x.StartDate == file.StartDate);
                                //if (userFile == null)
                                //{
                                //    userFile = file;
                                //    userFile.User = user;

                                //    files.Add(userFile);
                                //}

                                //userFile.Type = fileType;

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

            //base.AddFilterCriteria(ConditionType.In, "File", "Id", "id", searchFilter.Ids);
        }

        #endregion
    }
}

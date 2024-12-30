using Dapper;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data.Helpers.Interfaces;
using RaceBoard.Data.Helpers.SqlBulkHelper;
using RaceBoard.Data.Repositories.Base.Abstract;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;
using File = RaceBoard.Domain.File;

namespace RaceBoard.Data.Repositories
{
    public class ChampionshipFileRepository : AbstractRepository, IChampionshipFileRepository
    {
        public class BulkChampionshipFileRaceClass
        {
            public int IdChampionshipFile { get; set; }
            public int IdRaceClass { get; set; }

            public BulkChampionshipFileRaceClass(int idChampionshipFile, int idRaceClass)
            {
                this.IdChampionshipFile = idChampionshipFile;
                this.IdRaceClass = idRaceClass;
            }
        }

        #region Private Members

        private readonly Dictionary<string, string> _columnsMapping = new()
        {
            { "Id", "[Championship_File].Id" },
            { "File.Name", "[File].Name" },
            { "File.CreationDate", "[File].CreationDate" }
        };

        private readonly ISqlBulkInsertHelper _bulkInsertHelper;

        #endregion

        #region Constructors

        public ChampionshipFileRepository
            (
                IContextResolver contextResolver,
                IQueryBuilder queryBuilder,
                ISqlBulkInsertHelper bulkInsertHelper
            ) : base(contextResolver, queryBuilder)
        {
            _bulkInsertHelper = bulkInsertHelper;
        }

        #endregion

        #region IChampionshipFileRepository implementation

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
            return base.Exists(id, "Championship_File", "Id", context);
        }

        public bool ExistsDuplicate(ChampionshipFile championshipFile, ITransactionalContext? context = null)
        {
            return false;
        }

        public PaginatedResult<ChampionshipFile> Get(ChampionshipFileSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            return this.GetChampionshipFiles(searchFilter, paginationFilter, sorting, context);
        }

        public void Create(ChampionshipFile championshipFile, ITransactionalContext? context = null)
        {
            string sql = @" INSERT INTO [Championship_File]
                                ( IdChampionship, IdFile, IdFileType )
                            VALUES
                                ( @idChampionship, @idFile, @idFileType )";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("idChampionship", championshipFile.Championship.Id);
            QueryBuilder.AddParameter("idFile", championshipFile.File.Id);
            QueryBuilder.AddParameter("idFileType", championshipFile.FileType.Id);

            QueryBuilder.AddReturnLastInsertedId();

            championshipFile.Id = base.Execute<int>(context);
        }

        public void AssociateRaceClasses(ChampionshipFile championshipFile, ITransactionalContext? context = null)
        {
            if (championshipFile.RaceClasses == null || championshipFile.RaceClasses.Count == 0)
                return;

            var bulkItems = new List<BulkChampionshipFileRaceClass>();

            championshipFile.RaceClasses.ForEach(x => { bulkItems.Add(new BulkChampionshipFileRaceClass(championshipFile.Id, x.Id)); });

            var sqlBulkSettings = new SqlBulkSettings<BulkChampionshipFileRaceClass>();
            sqlBulkSettings.TableName = "Championship_File_RaceClass";
            sqlBulkSettings.Mappings.Add(new SqlBulkColumnMapping(nameof(BulkChampionshipFileRaceClass.IdChampionshipFile), "IdChampionshipFile"));
            sqlBulkSettings.Mappings.Add(new SqlBulkColumnMapping(nameof(BulkChampionshipFileRaceClass.IdRaceClass), "IdRaceClass"));
            sqlBulkSettings.Data = bulkItems;

            _bulkInsertHelper.PerformBulkInsert(sqlBulkSettings, context);
        }

        public int Delete(int id, ITransactionalContext? context = null)
        {
            return base.Delete("[Championship_File]", id, "Id", context);
        }

        public int DeleteRaceClasses(int id, ITransactionalContext? context = null)
        {
            return base.Delete("[Championship_File_RaceClass]", id, "IdChampionshipFile", context);
        }

        #endregion

        #region Private Methods

        private PaginatedResult<ChampionshipFile> GetChampionshipFiles(ChampionshipFileSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            string sql = $@"SELECT
	                            [Championship_File].Id   [Id],
                                [Championship].Id        [Id],
                                [Championship].[Name]    [Name],
	                            [File].Id               [Id],
	                            [File].[Description]    [Description],
	                            [File].[Name]           [Name],
	                            [File].[Path]           [Path],
	                            [File].CreationDate     [CreationDate],
	                            [FileType].Id           [Id],
	                            [FileType].[Name]       [Name],
	                            [User].Id               [Id],
                                [Person].Id             [Id],
                                [Person].Firstname      [Firstname],
	                            [Person].Lastname       [Lastname],
	                            [RaceClass].Id          [Id],
	                            [RaceClass].[Name]      [Name]
                            FROM [Championship_File] 
                            INNER JOIN [Championship] [Championship] ON [Championship].Id = [Championship_File].IdChampionship
                            INNER JOIN [File] ON [File].Id = [Championship_File].IdFile
                            INNER JOIN [FileType] ON [FileType].Id = [Championship_File].IdFileType
                            LEFT JOIN [Championship_File_RaceClass] ON [Championship_File_RaceClass].IdChampionshipFile = [Championship_File].Id
                            LEFT JOIN [RaceClass] [RaceClass] ON [RaceClass].Id = [Championship_File_RaceClass].IdRaceClass
                            LEFT JOIN [User] [User] ON [User].Id = [File].IdCreationUser
                            LEFT JOIN [User_Person] [User_Person] ON [User_Person].IdUser = [User].Id
                            LEFT JOIN [Person] [Person] ON [Person].Id = [User_Person].IdPerson";

            QueryBuilder.AddCommand(sql);

            ProcessSearchFilter(searchFilter);

            QueryBuilder.AddSorting(sorting, _columnsMapping);
            QueryBuilder.AddPagination(paginationFilter);

            var championshipFiles = new List<ChampionshipFile>();

            PaginatedResult<ChampionshipFile> items = base.GetPaginatedResults<ChampionshipFile>
                (
                    (reader) =>
                    {
                        return reader.Read<ChampionshipFile, Championship, File, FileType, User, Person, RaceClass, ChampionshipFile>
                        (
                            (championshipFile, championship, file, fileType, user, person, raceClass) =>
                            {
                                var existingChampionshipFile = championshipFiles.FirstOrDefault(x => x.Id == championshipFile.Id);
                                if (existingChampionshipFile == null)
                                {
                                    championshipFiles.Add(championshipFile);
                                    championshipFile.Championship = championship;
                                }
                                else
                                {
                                    championshipFile = existingChampionshipFile;
                                }
                                championshipFile.RaceClasses.Add(raceClass);

                                file.CreationPerson = person;
                                file.CreationUser = user;

                                championshipFile.FileType = fileType;

                                championshipFile.File = file;

                                return championshipFile;
                            },
                            splitOn: "Id, Id, Id, Id, Id, Id, Id"
                        ).AsList();
                    },
                    context
                );

            items.Results = championshipFiles;

            return items;
        }

        private void ProcessSearchFilter(ChampionshipFileSearchFilter? searchFilter = null)
        {
            if (searchFilter == null)
                return;

            base.AddFilterCriteria(ConditionType.In, "Championship_File", "Id", "id", searchFilter.Ids);
            base.AddFilterCriteria(ConditionType.Equal, "Championship", "Id", "idChampionship", searchFilter.Championship?.Id);
            base.AddFilterCriteria(ConditionType.Equal, "FileType", "Id", "idFileType", searchFilter.FileType?.Id);
            base.AddFilterCriteria(ConditionType.In, "Championship_File_RaceClass", "IdRaceClass", "idRaceClass", searchFilter.RaceClasses);
        }

        #endregion
    }
}

using Dapper;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data.Helpers.Interfaces;
using RaceBoard.Data.Repositories.Base.Abstract;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;
using System;
using File = RaceBoard.Domain.File;

namespace RaceBoard.Data.Repositories
{
    public class CompetitionFileRepository : AbstractRepository, ICompetitionFileRepository
    {
        #region Private Members

        private readonly Dictionary<string, string> _columnsMapping = new()
        {
            { "Id", "[Competition_File].Id" },
            { "File.Name", "[File].Name" },
            { "File.CreationDate", "[File].CreationDate" }
        };

        #endregion

        #region Constructors

        public CompetitionFileRepository(IContextResolver contextResolver, IQueryBuilder queryBuilder) : base(contextResolver, queryBuilder)
        {
        }

        #endregion

        #region ICompetitionFileRepository implementation

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
            return base.Exists(id, "Competition_NewsUpdate", "Id", context);
        }

        public bool ExistsDuplicate(CompetitionFile competitionFile, ITransactionalContext? context = null)
        {
            return false;
        }

        public PaginatedResult<CompetitionFile> Get(CompetitionFileSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            return this.GetCompetitionFiles(searchFilter, paginationFilter, sorting, context);
        }

        public void Create(CompetitionFile competitionFile, ITransactionalContext? context = null)
        {
            string sql = @" INSERT INTO [Competition_File]
                                ( IdCompetition, IdFile, IdFileType )
                            VALUES
                                ( @idCompetition, @idFile, @idFileType )";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("idCompetition", competitionFile.Competition.Id);
            QueryBuilder.AddParameter("idFile", competitionFile.File.Id);
            QueryBuilder.AddParameter("idFileType", competitionFile.FileType.Id);

            QueryBuilder.AddReturnLastInsertedId();

            competitionFile.Id = base.Execute<int>(context);
        }

        public void AssociateRaceClasses(CompetitionFile competitionFile, ITransactionalContext? context = null)
        {
            if (competitionFile.RaceClasses == null)
                return;

            string sql = @" INSERT INTO [Competition_File_RaceClass]
                                ( IdCompetitionFile, IdRaceClass )
                            VALUES
                                ( @idCompetitionFile, @idRaceClass )";

            foreach (var raceClass in competitionFile.RaceClasses)
            {
                QueryBuilder.Clear();

                QueryBuilder.AddCommand(sql);

                QueryBuilder.AddParameter("idCompetitionFile", competitionFile.Id);
                QueryBuilder.AddParameter("idRaceClass", raceClass.Id);

                QueryBuilder.AddReturnLastInsertedId();

                int id = base.Execute<int>(context);
            }
        }

        public int Delete(int id, ITransactionalContext? context = null)
        {
            int affectedChildRecords = base.Delete("[Competition_File_RaceClass]", id, "IdCompetitionFile", context);

            int affectedRecords = base.Delete("[Competition_File]", id, "Id", context);

            return affectedRecords;
        }

        #endregion

        #region Private Methods

        private PaginatedResult<CompetitionFile> GetCompetitionFiles(CompetitionFileSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            string sql = $@"SELECT
	                            [Competition_File].Id   [Id],
                                [Competition].Id        [Id],
                                [Competition].[Name]    [Name],
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
                            FROM [Competition_File] 
                            INNER JOIN [Competition] [Competition] ON [Competition].Id = [Competition_File].IdCompetition
                            INNER JOIN [File] ON [File].Id = [Competition_File].IdFile
                            INNER JOIN [FileType] ON [FileType].Id = [Competition_File].IdFileType
                            LEFT JOIN [Competition_File_RaceClass] ON [Competition_File_RaceClass].IdCompetitionFile = [Competition_File].Id
                            LEFT JOIN [RaceClass] [RaceClass] ON [RaceClass].Id = [Competition_File_RaceClass].IdRaceClass
                            LEFT JOIN [User] [User] ON [User].Id = [File].IdCreationUser
                            LEFT JOIN [User_Person] [User_Person] ON [User_Person].IdUser = [User].Id
                            LEFT JOIN [Person] [Person] ON [Person].Id = [User_Person].IdPerson";

            QueryBuilder.AddCommand(sql);

            ProcessSearchFilter(searchFilter);

            QueryBuilder.AddSorting(sorting, _columnsMapping);
            QueryBuilder.AddPagination(paginationFilter);

            var competitionFiles = new List<CompetitionFile>();

            PaginatedResult<CompetitionFile> items = base.GetPaginatedResults<CompetitionFile>
                (
                    (reader) =>
                    {
                        return reader.Read<CompetitionFile, Competition, File, FileType, User, Person, RaceClass, CompetitionFile>
                        (
                            (competitionFile, competition, file, fileType, user, person, raceClass) =>
                            {
                                var existingCompetitionFile = competitionFiles.FirstOrDefault(x => x.Id == competitionFile.Id);
                                if (existingCompetitionFile == null)
                                {
                                    competitionFiles.Add(competitionFile);
                                    competitionFile.Competition = competition;
                                }
                                else
                                {
                                    competitionFile = existingCompetitionFile;
                                }
                                competitionFile.RaceClasses.Add(raceClass);

                                file.CreationPerson = person;
                                file.CreationUser = user;

                                competitionFile.FileType = fileType;

                                competitionFile.File = file;

                                return competitionFile;
                            },
                            splitOn: "Id, Id, Id, Id, Id, Id, Id"
                        ).AsList();
                    },
                    context
                );

            items.Results = competitionFiles;

            return items;
        }

        private void ProcessSearchFilter(CompetitionFileSearchFilter? searchFilter = null)
        {
            if (searchFilter == null)
                return;

            base.AddFilterCriteria(ConditionType.In, "Competition_File", "Id", "id", searchFilter.Ids);
            base.AddFilterCriteria(ConditionType.Equal, "Competition", "Id", "idCompetition", searchFilter.Competition?.Id);
            base.AddFilterCriteria(ConditionType.Equal, "FileType", "Id", "idFileType", searchFilter.FileType?.Id);
            base.AddFilterCriteria(ConditionType.In, "Competition_File_RaceClass", "IdRaceClass", "idRaceClass", searchFilter.RaceClasses);
        }

        #endregion
    }
}

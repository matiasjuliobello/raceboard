using Dapper;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data.Helpers.Interfaces;
using RaceBoard.Data.Repositories.Base.Abstract;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;
using System.Text;

namespace RaceBoard.Data.Repositories
{
    public class CompetitionNotificationRepository : AbstractRepository, ICompetitionNotificationRepository
    {
        #region Private Members

        private readonly Dictionary<string, string> _columnsMapping = new()
        {
            { "Id", "[Competition_Notification].Id" },
            { "Title", "[Competition_Notification].Title" },
            { "Message", "[Competition_Notification].Message" },
            { "IdCreationUser", "[Competition_Notification].IdCreationUser"},
            { "CreationDate", "[Competition_Notification].CreationDate"}
        };

        #endregion

        #region Constructors

        public CompetitionNotificationRepository(IContextResolver contextResolver, IQueryBuilder queryBuilder) : base(contextResolver, queryBuilder)
        {
        }

        #endregion

        #region ICompetitionNotificationRepository implementation

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
            return base.Exists(id, "Competition_Notification", "Id", context);
        }

        public bool ExistsDuplicate(CompetitionNotification competitionNotification, ITransactionalContext? context = null)
        {
            return false;
        }

        public PaginatedResult<CompetitionNotification> Get(CompetitionNotificationSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            return this.GetCompetitionNotifications(searchFilter, paginationFilter, sorting, context);
        }

        public void Create(CompetitionNotification competitionNotification, ITransactionalContext? context = null)
        {
            string sql = @" INSERT INTO [Competition_Notification]
                                ( IdCompetition, Title, Message, Timestamp, IdCreationUser, CreationDate )
                            VALUES
                                ( @idCompetition, @title, @message, @timestamp, @idCreationUser, @creationDate )";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("idCompetition", competitionNotification.Competition.Id);
            QueryBuilder.AddParameter("title", competitionNotification.Title);
            QueryBuilder.AddParameter("message", competitionNotification.Message);
            QueryBuilder.AddParameter("idCreationUser", competitionNotification.CreationUser.Id);
            QueryBuilder.AddParameter("creationDate", competitionNotification.CreationDate);

            QueryBuilder.AddReturnLastInsertedId();

            competitionNotification.Id = base.Execute<int>(context);
        }

        public void AssociateRaceClasses(CompetitionNotification competitionNotification, ITransactionalContext? context = null)
        {
            if (competitionNotification.RaceClasses == null)
                return;

            string sql = @" INSERT INTO [Competition_Notification_RaceClass]
                                ( IdCompetitionNotification, IdRaceClass )
                            VALUES
                                ( @idCompetitionNotification, @idRaceClass )";

            foreach (var raceClass in competitionNotification.RaceClasses)
            {
                QueryBuilder.Clear();

                QueryBuilder.AddCommand(sql);

                QueryBuilder.AddParameter("idCompetitionNotification", competitionNotification.Id);
                QueryBuilder.AddParameter("idRaceClass", raceClass.Id);

                QueryBuilder.AddReturnLastInsertedId();

                int id = base.Execute<int>(context);
            }
        }

        public int Delete(int id, ITransactionalContext? context = null)
        {
            return base.Delete("[Competition_Notification]", id, "Id", context);
        }

        public int DeleteRaceClasses(int id, ITransactionalContext? context = null)
        {
            return base.Delete("[Competition_Notification_RaceClass]", id, "IdCompetitionNotification", context);
        }

        #endregion

        #region Private Methods

        private PaginatedResult<CompetitionNotification> GetCompetitionNotifications(CompetitionNotificationSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            var query = new StringBuilder();

            query.AppendLine($@"SELECT
                                [Competition_Notification].Id           [Id],");

            if (searchFilter?.Ids?.Length > 0 )
                query.AppendLine("[Competition_Notification].Message    [Message],");

            query.AppendLine(@"
                                [Competition_Notification].Title        [Title],
                                [Competition_Notification].CreationDate [CreationDate],
                                [Competition].Id                        [Id],
                                [Competition].[Name]                    [Name],
	                            [User].Id                               [Id],
                                [Person].Id                             [Id],
                                [Person].Firstname                      [Firstname],
	                            [Person].Lastname                       [Lastname],
	                            [RaceClass].Id                          [Id],
	                            [RaceClass].[Name]                      [Name]
                            FROM [Competition_Notification] [Competition_Notification]
                            INNER JOIN [Competition] [Competition] ON [Competition].Id = [Competition_Notification].IdCompetition
                            LEFT JOIN [Competition_Notification_RaceClass] ON [Competition_Notification_RaceClass].IdCompetitionNotification = [Competition_Notification].Id
                            LEFT JOIN [RaceClass] [RaceClass] ON [RaceClass].Id = [Competition_Notification_RaceClass].IdRaceClass
                            LEFT JOIN [User] [User] ON [User].Id = [Competition_Notification].IdCreationUser
                            LEFT JOIN [User_Person] [User_Person] ON [User_Person].IdUser = [User].Id
                            LEFT JOIN [Person] [Person] ON [Person].Id = [User_Person].IdPerson");

            QueryBuilder.AddCommand(query.ToString());

            ProcessSearchFilter(searchFilter);

            QueryBuilder.AddSorting(sorting, _columnsMapping);
            QueryBuilder.AddPagination(paginationFilter);

            var competitionNotifications = new List<CompetitionNotification>();

            PaginatedResult<CompetitionNotification> items = base.GetPaginatedResults<CompetitionNotification>
                (
                    (reader) =>
                    {
                        return reader.Read<CompetitionNotification, Competition, User, Person, RaceClass, CompetitionNotification>
                        (
                            (competitionNotification, competition, user, person, raceClass) =>
                            {
                                var existingCompetitionNotification = competitionNotifications.FirstOrDefault(x => x.Id == competitionNotification.Id);
                                if (existingCompetitionNotification == null)
                                {
                                    competitionNotifications.Add(competitionNotification);
                                    competitionNotification.Competition = competition;
                                }
                                else
                                {
                                    competitionNotification = existingCompetitionNotification;
                                }
                                competitionNotification.RaceClasses.Add(raceClass);

                                competitionNotification.CreationPerson = person;
                                competitionNotification.CreationUser = user;

                                return competitionNotification;
                            },
                            splitOn: "Id, Id, Id, Id, Id"
                        ).AsList();
                    },
                    context
                );

            items.Results = competitionNotifications;

            return items;
        }

        private void ProcessSearchFilter(CompetitionNotificationSearchFilter? searchFilter = null)
        {
            if (searchFilter == null)
                return;

            base.AddFilterCriteria(ConditionType.In, "Competition_Notification", "Id", "id", searchFilter.Ids);
            base.AddFilterCriteria(ConditionType.Equal, "Competition_Notification", "IdCompetition", "idCompetition", searchFilter.Competition?.Id);
            base.AddFilterCriteria(ConditionType.In, "Competition_Notification_RaceClass", "IdRaceClass", "idRaceClass", searchFilter.RaceClasses);
        }

        #endregion
    }
}

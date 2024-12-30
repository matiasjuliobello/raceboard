using Dapper;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data.Helpers.Interfaces;
using RaceBoard.Data.Repositories.Base.Abstract;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;
using System.Text;

namespace RaceBoard.Data.Repositories
{
    public class ChampionshipNotificationRepository : AbstractRepository, IChampionshipNotificationRepository
    {
        #region Private Members

        private readonly Dictionary<string, string> _columnsMapping = new()
        {
            { "Id", "[Championship_Notification].Id" },
            { "Title", "[Championship_Notification].Title" },
            { "Message", "[Championship_Notification].Message" },
            { "IdCreationUser", "[Championship_Notification].IdCreationUser"},
            { "CreationDate", "[Championship_Notification].CreationDate"}
        };

        #endregion

        #region Constructors

        public ChampionshipNotificationRepository(IContextResolver contextResolver, IQueryBuilder queryBuilder) : base(contextResolver, queryBuilder)
        {
        }

        #endregion

        #region IChampionshipNotificationRepository implementation

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
            return base.Exists(id, "Championship_Notification", "Id", context);
        }

        public bool ExistsDuplicate(ChampionshipNotification championshipNotification, ITransactionalContext? context = null)
        {
            return false;
        }

        public PaginatedResult<ChampionshipNotification> Get(ChampionshipNotificationSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            return this.GetChampionshipNotifications(searchFilter, paginationFilter, sorting, context);
        }

        public void Create(ChampionshipNotification championshipNotification, ITransactionalContext? context = null)
        {
            string sql = @" INSERT INTO [Championship_Notification]
                                ( IdChampionship, Title, Message, IdCreationUser, CreationDate )
                            VALUES
                                ( @idChampionship, @title, @message, @idCreationUser, @creationDate )";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("idChampionship", championshipNotification.Championship.Id);
            QueryBuilder.AddParameter("title", championshipNotification.Title);
            QueryBuilder.AddParameter("message", championshipNotification.Message);
            QueryBuilder.AddParameter("idCreationUser", championshipNotification.CreationUser.Id);
            QueryBuilder.AddParameter("creationDate", championshipNotification.CreationDate);

            QueryBuilder.AddReturnLastInsertedId();

            championshipNotification.Id = base.Execute<int>(context);
        }

        public void AssociateRaceClasses(ChampionshipNotification championshipNotification, ITransactionalContext? context = null)
        {
            if (championshipNotification.RaceClasses == null || championshipNotification.RaceClasses.Count == 0)
                return;

            string sql = @" INSERT INTO [Championship_Notification_RaceClass]
                                ( IdChampionshipNotification, IdRaceClass )
                            VALUES
                                ( @idChampionshipNotification, @idRaceClass )";

            foreach (var raceClass in championshipNotification.RaceClasses)
            {
                QueryBuilder.Clear();

                QueryBuilder.AddCommand(sql);

                QueryBuilder.AddParameter("idChampionshipNotification", championshipNotification.Id);
                QueryBuilder.AddParameter("idRaceClass", raceClass.Id);

                QueryBuilder.AddReturnLastInsertedId();

                int id = base.Execute<int>(context);
            }
        }

        public int Delete(int id, ITransactionalContext? context = null)
        {
            return base.Delete("[Championship_Notification]", id, "Id", context);
        }

        public int DeleteRaceClasses(int id, ITransactionalContext? context = null)
        {
            return base.Delete("[Championship_Notification_RaceClass]", id, "IdChampionshipNotification", context);
        }

        #endregion

        #region Private Methods

        private PaginatedResult<ChampionshipNotification> GetChampionshipNotifications(ChampionshipNotificationSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            var query = new StringBuilder();

            query.AppendLine($@"SELECT
                                [Championship_Notification].Id           [Id],");

            if (searchFilter?.Ids?.Length > 0 )
                query.AppendLine("[Championship_Notification].Message    [Message],");

            query.AppendLine(@"
                                [Championship_Notification].Title        [Title],
                                [Championship_Notification].CreationDate [CreationDate],
                                [Championship].Id                        [Id],
                                [Championship].[Name]                    [Name],
	                            [User].Id                               [Id],
                                [Person].Id                             [Id],
                                [Person].Firstname                      [Firstname],
	                            [Person].Lastname                       [Lastname],
	                            [RaceClass].Id                          [Id],
	                            [RaceClass].[Name]                      [Name]
                            FROM [Championship_Notification] [Championship_Notification]
                            INNER JOIN [Championship] [Championship] ON [Championship].Id = [Championship_Notification].IdChampionship
                            LEFT JOIN [Championship_Notification_RaceClass] ON [Championship_Notification_RaceClass].IdChampionshipNotification = [Championship_Notification].Id
                            LEFT JOIN [RaceClass] [RaceClass] ON [RaceClass].Id = [Championship_Notification_RaceClass].IdRaceClass
                            LEFT JOIN [User] [User] ON [User].Id = [Championship_Notification].IdCreationUser
                            LEFT JOIN [User_Person] [User_Person] ON [User_Person].IdUser = [User].Id
                            LEFT JOIN [Person] [Person] ON [Person].Id = [User_Person].IdPerson");

            QueryBuilder.AddCommand(query.ToString());

            ProcessSearchFilter(searchFilter);

            QueryBuilder.AddSorting(sorting, _columnsMapping);
            QueryBuilder.AddPagination(paginationFilter);

            var championshipNotifications = new List<ChampionshipNotification>();

            PaginatedResult<ChampionshipNotification> items = base.GetPaginatedResults<ChampionshipNotification>
                (
                    (reader) =>
                    {
                        return reader.Read<ChampionshipNotification, Championship, User, Person, RaceClass, ChampionshipNotification>
                        (
                            (championshipNotification, championship, user, person, raceClass) =>
                            {
                                var existingChampionshipNotification = championshipNotifications.FirstOrDefault(x => x.Id == championshipNotification.Id);
                                if (existingChampionshipNotification == null)
                                {
                                    championshipNotifications.Add(championshipNotification);
                                    championshipNotification.Championship = championship;
                                }
                                else
                                {
                                    championshipNotification = existingChampionshipNotification;
                                }
                                championshipNotification.RaceClasses.Add(raceClass);

                                championshipNotification.CreationPerson = person;
                                championshipNotification.CreationUser = user;

                                return championshipNotification;
                            },
                            splitOn: "Id, Id, Id, Id, Id"
                        ).AsList();
                    },
                    context
                );

            items.Results = championshipNotifications;

            return items;
        }

        private void ProcessSearchFilter(ChampionshipNotificationSearchFilter? searchFilter = null)
        {
            if (searchFilter == null)
                return;

            base.AddFilterCriteria(ConditionType.In, "Championship_Notification", "Id", "id", searchFilter.Ids);
            base.AddFilterCriteria(ConditionType.Equal, "Championship_Notification", "IdChampionship", "idChampionship", searchFilter.Championship?.Id);
            base.AddFilterCriteria(ConditionType.In, "Championship_Notification_RaceClass", "IdRaceClass", "idRaceClass", searchFilter.RaceClasses);
        }

        #endregion
    }
}

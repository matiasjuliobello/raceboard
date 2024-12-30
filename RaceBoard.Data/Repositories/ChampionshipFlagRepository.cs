using Dapper;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data.Helpers.Interfaces;
using RaceBoard.Data.Repositories.Base.Abstract;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;

namespace RaceBoard.Data.Repositories
{
    public class ChampionshipFlagRepository : AbstractRepository, IChampionshipFlagRepository
    {
        #region Private Members

        private readonly Dictionary<string, string> _columnsMapping = new()
        {
            { "Id", "[Championship_Flag].Id" },
            { "Raising", "[Championship_Flag].Raising" },
            { "Lowering", "[Championship_Flag].Lowering" },
            { "Order", "[Championship_Flag].[Order]" },
            { "Championship.Id", "[Championship].Id" },
            { "Flag.Id", "[Flag].Id" },
            { "Flag.Name", "[Flag].Name" },
            { "Person.Id", "[Person].Id" },
            { "Person.Firstanme", "[Person].Firstanme" },
            { "Person.Lastname", "[Person].Lastname" }
        };

        #endregion

        public ChampionshipFlagRepository
            (
                IContextResolver contextResolver,
                IQueryBuilder queryBuilder
            ) : base(contextResolver, queryBuilder)
        {
        }

        #region IChampionshipFlagRepository implementation

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
            return base.Exists(id, "Championship_Flag", "Id", context);
        }

        public bool ExistsDuplicate(ChampionshipFlagGroup championshipFlagGroup, ITransactionalContext? context = null)
        {
            //string condition = "[IdChampionship] = @idChampionship AND [IdFlag] = @idFlag";

            //string existsQuery = base.GetExistsDuplicateQuery("[Championship_Flag]", condition, "Id", "@id");

            //QueryBuilder.AddCommand(existsQuery);
            //QueryBuilder.AddParameter("id", championshipFlag.Id);
            //QueryBuilder.AddParameter("idChampionship", championshipFlag.Championship.Id);
            //QueryBuilder.AddParameter("idFlag", championshipFlag.Flag.Id);

            //return base.Execute<bool>(context);

            return false;
        }

        public PaginatedResult<ChampionshipFlagGroup> Get(ChampionshipFlagSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            return this.GetChampionshipFlags(searchFilter: searchFilter, paginationFilter: paginationFilter, sorting: sorting, context: context);
        }

        public void CreateGroup(ChampionshipFlagGroup championshipFlagGroup, ITransactionalContext? context = null)
        {
            this.CreateChampionshipFlagGroup(championshipFlagGroup, context);
        }

        public void AddFlags(List<ChampionshipFlag> championshipFlags, ITransactionalContext? context = null)
        {
            this.CreateChampionshipFlags(championshipFlags, context);
        }

        public void UpdateFlags(List<ChampionshipFlag> championshipFlags, ITransactionalContext? context = null)
        {
            this.UpdateChampionshipFlags(championshipFlags, context);
        }

        public int DeleteGroup(int id, ITransactionalContext? context = null)
        {
            int affectedRows = base.Delete("[Championship_Flag]", id, "IdChampionshipFlagGroup", context);

            base.Delete("[Championship_FlagGroup]", id, "Id", context);

            return affectedRows;
        }

        #endregion


        #region Private Methods

        private PaginatedResult<ChampionshipFlagGroup> GetChampionshipFlags(ChampionshipFlagSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            string sql = $@"SELECT
	                            [Championship_FlagGroup].Id [Id],
                                [Championship_Flag].Id [Id],
	                            [Championship_Flag].Raising [Raising],
	                            [Championship_Flag].Lowering [Lowering],
	                            [Championship_Flag].[Order] [Order],
	                            [Championship].Id [Id],
                                [Flag].Id [Id],
                                [Flag].Name [Name],
	                            [Person].Id [Id],
                                [Person].Firstname [Firstname],
                                [Person].Lastname [Lastname],
	                            [User].Id [Id]
                            FROM [Championship_FlagGroup] [Championship_FlagGroup]
                            INNER JOIN [Championship_Flag] [Championship_Flag] ON [Championship_Flag].IdChampionshipFlagGroup = [Championship_FlagGroup].Id
                            INNER JOIN [Championship] [Championship] ON [Championship].Id = [Championship_FlagGroup].IdChampionship
                            INNER JOIN [Flag] [Flag] ON [Flag].Id = [Championship_Flag].IdFlag
                            INNER JOIN [User] [User] ON [User].Id = [Championship_Flag].IdUser
                            INNER JOIN [User_Person] [User_Person] ON [User_Person].IdUser = [User].Id
                            INNER JOIN [Person] [Person] ON [Person].Id = [User_Person].IdPerson";

            QueryBuilder.AddCommand(sql);

            ProcessSearchFilter(searchFilter);

            //QueryBuilder.AddSorting(sorting, _columnsMapping);
            QueryBuilder.AddSorting(new string[] { "[Championship_Flag].Raising ASC", "[Championship_Flag].[Order] ASC" });
            QueryBuilder.AddPagination(paginationFilter);

            var championshipFlagGroups = new List<ChampionshipFlagGroup>();

            PaginatedResult<ChampionshipFlagGroup> items = base.GetPaginatedResults<ChampionshipFlagGroup>
                (
                    (reader) =>
                    {
                        return reader.Read<ChampionshipFlagGroup, ChampionshipFlag, Championship, Flag, Person, User, ChampionshipFlagGroup>
                        (
                            (championshipFlagGroup, championshipFlag, championship, flag, person, user) =>
                            {
                                var current = championshipFlagGroups.Where(x => x.Id == championshipFlagGroup.Id).FirstOrDefault();
                                if (current == null)
                                {
                                    current = championshipFlagGroup;
                                    championshipFlagGroups.Add(current);
                                }

                                person.User = user;

                                championshipFlag.Person = person;
                                championshipFlag.Flag = flag;
                                championshipFlag.Group = current;

                                current.Championship = championship;

                                current.Flags.Add(championshipFlag);

                                return current;
                            },
                            splitOn: "Id, Id, Id, Id, Id, Id"
                        ).AsList();
                    },
                    context
                );

            items.Results = championshipFlagGroups;

            return items;
        }

        private void ProcessSearchFilter(ChampionshipFlagSearchFilter? searchFilter = null)
        {
            if (searchFilter == null)
                return;

            base.AddFilterCriteria(ConditionType.In, "Championship_Flag", "Id", "ids", searchFilter.Ids);
            base.AddFilterCriteria(ConditionType.Equal, "Championship", "Id", "idChampionship", searchFilter.Championship?.Id);
            base.AddFilterCriteria(ConditionType.Equal, "Championship_FlagGroup", "IdChampionship", "idChampionship", searchFilter.Championship?.Id);
            base.AddFilterCriteria(ConditionType.Equal, "Championship_Flag", "IdFlag", "idFlag", searchFilter.Flag?.Id);
            base.AddFilterCriteria(ConditionType.Equal, "Championship_Flag", "IdUser", "idUser", searchFilter.Person?.User?.Id);

            //if (searchFilter.Raising.HasValue)
            //    searchFilter.Raising = searchFilter.Raising.Value.UtcDateTime.Date;
            //base.AddFilterCriteria(ConditionType.GreaterOrEqualThan, "Championship_Flag", "Raising", "raising", searchFilter.Raising);

            //if (searchFilter.Lowering.HasValue)
            //{
            //    searchFilter.Lowering = searchFilter.Lowering.Value.UtcDateTime.Date.AddDays(1);
            //    //base.AddFilterCriteria(ConditionType.LessOrEqualThan, "Championship_Flag", "Lowering", "lowering", searchFilter.Lowering);
            //    QueryBuilder.AddCondition($"( [Championship_Flag].Lowering <= @lowering OR [Championship_Flag].Lowering IS NULL)");
            //    QueryBuilder.AddParameter("lowering", searchFilter.Lowering);
            //}
        }

        private void CreateChampionshipFlagGroup(ChampionshipFlagGroup championshipFlagGroup, ITransactionalContext? context = null)
        {
            string sql = @" INSERT INTO [Championship_FlagGroup]
                                ( IdChampionship )
                            VALUES
                                ( @idChampionship )";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("idChampionship", championshipFlagGroup.Championship.Id);

            QueryBuilder.AddReturnLastInsertedId();

            championshipFlagGroup.Id = base.Execute<int>(context);
        }

        private void CreateChampionshipFlags(List<ChampionshipFlag> championshipFlags, ITransactionalContext? context = null)
        {
            string sql = @" INSERT INTO [Championship_Flag]
                                ( IdChampionshipFlagGroup, IdFlag, [Order], Raising, Lowering, IdUser )
                            VALUES
                                ( @idChampionshipFlagGroup, @idFlag, @order, @raising, @lowering, @idUser )";

            foreach (ChampionshipFlag championshipFlag in championshipFlags.OrderBy(x => x.Order))
            {
                QueryBuilder.Clear();
                QueryBuilder.AddCommand(sql);
                QueryBuilder.AddParameter("idChampionshipFlagGroup", championshipFlag.Group.Id);
                QueryBuilder.AddParameter("idFlag", championshipFlag.Flag.Id);
                QueryBuilder.AddParameter("order", championshipFlag.Order);
                QueryBuilder.AddParameter("raising", championshipFlag.Raising);
                QueryBuilder.AddParameter("lowering", championshipFlag.Lowering);
                QueryBuilder.AddParameter("idUser", championshipFlag.User.Id);

                QueryBuilder.AddReturnLastInsertedId();

                championshipFlag.Id = base.Execute<int>(context);
            }
        }

        private void UpdateChampionshipFlags(List<ChampionshipFlag> championshipFlags, ITransactionalContext? context = null)
        {
            string sql = @" UPDATE [Championship_Flag] SET Lowering = @lowering, IdUser = @idUser";

            foreach (ChampionshipFlag championshipFlag in championshipFlags.OrderBy(x => x.Order))
            {
                QueryBuilder.Clear();
                QueryBuilder.AddCommand(sql);
                QueryBuilder.AddParameter("lowering", championshipFlag.Lowering);
                QueryBuilder.AddParameter("idUser", championshipFlag.User.Id);
                QueryBuilder.AddParameter("id", championshipFlag.Id);

                QueryBuilder.AddCondition("Id = @id");

                championshipFlag.Id = base.Execute<int>(context);
            }
        }

        #endregion
    }
}

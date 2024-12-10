using Dapper;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data.Helpers.Interfaces;
using RaceBoard.Data.Repositories.Base.Abstract;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;

namespace RaceBoard.Data.Repositories
{
    public class CompetitionFlagRepository : AbstractRepository, ICompetitionFlagRepository
    {
        #region Private Members

        private readonly Dictionary<string, string> _columnsMapping = new()
        {
            { "Id", "[Competition_Flag].Id" },
            { "Raising", "[Competition_Flag].Raising" },
            { "Lowering", "[Competition_Flag].Lowering" },
            { "Order", "[Competition_Flag].[Order]" },
            { "Competition.Id", "[Competition].Id" },
            { "Flag.Id", "[Flag].Id" },
            { "Flag.Name", "[Flag].Name" },
            { "Person.Id", "[Person].Id" },
            { "Person.Firstanme", "[Person].Firstanme" },
            { "Person.Lastname", "[Person].Lastname" }
        };

        #endregion

        public CompetitionFlagRepository
            (
                IContextResolver contextResolver,
                IQueryBuilder queryBuilder
            ) : base(contextResolver, queryBuilder)
        {
        }

        #region ICompetitionFlagRepository implementation

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
            return base.Exists(id, "Competition_Flag", "Id", context);
        }

        public bool ExistsDuplicate(CompetitionFlagGroup competitionFlagGroup, ITransactionalContext? context = null)
        {
            //string condition = "[IdCompetition] = @idCompetition AND [IdFlag] = @idFlag";

            //string existsQuery = base.GetExistsDuplicateQuery("[Competition_Flag]", condition, "Id", "@id");

            //QueryBuilder.AddCommand(existsQuery);
            //QueryBuilder.AddParameter("id", competitionFlag.Id);
            //QueryBuilder.AddParameter("idCompetition", competitionFlag.Competition.Id);
            //QueryBuilder.AddParameter("idFlag", competitionFlag.Flag.Id);

            //return base.Execute<bool>(context);

            return false;
        }

        public PaginatedResult<CompetitionFlagGroup> Get(CompetitionFlagSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            return this.GetCompetitionFlags(searchFilter: searchFilter, paginationFilter: paginationFilter, sorting: sorting, context: context);
        }

        public void CreateGroup(CompetitionFlagGroup competitionFlagGroup, ITransactionalContext? context = null)
        {
            this.CreateCompetitionFlagGroup(competitionFlagGroup, context);
        }

        public void AddFlags(List<CompetitionFlag> competitionFlags, ITransactionalContext? context = null)
        {
            this.CreateCompetitionFlags(competitionFlags, context);
        }

        public void UpdateFlags(List<CompetitionFlag> competitionFlags, ITransactionalContext? context = null)
        {
            this.UpdateCompetitionFlags(competitionFlags, context);
        }

        public int DeleteGroup(int id, ITransactionalContext? context = null)
        {
            int affectedRows = base.Delete("[Competition_Flag]", id, "IdCompetitionFlagGroup", context);

            base.Delete("[Competition_FlagGroup]", id, "Id", context);

            return affectedRows;
        }

        #endregion


        #region Private Methods

        private PaginatedResult<CompetitionFlagGroup> GetCompetitionFlags(CompetitionFlagSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            string sql = $@"SELECT
	                            [Competition_FlagGroup].Id [Id],
                                [Competition_Flag].Id [Id],
	                            [Competition_Flag].Raising [Raising],
	                            [Competition_Flag].Lowering [Lowering],
	                            [Competition_Flag].[Order] [Order],
	                            [Competition].Id [Id],
                                [Flag].Id [Id],
                                [Flag].Name [Name],
	                            [Person].Id [Id],
                                [Person].Firstname [Firstname],
                                [Person].Lastname [Lastname],
	                            [User].Id [Id]
                            FROM [Competition_FlagGroup] [Competition_FlagGroup]
                            INNER JOIN [Competition_Flag] [Competition_Flag] ON [Competition_Flag].IdCompetitionFlagGroup = [Competition_FlagGroup].Id
                            INNER JOIN [Competition] [Competition] ON [Competition].Id = [Competition_FlagGroup].IdCompetition
                            INNER JOIN [Flag] [Flag] ON [Flag].Id = [Competition_Flag].IdFlag
                            INNER JOIN [User] [User] ON [User].Id = [Competition_Flag].IdUser
                            INNER JOIN [User_Person] [User_Person] ON [User_Person].IdUser = [User].Id
                            INNER JOIN [Person] [Person] ON [Person].Id = [User_Person].IdPerson";

            QueryBuilder.AddCommand(sql);

            ProcessSearchFilter(searchFilter);

            //QueryBuilder.AddSorting(sorting, _columnsMapping);
            QueryBuilder.AddSorting(new string[] { "[Competition_Flag].Raising ASC", "[Competition_Flag].[Order] ASC" });
            QueryBuilder.AddPagination(paginationFilter);

            var competitionFlagGroups = new List<CompetitionFlagGroup>();

            PaginatedResult<CompetitionFlagGroup> items = base.GetPaginatedResults<CompetitionFlagGroup>
                (
                    (reader) =>
                    {
                        return reader.Read<CompetitionFlagGroup, CompetitionFlag, Competition, Flag, Person, User, CompetitionFlagGroup>
                        (
                            (competitionFlagGroup, competitionFlag, competition, flag, person, user) =>
                            {
                                var current = competitionFlagGroups.Where(x => x.Id == competitionFlagGroup.Id).FirstOrDefault();
                                if (current == null)
                                {
                                    current = competitionFlagGroup;
                                    competitionFlagGroups.Add(current);
                                }

                                person.User = user;

                                competitionFlag.Person = person;
                                competitionFlag.Flag = flag;
                                competitionFlag.Group = current;

                                current.Competition = competition;

                                current.Flags.Add(competitionFlag);

                                return current;
                            },
                            splitOn: "Id, Id, Id, Id, Id, Id"
                        ).AsList();
                    },
                    context
                );

            items.Results = competitionFlagGroups;

            return items;
        }

        private void ProcessSearchFilter(CompetitionFlagSearchFilter? searchFilter = null)
        {
            if (searchFilter == null)
                return;

            base.AddFilterCriteria(ConditionType.In, "Competition_Flag", "Id", "ids", searchFilter.Ids);
            base.AddFilterCriteria(ConditionType.Equal, "Competition", "Id", "idCompetition", searchFilter.Competition?.Id);
            base.AddFilterCriteria(ConditionType.Equal, "Competition_FlagGroup", "IdCompetition", "idCompetition", searchFilter.Competition?.Id);
            base.AddFilterCriteria(ConditionType.Equal, "Competition_Flag", "IdFlag", "idFlag", searchFilter.Flag?.Id);
            base.AddFilterCriteria(ConditionType.Equal, "Competition_Flag", "IdUser", "idUser", searchFilter.Person?.User?.Id);

            //if (searchFilter.Raising.HasValue)
            //    searchFilter.Raising = searchFilter.Raising.Value.UtcDateTime.Date;
            //base.AddFilterCriteria(ConditionType.GreaterOrEqualThan, "Competition_Flag", "Raising", "raising", searchFilter.Raising);

            //if (searchFilter.Lowering.HasValue)
            //{
            //    searchFilter.Lowering = searchFilter.Lowering.Value.UtcDateTime.Date.AddDays(1);
            //    //base.AddFilterCriteria(ConditionType.LessOrEqualThan, "Competition_Flag", "Lowering", "lowering", searchFilter.Lowering);
            //    QueryBuilder.AddCondition($"( [Competition_Flag].Lowering <= @lowering OR [Competition_Flag].Lowering IS NULL)");
            //    QueryBuilder.AddParameter("lowering", searchFilter.Lowering);
            //}
        }

        private void CreateCompetitionFlagGroup(CompetitionFlagGroup competitionFlagGroup, ITransactionalContext? context = null)
        {
            string sql = @" INSERT INTO [Competition_FlagGroup]
                                ( IdCompetition )
                            VALUES
                                ( @idCompetition )";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("idCompetition", competitionFlagGroup.Competition.Id);

            QueryBuilder.AddReturnLastInsertedId();

            competitionFlagGroup.Id = base.Execute<int>(context);
        }

        private void CreateCompetitionFlags(List<CompetitionFlag> competitionFlags, ITransactionalContext? context = null)
        {
            string sql = @" INSERT INTO [Competition_Flag]
                                ( IdCompetitionFlagGroup, IdFlag, [Order], Raising, Lowering, IdUser )
                            VALUES
                                ( @idCompetitionFlagGroup, @idFlag, @order, @raising, @lowering, @idUser )";

            foreach (CompetitionFlag competitionFlag in competitionFlags.OrderBy(x => x.Order))
            {
                QueryBuilder.Clear();
                QueryBuilder.AddCommand(sql);
                QueryBuilder.AddParameter("idCompetitionFlagGroup", competitionFlag.Group.Id);
                QueryBuilder.AddParameter("idFlag", competitionFlag.Flag.Id);
                QueryBuilder.AddParameter("order", competitionFlag.Order);
                QueryBuilder.AddParameter("raising", competitionFlag.Raising);
                QueryBuilder.AddParameter("lowering", competitionFlag.Lowering);
                QueryBuilder.AddParameter("idUser", competitionFlag.User.Id);

                QueryBuilder.AddReturnLastInsertedId();

                competitionFlag.Id = base.Execute<int>(context);
            }
        }

        private void UpdateCompetitionFlags(List<CompetitionFlag> competitionFlags, ITransactionalContext? context = null)
        {
            string sql = @" UPDATE [Competition_Flag] SET Lowering = @lowering, IdUser = @idUser";

            foreach (CompetitionFlag competitionFlag in competitionFlags.OrderBy(x => x.Order))
            {
                QueryBuilder.Clear();
                QueryBuilder.AddCommand(sql);
                QueryBuilder.AddParameter("lowering", competitionFlag.Lowering);
                QueryBuilder.AddParameter("idUser", competitionFlag.User.Id);
                QueryBuilder.AddParameter("id", competitionFlag.Id);

                QueryBuilder.AddCondition("Id = @id");

                competitionFlag.Id = base.Execute<int>(context);
            }
        }

        #endregion
    }
}

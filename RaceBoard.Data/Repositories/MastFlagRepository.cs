using Dapper;
using Newtonsoft.Json.Linq;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data.Helpers.Interfaces;
using RaceBoard.Data.Repositories.Base.Abstract;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;
using System;

namespace RaceBoard.Data.Repositories
{
    public class MastFlagRepository : AbstractRepository, IMastFlagRepository
    {
        #region Private Members

        private readonly Dictionary<string, string> _columnsMapping = new()
        {
            { "Id", "[Mast_Flag].Id" },
            { "RaisingMoment", "[Mast_Flag].RaisingMoment" },
            { "LoweringMoment", "[Mast_Flag].LoweringMoment" },
            { "Competition.Id", "[Competition].Id" },
            { "Mast.Id", "[Mast].Id" },
            { "Flag.Id", "[Flag].Id" },
            { "Flag.Name", "[Flag].Name" },
            { "Person.Id", "[Person].Id" },
            { "Person.Firstanme", "[Person].Firstanme" },
            { "Person.Lastname", "[Person].Lastname" }
        };

        #endregion

        #region Constructors

        public MastFlagRepository(IContextResolver contextResolver, IQueryBuilder queryBuilder) : base(contextResolver, queryBuilder)
        {
        }

        #endregion

        #region IMastFlagRepository implementation

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
            return base.Exists(id, "Mast_Flag", "Id", context);
        }

        public bool ExistsDuplicate(MastFlag mastFlag, ITransactionalContext? context = null)
        {
            string condition = "[IdMast] = @idMast AND [IdFlag] = @idFlag";

            string existsQuery = base.GetExistsDuplicateQuery("[Mast_Flag]", condition, "Id", "@id");

            QueryBuilder.AddCommand(existsQuery);
            QueryBuilder.AddParameter("id", mastFlag.Id);
            QueryBuilder.AddParameter("idMast", mastFlag.Mast.Id);
            QueryBuilder.AddParameter("idFlag", mastFlag.Flag.Id);

            return base.Execute<bool>(context);
        }

        public PaginatedResult<MastFlag> Get(MastFlagSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            return this.GetMastFlags(searchFilter: searchFilter, paginationFilter: paginationFilter, sorting: sorting, context: context);
        }

        public void Create(MastFlag mastFlag, ITransactionalContext? context = null)
        {
            this.CreateMastFlag(mastFlag, context);
        }

        public void Update(MastFlag mastFlag, ITransactionalContext? context = null)
        {
            this.UpdateMastFlag(mastFlag, context);
        }

        public int Delete(int id, ITransactionalContext? context = null)
        {
            return base.Delete("[Mast_Flag]", id, "Id", context);
        }

        #endregion

        #region Private Methods

        private PaginatedResult<MastFlag> GetMastFlags(MastFlagSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            string sql = $@"SELECT
                                [Mast_Flag].Id [Id],
	                            [Mast_Flag].RaisingMoment [RaisingMoment],
	                            [Mast_Flag].LoweringMoment [LoweringMoment],
	                            [Mast].Id [Id],
                                [Flag].Id [Id],
                                [Flag].Name [Name],
	                            [Person].Id [Id],
                                [Person].Firstname [Firstname],
                                [Person].Lastname [Lastname],
	                            [User].Id [Id]
                            FROM [Mast_Flag]
                            INNER JOIN [Mast] [Mast] ON [Mast].Id = [Mast_Flag].IdMast
                            INNER JOIN [Competition] [Competition] ON [Competition].Id = [Mast].IdCompetition
                            INNER JOIN [Flag] [Flag] ON [Flag].Id = [Mast_Flag].IdFlag
                            INNER JOIN [User] [User] ON [User].Id = [Mast_Flag].IdUser
                            INNER JOIN [User_Person] [User_Person] ON [User_Person].IdUser = [User].Id
                            INNER JOIN [Person] [Person] ON [Person].Id = [User_Person].IdPerson";

            QueryBuilder.AddCommand(sql);

            ProcessSearchFilter(searchFilter);

            QueryBuilder.AddSorting(sorting, _columnsMapping);
            QueryBuilder.AddPagination(paginationFilter);

            var mastFlags = new List<MastFlag>();
            
            PaginatedResult<MastFlag> items = base.GetPaginatedResults<MastFlag>
                (
                    (reader) =>
                    {
                        return reader.Read<MastFlag, Mast, Flag, Person, User, MastFlag>
                        (
                            (mastFlag, mast, flag, person, user) =>
                            {
                                mastFlag.Mast = mast;
                                mastFlag.Flag = flag;

                                person.User = user;
                                mastFlag.Person = person;

                                mastFlags.Add(mastFlag);

                                return mastFlag;
                            },
                            splitOn: "Id, Id, Id, Id, Id"
                        ).AsList();
                    },
                    context
                );

            items.Results = mastFlags;

            return items;
        }

        private void ProcessSearchFilter(MastFlagSearchFilter? searchFilter = null)
        {
            if (searchFilter == null)
                return;

            base.AddFilterCriteria(ConditionType.In, "Mast_Flag", "Id", "ids", searchFilter.Ids);
            base.AddFilterCriteria(ConditionType.Equal, "Competition", "Id", "idCompetition", searchFilter.Competition?.Id);
            base.AddFilterCriteria(ConditionType.Equal, "Mast_Flag", "IdFlag", "idFlag", searchFilter.Flag?.Id);
            base.AddFilterCriteria(ConditionType.Equal, "Mast_Flag", "IdMast", "idMast", searchFilter.Mast?.Id);
            base.AddFilterCriteria(ConditionType.Equal, "Mast_Flag", "IdUser", "idUser", searchFilter.Person?.User?.Id);

            if (searchFilter.RisingMoment.HasValue)
                searchFilter.RisingMoment = searchFilter.RisingMoment.Value.UtcDateTime.Date;
            base.AddFilterCriteria(ConditionType.GreaterOrEqualThan, "Mast_Flag", "RaisingMoment", "raisingMoment", searchFilter.RisingMoment);

            if (searchFilter.LoweringMoment.HasValue)
            {
                searchFilter.LoweringMoment = searchFilter.LoweringMoment.Value.UtcDateTime.Date.AddDays(1);
                //base.AddFilterCriteria(ConditionType.LessOrEqualThan, "Mast_Flag", "LoweringMoment", "loweringMoment", searchFilter.LoweringMoment);
                QueryBuilder.AddCondition($"( [Mast_Flag].LoweringMoment <= @loweringMoment OR [Mast_Flag].LoweringMoment IS NULL)");
                QueryBuilder.AddParameter("loweringMoment", searchFilter.LoweringMoment);
            }
        }

        private void CreateMastFlag(MastFlag mastFlag, ITransactionalContext? context = null)
        {
            string sql = @" INSERT INTO [Mast_Flag]
                                ( IdMast, IdFlag, IdUser, RaisingMoment, LoweringMoment )
                            VALUES
                                ( @idMast, @idFlag, @idUser, @raisingMoment, @loweringMoment )";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("idMast", mastFlag.Mast.Id);
            QueryBuilder.AddParameter("idFlag", mastFlag.Flag.Id);
            QueryBuilder.AddParameter("idUser", mastFlag.Person.User.Id);
            QueryBuilder.AddParameter("raisingMoment", mastFlag.RaisingMoment);
            QueryBuilder.AddParameter("loweringMoment", mastFlag.LoweringMoment);

            QueryBuilder.AddReturnLastInsertedId();

            mastFlag.Id = base.Execute<int>(context);
        }

        private void UpdateMastFlag(MastFlag mastFlag, ITransactionalContext? context = null)
        {
            string sql = @" UPDATE [Mast_Flag] SET LoweringMoment = @loweringMoment";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("loweringMoment", mastFlag.LoweringMoment);
            QueryBuilder.AddParameter("id", mastFlag.Id);

            QueryBuilder.AddCondition("Id = @id");

            mastFlag.Id = base.Execute<int>(context);
        }

        #endregion
    }
}
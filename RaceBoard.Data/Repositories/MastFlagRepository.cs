using Dapper;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data.Helpers.Interfaces;
using RaceBoard.Data.Repositories.Base.Abstract;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;

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
            { "IsActive", "[Mast_Flag].IsActive" },
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
            string existsQuery = base.GetExistsQuery("[Mast_Flag]", "[Id] = @id");

            QueryBuilder.AddCommand(existsQuery);
            QueryBuilder.AddParameter("id", id);

            return base.Execute<bool>(context);
        }

        public bool ExistsDuplicate(MastFlag mastFlag, ITransactionalContext? context = null)
        {
            string condition = "[IdMast] = @idMast AND [IdFlag] = @idFlag AND [IsActive] = @isActive";

            string existsQuery = base.GetExistsDuplicateQuery("[Mast_Flag]", condition, "Id", "@id");

            QueryBuilder.AddCommand(existsQuery);
            QueryBuilder.AddParameter("id", mastFlag.Id);
            QueryBuilder.AddParameter("idMast", mastFlag.Mast.Id);
            QueryBuilder.AddParameter("idFlag", mastFlag.Flag.Id);
            QueryBuilder.AddParameter("isActive", mastFlag.IsActive);

            return base.Execute<bool>(context);
        }

        public PaginatedResult<MastFlag> Get(MastFlagSearchFilter searchFilter, PaginationFilter paginationFilter, Sorting sorting, ITransactionalContext? context = null)
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

        #endregion

        #region Private Methods

        private PaginatedResult<MastFlag> GetMastFlags(MastFlagSearchFilter searchFilter, PaginationFilter paginationFilter, Sorting sorting, ITransactionalContext? context = null)
        {
            string sql = $@"SELECT
                                [Mast_Flag].Id [Id],
	                            [Mast_Flag].RaisingMoment [RaisingMoment],
	                            [Mast_Flag].LoweringMoment [LoweringMoment],
	                            [Mast_Flag].IsActive,
	                            [Mast].Id [Id],
                                [Flag].Id [Id],
                                [Flag].Name [Name],
	                            [Person].Id [Id],
                                [Person].Firstname [Firstname],
                                [Person].Lastname [Lastname]
                            FROM [Mast_Flag]
                            INNER JOIN [Mast] [Mast] ON [Mast].Id = [Mast_Flag].IdMast
                            INNER JOIN [Flag] [Flag] ON [Flag].Id = [Mast_Flag].IdFlag
                            INNER JOIN [Person] [Person] ON [Person].Id = [Mast_Flag].IdPerson";

            QueryBuilder.AddCommand(sql);

            ProcessSearchFilter(searchFilter);

            QueryBuilder.AddSorting(sorting, _columnsMapping);
            QueryBuilder.AddPagination(paginationFilter);

            var mastFlags = new List<MastFlag>();

            PaginatedResult<MastFlag> items = base.GetPaginatedResults<MastFlag>
                (
                    (reader) =>
                    {
                        return reader.Read<MastFlag, Mast, Flag, Person, MastFlag>
                        (
                            (mastFlag, mast, flag, person) =>
                            {
                                mastFlag.Mast = mast;
                                mastFlag.Flag = flag;
                                mastFlag.Person = person;

                                mastFlags.Add(mastFlag);

                                return mastFlag;
                            },
                            splitOn: "Id, Id, Id, Id"
                        ).AsList();
                    },
                    context
                );

            items.Results = mastFlags;

            return items;
        }

        private void ProcessSearchFilter(MastFlagSearchFilter searchFilter)
        {
            base.AddFilterCriteria(ConditionType.In, "Mast_Flag", "Id", searchFilter.Ids);
            base.AddFilterCriteria(ConditionType.Equal, "Mast_Flag", "IdMast", searchFilter.Mast);
            base.AddFilterCriteria(ConditionType.Equal, "Mast_Flag", "IdFlag", searchFilter.Flag);
            base.AddFilterCriteria(ConditionType.Equal, "Mast_Flag", "IdPerson", searchFilter.Person);
            base.AddFilterCriteria(ConditionType.Equal, "Mast_Flag", "IsActive", searchFilter.IsActive);
        }

        private void CreateMastFlag(MastFlag mastFlag, ITransactionalContext? context = null)
        {
            string sql = @" INSERT INTO [Mast_Flag]
                                ( IdMast, IdFlag, IdPerson, RaisingMoment, LoweringMoment, IsActive )
                            VALUES
                                ( @idMast, @idFlag, @idPerson, @raisingMoment, @loweringMoment, @isActive )";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("idMast", mastFlag.Mast.Id);
            QueryBuilder.AddParameter("idFlag", mastFlag.Flag.Id);
            QueryBuilder.AddParameter("idPerson", mastFlag.Person.Id);
            QueryBuilder.AddParameter("raisingMoment", mastFlag.RaisingMoment);
            QueryBuilder.AddParameter("loweringMoment", mastFlag.LoweringMoment);
            QueryBuilder.AddParameter("isActive", mastFlag.IsActive);

            QueryBuilder.AddReturnLastInsertedId();

            mastFlag.Id = base.Execute<int>(context);
        }

        private void UpdateMastFlag(MastFlag mastFlag, ITransactionalContext? context = null)
        {
            string sql = @" UPDATE [Mast_Flag]
                            SET
                                LoweringMoment = @loweringMoment, 
                                IsActive = @isActive";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("loweringMoment", mastFlag.LoweringMoment);
            QueryBuilder.AddParameter("isActive", mastFlag.IsActive);
            QueryBuilder.AddParameter("id", mastFlag.Id);

            QueryBuilder.AddCondition("Id = @id");

            mastFlag.Id = base.Execute<int>(context);
        }

        #endregion
    }
}
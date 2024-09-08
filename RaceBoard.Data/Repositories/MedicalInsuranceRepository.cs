using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data.Helpers.Interfaces;
using RaceBoard.Data.Repositories.Base.Abstract;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;

namespace RaceBoard.Data.Repositories
{
    public class MedicalInsuranceRepository : AbstractRepository, IMedicalInsuranceRepository
    {
        #region Private Members

        private readonly Dictionary<string, string> _columnsMapping = new()
        {
            { "Id", "[MedicalInsurance].Id" },
            { "Name", "[MedicalInsurance].Name"}
        };

        #endregion

        #region Constructors

        public MedicalInsuranceRepository(IContextResolver contextResolver, IQueryBuilder queryBuilder) : base(contextResolver, queryBuilder)
        {
        }

        #endregion

        #region IMedicalInsuranceRepository implementation

        public PaginatedResult<MedicalInsurance> Get(MedicalInsuranceSearchFilter searchFilter, PaginationFilter paginationFilter, Sorting sorting, ITransactionalContext? context = null)
        {
            return this.GetMedicalInsurances(searchFilter: searchFilter, paginationFilter: paginationFilter, sorting: sorting, context: context);
        }

        #endregion

        #region Private Methods

        private PaginatedResult<MedicalInsurance> GetMedicalInsurances(MedicalInsuranceSearchFilter searchFilter, PaginationFilter paginationFilter, Sorting sorting, ITransactionalContext? context = null)
        {
            string sql = $@"SELECT
                                [MedicalInsurance].Id [Id],
                                [MedicalInsurance].Name [Name]
                            FROM [MedicalInsurance] [MedicalInsurance]";

            QueryBuilder.AddCommand(sql);

            ProcessSearchFilter(searchFilter);

            QueryBuilder.AddSorting(sorting, _columnsMapping);
            QueryBuilder.AddPagination(paginationFilter);

            return base.GetMultipleResultsWithPagination<MedicalInsurance>(context);
        }

        private void ProcessSearchFilter(MedicalInsuranceSearchFilter searchFilter)
        {
            base.AddFilterCriteria(ConditionType.In, "MedicalInsurance", "Id", searchFilter.Ids);
            base.AddFilterCriteria(ConditionType.Equal, "MedicalInsurance", "Name", searchFilter.Name);
        }

        #endregion
    }
}
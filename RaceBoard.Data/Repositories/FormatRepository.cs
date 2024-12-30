using RaceBoard.Data.Helpers.Interfaces;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Data.Repositories.Base.Abstract;
using RaceBoard.Domain;

namespace RaceBoard.Data.Repositories
{
    public class FormatRepository : AbstractRepository, IFormatRepository
    {
        #region Private Members

        #endregion

        #region Constructors

        public FormatRepository(IContextResolver contextResolver, IQueryBuilder queryBuilder) : base(contextResolver, queryBuilder)
        {
        }

        #endregion

        #region IFormatRepository implementation

        public List<DateFormat> GetDateFormats(ITransactionalContext? context = null)
        {
            string sql = $@"SELECT
	                            Id [Id],	                            
	                            Format [Format]
                            FROM [DateFormat]";

            QueryBuilder.AddCommand(sql);

            return base.GetMultipleResults<DateFormat>(context).ToList();
        }

        #endregion

        #region Private Methods

        #endregion
    }
}
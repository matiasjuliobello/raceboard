using RaceBoard.Data.Helpers.Interfaces;
using RaceBoard.Data.Repositories.Base.Interfaces;
using RaceBoard.Data.Repositories.Interfaces;
using TimeZone = RaceBoard.Domain.TimeZone;
using RaceBoard.Data.Repositories.Base.Abstract;
using Microsoft.Extensions.Configuration;

namespace RaceBoard.Data.Repositories
{
    public class TimeZoneRepository : AbstractRepository, ITimeZoneRepository
    {
        #region Private Members

        #endregion

        #region Constructors

        public TimeZoneRepository(IContextResolver contextResolver, IQueryBuilder queryBuilder) : base(contextResolver, queryBuilder)
        {
        }

        #endregion

        #region ITimeZoneRepository implementation

        public List<TimeZone> Get(ITransactionalContext? context = null)
        {
            return this.GetTimeZones(id: null, context: context);
        }

        #endregion

        #region Private Methods

        private List<TimeZone> GetTimeZones(int? id, ITransactionalContext? context = null)
        {
            string sql = $@"SELECT
	                            Id [Id],	                            
	                            Name [Name],
	                            Identifier [Identifier],
	                            Offset [Offset]
                            FROM [TimeZone]";

            QueryBuilder.AddCommand(sql);

            if (id.HasValue)
            {
                QueryBuilder.AddCondition($"[TimeZone].Id = @id");
                QueryBuilder.AddParameter("id", id.Value);
            }

            return base.GetMultipleResults<TimeZone>(context).ToList();
        }

        #endregion
    }
}
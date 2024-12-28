using RaceBoard.Data.Helpers.Interfaces;
using RaceBoard.Data.Repositories.Interfaces;
using Language = RaceBoard.Domain.Language;
using RaceBoard.Data.Repositories.Base.Abstract;

namespace RaceBoard.Data.Repositories
{
    public class LanguageRepository : AbstractRepository, ILanguageRepository
    {
        #region Private Members

        #endregion

        #region Constructors

        public LanguageRepository(IContextResolver contextResolver, IQueryBuilder queryBuilder) : base(contextResolver, queryBuilder)
        {
        }

        #endregion

        #region ILanguageRepository implementation

        public List<Language> Get(ITransactionalContext? context = null)
        {
            return this.GetLanguages(id: null, context: context);
        }

        #endregion

        #region Private Methods

        private List<Language> GetLanguages(int? id, ITransactionalContext? context = null)
        {
            string sql = $@"SELECT
	                            Id [Id],	                            
	                            Name [Name],
                                Code [Code]
                            FROM [Language]";

            QueryBuilder.AddCommand(sql);

            if (id.HasValue)
            {
                QueryBuilder.AddCondition($"[Language].Id = @id");
                QueryBuilder.AddParameter("id", id.Value);
            }

            return base.GetMultipleResults<Language>(context).ToList();
        }

        #endregion
    }
}
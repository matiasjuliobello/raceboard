using RaceBoard.Data.Helpers.Interfaces;
using RaceBoard.Data.Repositories.Base.Abstract;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;

namespace RaceBoard.Data.Repositories
{
    public class OrganizationRepository : AbstractRepository, IOrganizationRepository
    {
        #region Private Members

        #endregion

        #region Constructors

        public OrganizationRepository(IContextResolver contextResolver, IQueryBuilder queryBuilder) : base(contextResolver, queryBuilder)
        {
        }

        #endregion

        #region IOrganizationRepository implementation

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

        public void Create(Organization organization, ITransactionalContext? context = null)
        {
            this.CreateOrganization(organization, context);
        }

        public void Update(Organization organization, ITransactionalContext? context = null)
        {
            this.UpdateOrganization(organization, context);
        }

        public int Delete(int id, ITransactionalContext? context = null)
        {
            return base.Delete("[Organization]", id, "Id", context);
        }

        #endregion

        #region Private Methods

        private void CreateOrganization(Organization organization, ITransactionalContext? context = null)
        {
            string sql = @" INSERT INTO [Organization]
                                ( IdCity, Name )
                            VALUES
                                ( @idCity, @name )";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("idCity", organization.City.Id);
            QueryBuilder.AddParameter("name", organization.Name);

            QueryBuilder.AddReturnLastInsertedId();

            organization.Id = base.Execute<int>(context);
        }

        private void UpdateOrganization(Organization organization, ITransactionalContext? context = null)
        {
            string sql = @" UPDATE [Organization] SET
                                IdCity = @idCity,
                                Name = @name";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("idCity", organization.City.Id);
            QueryBuilder.AddParameter("name", organization.Name);

            QueryBuilder.AddParameter("id", organization.Id);
            QueryBuilder.AddCondition("Id = @id");

            base.ExecuteAndGetRowsAffected(context);
        }

        #endregion
    }
}
using BCrypt.Net;
using RaceBoard.Common.Enums;
using RaceBoard.Common.Exceptions;
using RaceBoard.Data.Helpers.Interfaces;
using RaceBoard.Data.Repositories.Base.Abstract;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;
using System;

namespace RaceBoard.Data.Repositories
{
    public class CompetitionRepository : AbstractRepository, ICompetitionRepository
    {
        #region Private Members

        #endregion

        #region Constructors

        public CompetitionRepository(IContextResolver contextResolver, IQueryBuilder queryBuilder) : base(contextResolver, queryBuilder)
        {
        }

        #endregion

        #region ICompetitionRepository implementation

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

        public void Create(Competition competition, ITransactionalContext? context = null)
        {
            this.CreateCompetition(competition, context);          
        }

        public void SetOrganizations(int idCompetition, List<Organization> organizations, ITransactionalContext? context = null)
        {
            this.SetCompetitionOrganizations(idCompetition, organizations, context);
        }

        public void DeleteOrganizations(int idCompetition, ITransactionalContext? context = null)
        {
            this.DeleteCompetitionOrganizations(idCompetition, context);
        }

        public void Update(Competition competition, ITransactionalContext? context = null)
        {
            this.UpdateCompetition(competition, context);
        }

        public int Delete(int id, ITransactionalContext? context = null)
        {
            return base.Delete("[Competition]", id, "Id", context);
        }

        #endregion

        #region Private Methods

        private void CreateCompetition(Competition competition, ITransactionalContext? context = null)
        {
            string sql = @" INSERT INTO [Competition]
                                ( IdCity, Name, StartDate, EndDate )
                            VALUES
                                ( @idCity, @name, @startDate, @endDate )";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("idCity", competition.City.Id);
            QueryBuilder.AddParameter("name", competition.Name);
            QueryBuilder.AddParameter("startDate", competition.StartDate);
            QueryBuilder.AddParameter("endDate", competition.EndDate);

            QueryBuilder.AddReturnLastInsertedId();

            competition.Id = base.Execute<int>(context);
        }

        private void UpdateCompetition(Competition competition, ITransactionalContext? context = null)
        {
            string sql = @" UPDATE [Competition] SET
                                IdCity = @idCity,
                                Name = @name,
                                StartDate = @startDate,
                                EndDate = @endDate";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("idCity", competition.City.Id);
            QueryBuilder.AddParameter("name", competition.Name);
            QueryBuilder.AddParameter("startDate", competition.StartDate);
            QueryBuilder.AddParameter("endDate", competition.EndDate);

            QueryBuilder.AddParameter("id", competition.Id);
            QueryBuilder.AddCondition("Id = @id");

            base.ExecuteAndGetRowsAffected(context);
        }

        public void SetCompetitionOrganizations(int idCompetition, List<Organization> organizations, ITransactionalContext? context = null)
        {
            int affectedRecords = this.DeleteCompetitionOrganizations(idCompetition, context);

            foreach (var organization in organizations)
            {
                string sql = @" INSERT INTO [Competition_Organization]
                                ( IdCompetition, IdOrganization )
                            VALUES
                                ( @idCompetition, @idOrganization )";

                QueryBuilder.AddCommand(sql);

                QueryBuilder.AddParameter("idCompetition", idCompetition);
                QueryBuilder.AddParameter("idOrganization", organization.Id);

                QueryBuilder.AddReturnLastInsertedId();

                base.Execute<int>(context);
            }
        }

        private int DeleteCompetitionOrganizations(int idCompetition, ITransactionalContext? context = null)
        {
            return base.Delete("[Competition_Organization]", idCompetition, "IdCompetition", context);
        }

        #endregion
    }
}
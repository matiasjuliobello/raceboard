using RaceBoard.Data.Helpers.Interfaces;
using RaceBoard.Data.Repositories.Base.Abstract;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;

namespace RaceBoard.Data.Repositories
{
    public class PersonRepository : AbstractRepository, IPersonRepository
    {
        #region Private Members

        #endregion

        #region Constructors

        public PersonRepository(IContextResolver contextResolver, IQueryBuilder queryBuilder) : base(contextResolver, queryBuilder)
        {
        }

        #endregion

        #region IPersonRepository implementation

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

        public void Create(Person person, ITransactionalContext? context = null)
        {
            this.CreatePerson(person, context);
        }

        public void Update(Person person, ITransactionalContext? context = null)
        {
            this.UpdatePerson(person, context);
        }

        public int Delete(int id, ITransactionalContext? context = null)
        {
            return base.Delete("[Person]", id, "Id", context);
        }

        #endregion

        #region Private Methods

        private void CreatePerson(Person person, ITransactionalContext? context = null)
        {
            string sql = @" INSERT INTO [Person]
                                ( IdCountry, IdBloodType, IdMedicalInsurance, FirstName, LastName, BirthDate, EmailAddress, ContactPhone )
                            VALUES
                                ( @idCountry, @idBloodType, @idMedicalInsurance, @firstname, @lastname, @birthDate, @emailAddress, @contactPhone )";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("idCountry", person.Country.Id);
            QueryBuilder.AddParameter("idBloodType", person.BloodType.Id);
            QueryBuilder.AddParameter("idMedicalInsurance", person.MedicalInsurance.Id);
            QueryBuilder.AddParameter("firstname", person.Firstname);
            QueryBuilder.AddParameter("lastname", person.Lastname);
            QueryBuilder.AddParameter("birthDate", person.BirthDate);
            QueryBuilder.AddParameter("emailAddress", person.EmailAddress);
            QueryBuilder.AddParameter("contactPhone", person.ContactPhone);

            QueryBuilder.AddReturnLastInsertedId();

            person.Id = base.Execute<int>(context);
        }

        private void UpdatePerson(Person person, ITransactionalContext? context = null)
        {
            string sql = @" UPDATE [Person] SET
                                IdCountry = @idCountry,
                                IdBloodType = @idBloodType,
                                IdMedicalInsurance = @idMedicalInsurance,
                                Firstname = @firstname,
                                Lastname = @lastname,
                                BirthDate = @birthDate,
                                EmailAddress = @emailAddress,
                                ContactPhone = @contactPhone";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("idCountry", person.Country.Id);
            QueryBuilder.AddParameter("idBloodType", person.BloodType.Id);
            QueryBuilder.AddParameter("idMedicalInsurance", person.MedicalInsurance.Id);
            QueryBuilder.AddParameter("firstname", person.Firstname);
            QueryBuilder.AddParameter("lastname", person.Lastname);
            QueryBuilder.AddParameter("birthDate", person.BirthDate);
            QueryBuilder.AddParameter("emailAddress", person.EmailAddress);
            QueryBuilder.AddParameter("contactPhone", person.ContactPhone);

            QueryBuilder.AddParameter("id", person.Id);
            QueryBuilder.AddCondition("Id = @id");

            base.ExecuteAndGetRowsAffected(context);
        }

        #endregion
    }
}
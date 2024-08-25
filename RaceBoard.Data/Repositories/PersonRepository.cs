using Dapper;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data.Helpers.Interfaces;
using RaceBoard.Data.Repositories.Base.Abstract;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;

namespace RaceBoard.Data.Repositories
{
    public class PersonRepository : AbstractRepository, IPersonRepository
    {
        #region Private Members

        private readonly Dictionary<string, string> _columnsMapping = new()
        {
            { "Id", "[Person].Id" },
            { "Firstname", "[Person].Firstname" },
            { "Lastname", "[Person].Lastname"},
            { "BirthDate", "[Person].BirthDate"},
            { "EmailAddress", "[Person].EmailAddress"},
            { "ContactPhone", "[Person].ContactPhone"},
            { "Country.Id", "[Country].Id"},
            { "Country.Name", "[Country].Name"},
            { "BloodType.Id", "[BloodType].Id"},
            { "BloodType.Name", "[BloodType].Name"},
            { "MedicalInsurance.Id", "[MedicalInsurance].Id"},
            { "MedicalInsurance.Name", "[MedicalInsurance].Name"}
        };

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

        public PaginatedResult<Person> Get(PersonSearchFilter searchFilter, PaginationFilter paginationFilter, Sorting sorting, ITransactionalContext? context = null)
        {
            return this.GetPersons(searchFilter, paginationFilter, sorting, context);
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

        private PaginatedResult<Person> GetPersons(PersonSearchFilter searchFilter, PaginationFilter? paginationFilter, Sorting? sorting, ITransactionalContext? context = null)
        {
            string sql = $@"SELECT
	                            [Person].Id [Id],	                            
	                            [Person].Firstname [Firstname],
                                [Person].Lastname [Lastname],
                                [Person].BirthDate [BirthDate],
                                [Person].EmailAddress [EmailAddress],
                                [Person].ContactPhone [ContactPhone],
                                [Country].Id [Id],
                                [Country].Name [Name],
                                [BloodType].Id [Id],
                                [BloodType].Name [Name],
                                [MedicalInsurance].Id [Id],
                                [MedicalInsurance].Name [Name]
                            FROM [Person] [Person]
                            INNER JOIN [Country] [Country] ON [Country].Id = [Person].IdCountry
                            INNER JOIN [BloodType] [BloodType] ON [BloodType].Id = [Person].IdBloodType
                            INNER JOIN [MedicalInsurance] [MedicalInsurance] ON [MedicalInsurance].Id = [Person].IdMedicalInsurance";

            QueryBuilder.AddCommand(sql);
            
            this.ProcessSearchFilter(searchFilter);

            QueryBuilder.AddSorting(sorting, _columnsMapping);
            QueryBuilder.AddPagination(paginationFilter);

            var persons = new List<Person>();

            PaginatedResult<Person> items = base.GetPaginatedResults<Person>
                (
                    (reader) =>
                    {
                        return reader.Read<Person, Country, BloodType, MedicalInsurance, Person>
                        (
                            (person, country, bloodType, medicalInsurance) =>
                            {
                                person.Country = country;
                                person.BloodType = bloodType;
                                person.MedicalInsurance = medicalInsurance;

                                persons.Add(person);

                                return person;
                            },
                            splitOn: "Id, Id, Id, Id"
                        ).AsList();
                    },
                    context
                );

            items.Results = persons;

            return items;
        }

        private void ProcessSearchFilter(PersonSearchFilter searchFilter)
        {
            if (searchFilter == null)
                return;

            if (searchFilter.Ids.Length > 0)
            {
                QueryBuilder.AddCondition($"[Person].Id IN @ids");
                QueryBuilder.AddParameter("ids", searchFilter.Ids);
            }

            if (!string.IsNullOrEmpty(searchFilter.Firstname))
            {
                QueryBuilder.AddCondition($"[Person].Firstname LIKE {AddLikeWildcards("@firstName")}");
                QueryBuilder.AddParameter("firstName", searchFilter.Firstname);
            }

            if (!string.IsNullOrEmpty(searchFilter.Lastname))
            {
                QueryBuilder.AddCondition($"[Person].Lastname LIKE {AddLikeWildcards("@lastName")}");
                QueryBuilder.AddParameter("lastName", searchFilter.Lastname);
            }

            if (!string.IsNullOrEmpty(searchFilter.EmailAddress))
            {
                QueryBuilder.AddCondition($"[Person].EmailAddress = @emailAddress");
                QueryBuilder.AddParameter("emailAddress", searchFilter.EmailAddress);
            }
        }

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
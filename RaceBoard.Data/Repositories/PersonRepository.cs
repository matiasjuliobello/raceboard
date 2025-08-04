using Dapper;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data.Helpers.Interfaces;
using RaceBoard.Data.Repositories.Base.Abstract;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;
using static RaceBoard.Data.Helpers.SqlQueryBuilder;

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
            { "Address", "[Person].Address"},
            { "PhoneNumber", "[Person].PhoneNumber"},
            { "EmergencyContactPhoneNumber", "[Person].EmergencyContactPhoneNumber"},
            { "MedicalInsuranceNumber", "[Person].MedicalInsuranceNumber"},
            { "Gender.Id", "[Gender].Id"},
            { "PersonalIdentificationCardNumber", "[Person].PersonalIdentificationCardNumber"},
            { "Gender.Name", "[Gender].Name"},
            { "Country.Id", "[Country].Id"},
            { "Country.Name", "[Country].Name"},
            { "Country.IsoCode", "[Country].IsoCode"},
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

        public bool Exists(int id, ITransactionalContext? context = null)
        {
            return base.Exists(id, "Person", "Id", context);
        }

        public bool ExistsDuplicate(Person person, ITransactionalContext? context = null)
        {
            string existsQuery = base.GetExistsDuplicateQuery("[Person]", "[Firstname] = @firstname AND [Lastname] = @lastname", "Id", "@id");

            QueryBuilder.AddCommand(existsQuery);
            QueryBuilder.AddParameter("firstname", person.Firstname);
            QueryBuilder.AddParameter("lastname", person.Lastname);
            QueryBuilder.AddParameter("id", person.Id);

            return base.Execute<bool>(context);
        }

        public PaginatedResult<Person> Search(string searchTerm, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            return this.SearchPersons(searchTerm, paginationFilter, sorting, context);
        }

        public PaginatedResult<Person> Get(PersonSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            return this.GetPersons(searchFilter, paginationFilter, sorting, context);
        }

        public Person? Get(int id, ITransactionalContext? context = null)
        {
            var searchFilter = new PersonSearchFilter() { Ids = new int[] { id } };

            return this.GetPersons(searchFilter: searchFilter, paginationFilter: null, sorting: null, context: context).Results.FirstOrDefault();
        }

        public Person GetByIdUser(int idUser, ITransactionalContext? context = null)
        {
            var searchFilter = new PersonSearchFilter() { IdUser = idUser };

            return this.GetPersons(searchFilter: searchFilter, paginationFilter: null, sorting: null, context: context).Results.FirstOrDefault();
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

        public void SetUserAssociation(Person person, ITransactionalContext? context = null)
        {
            if (person.User == null)
                return;

            this.RemovePersonUserAssociation(person, context);
            this.SetPersonUserAssociation(person, context);
        }

        #endregion

        #region Private Methods

        private PaginatedResult<Person> SearchPersons(string searchTerm, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            string sql = $@"SELECT
	                            [Person].Id [Id],	                            
	                            [Person].Firstname [Firstname],
                                [Person].Lastname [Lastname],
                                [Person].BirthDate [BirthDate],
                                [Person].Address [Address],
                                [Person].PhoneNumber [PhoneNumber],
                                [Person].EmergencyContactPhoneNumber [EmergencyContactPhoneNumber],
                                [Person].MedicalInsuranceNumber [MedicalInsuranceNumber],
                                [Person].PersonalIdentificationCardNumber [PersonalIdentificationCardNumber],
                                [Gender].Id [Id],
                                [Gender].Name [Name],
                                [User].Id [Id],
                                [User].Username [Username],
                                [User].Email [Email],
                                [Country].Id [Id],
                                [Country].Name [Name],
                                [Country].IsoCode [IsoCode],
                                [BloodType].Id [Id],
                                [BloodType].Name [Name],
                                [MedicalInsurance].Id [Id],
                                [MedicalInsurance].Name [Name]
                            FROM [Person] [Person]
                            LEFT JOIN [Gender] ON [Gender].Id = [Person].IdGender 
                            LEFT JOIN [Country] [Country] ON [Country].Id = [Person].IdCountry
                            LEFT JOIN [BloodType] [BloodType] ON [BloodType].Id = [Person].IdBloodType
                            LEFT JOIN [MedicalInsurance] [MedicalInsurance] ON [MedicalInsurance].Id = [Person].IdMedicalInsurance
                            LEFT JOIN [User_Person] [User_Person] ON [User_Person].IdPerson = [Person].Id                            
                            LEFT JOIN [User] [User] ON [User].Id = [User_Person].IdUser";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddCondition($"[Person].Firstname COLLATE {_collation_latin_CI_AI} LIKE '%' + @searchTerm + '%' ", LogicalOperator.Or);
            QueryBuilder.AddCondition($"[Person].Lastname COLLATE {_collation_latin_CI_AI} LIKE '%' + @searchTerm + '%'", LogicalOperator.Or);
            //QueryBuilder.AddCondition("[User].Email LIKE '%' + @searchTerm + '%'", LogicalOperator.Or);
            //QueryBuilder.AddParameter("firstName", searchTerm);
            //QueryBuilder.AddParameter("lastName", searchTerm);
            QueryBuilder.AddParameter("searchTerm", searchTerm);

            QueryBuilder.AddSorting(sorting, _columnsMapping);
            QueryBuilder.AddPagination(paginationFilter);

            var persons = new List<Person>();

            PaginatedResult<Person> items = base.GetPaginatedResults<Person>
                (
                    (reader) =>
                    {
                        return reader.Read<Person, Gender, User, Country, BloodType, MedicalInsurance, Person>
                        (
                            (person, gender, user, country, bloodType, medicalInsurance) =>
                            {
                                person.Gender = gender;
                                person.User = user;
                                person.Country = country;
                                person.BloodType = bloodType;
                                person.MedicalInsurance = medicalInsurance;

                                persons.Add(person);

                                return person;
                            },
                            splitOn: "Id, Id, Id, Id, Id, Id"
                        ).AsList();
                    },
                    context
                );

            items.Results = persons;

            return items;
        }

        private PaginatedResult<Person> GetPersons(PersonSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            string sql = $@"SELECT
	                            [Person].Id [Id],	                            
	                            [Person].Firstname [Firstname],
                                [Person].Lastname [Lastname],
                                [Person].BirthDate [BirthDate],
                                [Person].Address [Address],
                                [Person].PhoneNumber [PhoneNumber],
                                [Person].EmergencyContactPhoneNumber [EmergencyContactPhoneNumber],
                                [Person].MedicalInsuranceNumber [MedicalInsuranceNumber],
                                [Person].PersonalIdentificationCardNumber [PersonalIdentificationCardNumber],
                                [Gender].Id [Id],
                                [Gender].Name [Name],
                                [User].Id [Id],
                                [User].Username [Username],
                                [User].Email [Email],
                                [Country].Id [Id],
                                [Country].Name [Name],
                                [Country].IsoCode [IsoCode],
                                [BloodType].Id [Id],
                                [BloodType].Name [Name],
                                [MedicalInsurance].Id [Id],
                                [MedicalInsurance].Name [Name]
                            FROM [Person] [Person]
                            LEFT JOIN [Gender] ON [Gender].Id = [Person].IdGender 
                            LEFT JOIN [Country] [Country] ON [Country].Id = [Person].IdCountry
                            LEFT JOIN [BloodType] [BloodType] ON [BloodType].Id = [Person].IdBloodType
                            LEFT JOIN [MedicalInsurance] [MedicalInsurance] ON [MedicalInsurance].Id = [Person].IdMedicalInsurance
                            LEFT JOIN [User_Person] [User_Person] ON [User_Person].IdPerson = [Person].Id                            
                            LEFT JOIN [User] [User] ON [User].Id = [User_Person].IdUser";

            QueryBuilder.AddCommand(sql);
            
            this.ProcessSearchFilter(searchFilter);

            QueryBuilder.AddSorting(sorting, _columnsMapping);
            QueryBuilder.AddPagination(paginationFilter);

            var persons = new List<Person>();

            PaginatedResult<Person> items = base.GetPaginatedResults<Person>
                (
                    (reader) =>
                    {
                        return reader.Read<Person, Gender, User, Country, BloodType, MedicalInsurance, Person>
                        (
                            (person, gender, user, country, bloodType, medicalInsurance) =>
                            {
                                person.Gender = gender;
                                person.User = user;
                                person.Country = country;
                                person.BloodType = bloodType;
                                person.MedicalInsurance = medicalInsurance;

                                persons.Add(person);

                                return person;
                            },
                            splitOn: "Id, Id, Id, Id, Id, Id"
                        ).AsList();
                    },
                    context
                );

            items.Results = persons;

            return items;
        }

        private void ProcessSearchFilter(PersonSearchFilter? searchFilter = null)
        {
            if (searchFilter == null)
                return;

            base.AddFilterCriteria(ConditionType.In, "Person", "Id", "ids", searchFilter.Ids);
            base.AddFilterCriteria(ConditionType.Equal, "User_Person", "IdUser", "idUser", searchFilter.IdUser);
            base.AddFilterCriteria(ConditionType.Like, "Person", "Firstname", "firstName", searchFilter.Firstname);
            base.AddFilterCriteria(ConditionType.Like, "Person", "Lastname", "lastName", searchFilter.Lastname);
            base.AddFilterCriteria(ConditionType.Like, "[User]", "Email", "emailAddress", searchFilter.EmailAddress);
        }

        private void CreatePerson(Person person, ITransactionalContext? context = null)
        {
            string sql = @" INSERT INTO [Person]
                                ( IdGender, IdCountry, IdBloodType, IdMedicalInsurance, MedicalInsuranceNumber, PersonalIdentificationCardNumber, FirstName, LastName, BirthDate, Address, PhoneNumber, EmergencyContactPhoneNumber )
                            VALUES
                                ( @idGender, @idCountry, @idBloodType, @idMedicalInsurance, @medicalInsuranceNumber, @personalIdentificationCardNumber, @firstname, @lastname, @birthDate, @address, @phoneNumber, @emergencyContactPhoneNumber )";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("idGender", person.Gender?.Id);
            QueryBuilder.AddParameter("idCountry", person.Country?.Id);
            QueryBuilder.AddParameter("idBloodType", person.BloodType?.Id);
            QueryBuilder.AddParameter("idMedicalInsurance", person.MedicalInsurance?.Id);
            QueryBuilder.AddParameter("firstname", person.Firstname);
            QueryBuilder.AddParameter("lastname", person.Lastname);
            QueryBuilder.AddParameter("birthDate", person.BirthDate);
            QueryBuilder.AddParameter("address", person.Address);
            QueryBuilder.AddParameter("phoneNumber", person.PhoneNumber);
            QueryBuilder.AddParameter("emergencyContactPhoneNumber", person.EmergencyContactPhoneNumber);
            QueryBuilder.AddParameter("medicalInsuranceNumber", person.MedicalInsuranceNumber);
            QueryBuilder.AddParameter("personalIdentificationCardNumber", person.PersonalIdentificationCardNumber);


            QueryBuilder.AddReturnLastInsertedId();

            person.Id = base.Execute<int>(context);
        }

        private void UpdatePerson(Person person, ITransactionalContext? context = null)
        {
            string sql = @" UPDATE [Person] SET
                                IdGender = @idGender,
                                IdCountry = @idCountry,
                                IdBloodType = @idBloodType,
                                IdMedicalInsurance = @idMedicalInsurance,
                                MedicalInsuranceNumber = @medicalInsuranceNumber,
                                PersonalIdentificationCardNumber = @personalIdentificationCardNumber,
                                Firstname = @firstname,
                                Lastname = @lastname,
                                BirthDate = @birthDate,
                                Address = @address,
                                PhoneNumber = @phoneNumber,
                                EmergencyContactPhoneNumber = @emergencyContactPhoneNumber";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("idGender", person.Gender?.Id);
            QueryBuilder.AddParameter("idCountry", person.Country.Id);
            QueryBuilder.AddParameter("idBloodType", person.BloodType?.Id);
            QueryBuilder.AddParameter("idMedicalInsurance", person.MedicalInsurance?.Id);
            QueryBuilder.AddParameter("firstname", person.Firstname);
            QueryBuilder.AddParameter("lastname", person.Lastname);
            QueryBuilder.AddParameter("birthDate", person.BirthDate);
            QueryBuilder.AddParameter("address", person.Address);
            QueryBuilder.AddParameter("phoneNumber", person.PhoneNumber);
            QueryBuilder.AddParameter("emergencyContactPhoneNumber", person.EmergencyContactPhoneNumber);
            QueryBuilder.AddParameter("medicalInsuranceNumber", person.MedicalInsuranceNumber);
            QueryBuilder.AddParameter("personalIdentificationCardNumber", person.PersonalIdentificationCardNumber);

            QueryBuilder.AddParameter("id", person.Id);
            QueryBuilder.AddCondition("Id = @id");

            base.ExecuteAndGetRowsAffected(context);
        }

        public void SetPersonUserAssociation(Person person, ITransactionalContext? context = null)
        {
            string sql = @" INSERT INTO [User_Person]
                                ( IdUser, IdPerson )
                            VALUES
                                ( @idUser, @idPerson )";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("idUser", person.User.Id);
            QueryBuilder.AddParameter("idPerson", person.Id);

            QueryBuilder.AddReturnLastInsertedId();

            int id = base.Execute<int>(context);
        }

        public void RemovePersonUserAssociation(Person person, ITransactionalContext? context = null)
        {
            base.Delete("[User_Person]", person.Id, "IdPerson", context);
        }

        #endregion
    }
}
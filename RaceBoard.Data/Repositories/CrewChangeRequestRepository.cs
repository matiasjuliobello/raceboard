using Dapper;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data.Helpers.Interfaces;
using RaceBoard.Data.Repositories.Base.Abstract;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;

namespace RaceBoard.Data.Repositories
{
    public class CrewChangeRequestRepository : AbstractRepository, ICrewChangeRequestRepository
    {
        #region Private Members

        private readonly Dictionary<string, string> _columnsMapping = new()
        {
            { "Id", "[CrewChangeRequest].Id" },
            { "ChangeReason", "[CrewChangeRequest].ChangeReason" },
            { "ReplacementFullname", "[CrewChangeRequest].ReplacementFullname" },
            { "CreationDate", "[CrewChangeRequest].CreationDate" },
            { "ResolutionDate", "[CrewChangeRequest].ResolutionDate" },
            { "ResolutionComments", "[CrewChangeRequest].ResolutionComments"},
            { "Status.Id", "[RequestStatus].Id" },
            { "Status.Name", "[RequestStatus].Name"},
            { "Team.Id", "[Team].Id" },
            { "Team.RaceClass.Id", "[RaceClass].Id" },
            { "Team.RaceClass.Name", "[RaceClass].Name"},
            { "RequestPerson.Id", "[RequestPerson].Id"},
            { "RequestPerson.Firstname", "[RequestPerson].Firstname"},
            { "RequestPerson.Lastname", "[RequestPerson].Lastname"},
            { "ReplacedPerson.Id", "[ReplacedPerson].Id"},
            { "ReplacedPerson.Firstname", "[ReplacedPerson].Firstname"},
            { "ReplacedPerson.Lastname", "[ReplacedPerson].Lastname"}
        };

        #endregion

        #region Constructors

        public CrewChangeRequestRepository(IContextResolver contextResolver, IQueryBuilder queryBuilder) : base(contextResolver, queryBuilder)
        {
        }

        #endregion

        #region ICrewChangeRequestRepository implementation

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
            return base.Exists(id, "CrewChangeRequest", "Id", context);
        }

        public bool ExistsDuplicate(CrewChangeRequest crewChangeRequest, ITransactionalContext? context = null)
        {
            throw new NotImplementedException();
        }

        public PaginatedResult<CrewChangeRequest> Get(ChangeRequestSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            return this.GetCrewChangeRequests(searchFilter: searchFilter, paginationFilter: paginationFilter, sorting: sorting, context: context);
        }

        public CrewChangeRequest? Get(int id, ITransactionalContext? context = null)
        {
            var searchFilter = new ChangeRequestSearchFilter() { Ids = new int[] { id } };

            return this.GetCrewChangeRequests(searchFilter: searchFilter, paginationFilter: null, sorting: null, context: context).Results.FirstOrDefault();
        }

        public void Create(CrewChangeRequest crewChangeRequest, ITransactionalContext? context = null)
        {
            this.CreateCrewChangeRequest(crewChangeRequest, context);
        }

        #endregion

        #region Private Methods

        private PaginatedResult<CrewChangeRequest> GetCrewChangeRequests(ChangeRequestSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            string sql = $@"SELECT
                                [CrewChangeRequest].Id [Id],
                                [CrewChangeRequest].ChangeReason [ChangeReason],
                                [CrewChangeRequest].ReplacementFullname [ReplacementFullname],
                                [CrewChangeRequest].CreationDate [CreationDate],
                                [CrewChangeRequest].ResolutionDate [ResolutionDate],
                                [CrewChangeRequest].ResolutionComments [ResolutionComments],
	                            [RequestStatus].Id [Id],
	                            [RequestStatus].[Name] [Name],
	                            [Team].Id [Id],
                                [RequestPerson].Id [Id],
	                            [RequestPerson].Firstname [Firstname],
	                            [RequestPerson].Lastname [Lastname],
                                [ReplacedPerson].Id [Id],
	                            [ReplacedPerson].Firstname [Firstname],
	                            [ReplacedPerson].Lastname [Lastname],
                                [ReplacedUser].Id [Id],
                                [File].Id [Id],
                                [File].Description [Description]
                            FROM [CrewChangeRequest] [CrewChangeRequest]
                            INNER JOIN [Team] [Team] ON [Team].Id = [CrewChangeRequest].IdTeam
                            INNER JOIN [RequestStatus] [RequestStatus] ON [RequestStatus].Id = [CrewChangeRequest].IdRequestStatus
                            INNER JOIN [User] [RequestUser]  ON [RequestUser].Id  = [CrewChangeRequest].IdRequestUser
                            INNER JOIN [User_Person] [User_Person1] ON [User_Person1].IdUser = [RequestUser].Id
                            INNER JOIN [Person] [RequestPerson] ON [RequestPerson].Id = [User_Person1].IdPerson
                            INNER JOIN [User] [ReplacedUser]  ON [ReplacedUser].Id  = [CrewChangeRequest].IdRequestUser
                            INNER JOIN [User_Person] [User_Person2] ON [User_Person2].IdUser = [ReplacedUser].Id
                            INNER JOIN [Person] [ReplacedPerson] ON [ReplacedPerson].Id = [User_Person2].IdPerson
                            LEFT JOIN [File] [File] ON [File].Id = [CrewChangeRequest].IdFile";

            QueryBuilder.AddCommand(sql);

            this.ProcessSearchFilter(searchFilter);

            QueryBuilder.AddSorting(sorting, _columnsMapping);
            QueryBuilder.AddPagination(paginationFilter);

            var crewChangeRequests = new List<CrewChangeRequest>();

            PaginatedResult<CrewChangeRequest> items = base.GetPaginatedResults<CrewChangeRequest>
                (
                    (reader) =>
                    {
                        return reader.Read<CrewChangeRequest, RequestStatus, Team, Person, Person, User, RaceBoard.Domain.File, CrewChangeRequest>
                        (
                            (crewChangeRequest, requestStatus, team, requestPerson, replacedPerson, replacedUser, file) =>
                            {
                                crewChangeRequest.Team = team;
                                crewChangeRequest.Status = requestStatus;
                                crewChangeRequest.RequestPerson = requestPerson;
                                crewChangeRequest.ReplacedUser = replacedUser;
                                crewChangeRequest.ReplacedPerson = replacedPerson;

                                crewChangeRequest.File = file;

                                crewChangeRequests.Add(crewChangeRequest);

                                return crewChangeRequest;
                            },
                            splitOn: "Id, Id, Id, Id, Id, Id, Id"
                        ).AsList();
                    },
                    context
                );

            items.Results = crewChangeRequests;

            return items;
        }

        private void ProcessSearchFilter(ChangeRequestSearchFilter? searchFilter)
        {
            if (searchFilter == null)
                return;

            base.AddFilterCriteria(ConditionType.In, "CrewChangeRequest", "Id", "ids", searchFilter.Ids);
            base.AddFilterCriteria(ConditionType.Equal, "Team", "Id", "idTeam", searchFilter.Team?.Id);
            base.AddFilterCriteria(ConditionType.Equal, "RequestStatus", "Id", "idRequestStatus", searchFilter.Status);
            base.AddFilterCriteria(ConditionType.GreaterOrEqualThan, "CrewChangeRequest", "CreationDate", "creationDate", searchFilter.CreationDate);
        }

        private void CreateCrewChangeRequest(CrewChangeRequest crewChangeRequest, ITransactionalContext? context = null)
        {
            string sql = @" INSERT INTO [CrewChangeRequest]
                            ( IdTeam, IdRequestUser, IdRequestStatus, IdFile, ChangeReason, CreationDate, IdReplacedUser, ReplacementFullname )
                        VALUES
                            ( @idTeam, @idRequestUser, @idRequestStatus, @idFile, @changeReason, @creationDate, @idReplacedUser, @replacementFullname )";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("idTeam", crewChangeRequest.Team.Id);
            QueryBuilder.AddParameter("idRequestUser", crewChangeRequest.RequestUser.Id);
            QueryBuilder.AddParameter("idRequestStatus", crewChangeRequest.Status.Id);
            QueryBuilder.AddParameter("changeReason", crewChangeRequest.ChangeReason);
            QueryBuilder.AddParameter("creationDate", crewChangeRequest.CreationDate);
            QueryBuilder.AddParameter("idFile", crewChangeRequest.File?.Id);
            QueryBuilder.AddParameter("idReplacedUser", crewChangeRequest.ReplacedUser.Id);
            QueryBuilder.AddParameter("replacementFullname", crewChangeRequest.ReplacementFullName);

            QueryBuilder.AddReturnLastInsertedId();

            base.Execute<int>(context);
        }

        #endregion
    }
}
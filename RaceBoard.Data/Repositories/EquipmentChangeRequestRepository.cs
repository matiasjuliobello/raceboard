using Dapper;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data.Helpers.Interfaces;
using RaceBoard.Data.Repositories.Base.Abstract;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;

namespace RaceBoard.Data.Repositories
{
    public class EquipmentChangeRequestRepository : AbstractRepository, IEquipmentChangeRequestRepository
    {
        #region Private Members

        private readonly Dictionary<string, string> _columnsMapping = new()
        {
            { "Id", "[EquipmentChangeRequest].Id" },
            { "ChangeReason", "[EquipmentChangeRequest].ChangeReason" },
            { "CreationDate", "[EquipmentChangeRequest].CreationDate" },
            { "ResolutionDate", "[EquipmentChangeRequest].ResolutionDate" },
            { "ResolutionComments", "[EquipmentChangeRequest].ResolutionComments"},
            { "Status.Id", "[RequestStatus].Id" },
            { "Status.Name", "[RequestStatus].Name"},
            { "Team.Id", "[Team].Id" },
            { "Team.RaceClass.Id", "[RaceClass].Id" },
            { "Team.RaceClass.Name", "[RaceClass].Name"},
            { "RequestPerson.Id", "[RequestPerson].Id"},
            { "RequestPerson.Firstname", "[RequestPerson].Firstname"},
            { "RequestPerson.Lastname", "[RequestPerson].Lastname"}
        };

        #endregion

        #region Constructors

        public EquipmentChangeRequestRepository(IContextResolver contextResolver, IQueryBuilder queryBuilder) : base(contextResolver, queryBuilder)
        {
        }

        #endregion

        #region IEquipmentChangeRequestRepository implementation

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
            return base.Exists(id, "EquipmentChangeRequest", "Id", context);
        }

        public bool ExistsDuplicate(EquipmentChangeRequest equipmentChangeRequest, ITransactionalContext? context = null)
        {
            throw new NotImplementedException();
        }

        public PaginatedResult<EquipmentChangeRequest> Get(ChangeRequestSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            return this.GetEquipmentChangeRequests(searchFilter: searchFilter, paginationFilter: paginationFilter, sorting: sorting, context: context);
        }

        public EquipmentChangeRequest? Get(int id, ITransactionalContext? context = null)
        {
            var searchFilter = new ChangeRequestSearchFilter() { Ids = new int[] { id } };

            return this.GetEquipmentChangeRequests(searchFilter: searchFilter, paginationFilter: null, sorting: null, context: context).Results.FirstOrDefault();
        }

        public void Create(EquipmentChangeRequest equipmentChangeRequest, ITransactionalContext? context = null)
        {
            this.CreateEquipmentChangeRequest(equipmentChangeRequest, context);
        }

        #endregion

        #region Private Methods

        private PaginatedResult<EquipmentChangeRequest> GetEquipmentChangeRequests(ChangeRequestSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            string sql = $@"SELECT
                                [EquipmentChangeRequest].Id [Id],
                                [EquipmentChangeRequest].ChangeReason [ChangeReason],
                                [EquipmentChangeRequest].ChangeRequested [ChangeRequested],
                                [EquipmentChangeRequest].CreationDate [CreationDate],
                                [EquipmentChangeRequest].ResolutionDate [ResolutionDate],
                                [EquipmentChangeRequest].ResolutionComments [ResolutionComments],
	                            [RequestStatus].Id [Id],
	                            [RequestStatus].[Name] [Name],
	                            [Team].Id [Id],
                                [RaceClass].Id [Id],
                                [RaceClass].Name [Name],
                                [RequestPerson].Id [Id],
	                            [RequestPerson].Firstname [Firstname],
	                            [RequestPerson].Lastname [Lastname],
                                [File].Id [Id],
                                [File].Description [Description]
                            FROM [EquipmentChangeRequest] [EquipmentChangeRequest]
                            INNER JOIN [Team] [Team] ON [Team].Id = [EquipmentChangeRequest].IdTeam
                            INNER JOIN [RaceClass] [RaceClass] ON [RaceClass].Id = [Team].IdRaceClass
                            INNER JOIN [RequestStatus] [RequestStatus] ON [RequestStatus].Id = [EquipmentChangeRequest].IdRequestStatus
                            INNER JOIN [User] [RequestUser]  ON [RequestUser].Id  = [EquipmentChangeRequest].IdRequestUser
                            INNER JOIN [User_Person] [User_Person1] ON [User_Person1].IdUser = [RequestUser].Id
                            INNER JOIN [Person] [RequestPerson] ON [RequestPerson].Id = [User_Person1].IdPerson
                            LEFT JOIN [File] [File] ON [File].Id = [EquipmentChangeRequest].IdFile";

            QueryBuilder.AddCommand(sql);

            this.ProcessSearchFilter(searchFilter);

            QueryBuilder.AddSorting(sorting, _columnsMapping);
            QueryBuilder.AddPagination(paginationFilter);

            var equipmentChangeRequests = new List<EquipmentChangeRequest>();

            PaginatedResult<EquipmentChangeRequest> items = base.GetPaginatedResults<EquipmentChangeRequest>
                (
                    (reader) =>
                    {
                        return reader.Read<EquipmentChangeRequest, ChangeRequestStatus, Team, RaceClass, Person, RaceBoard.Domain.File, EquipmentChangeRequest>
                        (
                            (equipmentChangeRequest, requestStatus, team, raceClass, requestPerson, file) =>
                            {
                                team.RaceClass = raceClass;

                                equipmentChangeRequest.Team = team;
                                equipmentChangeRequest.Status = requestStatus;
                                equipmentChangeRequest.RequestPerson = requestPerson;
                                equipmentChangeRequest.File = file;

                                equipmentChangeRequests.Add(equipmentChangeRequest);

                                return equipmentChangeRequest;
                            },
                            splitOn: "Id, Id, Id, Id, Id, Id"
                        ).AsList();
                    },
                    context
                );

            items.Results = equipmentChangeRequests;

            return items;
        }

        private void ProcessSearchFilter(ChangeRequestSearchFilter? searchFilter)
        {
            if (searchFilter == null)
                return;

            base.AddFilterCriteria(ConditionType.In, "EquipmentChangeRequest", "Id", "ids", searchFilter.Ids);
            base.AddFilterCriteria(ConditionType.Equal, "Team", "Id", "idTeam", searchFilter.Team?.Id);
        }

        private void CreateEquipmentChangeRequest(EquipmentChangeRequest equipmentChangeRequest, ITransactionalContext? context = null)
        {
            string sql = @" INSERT INTO [EquipmentChangeRequest]
                            ( IdTeam, IdRequestUser, IdRequestStatus, IdFile, ChangeRequested, ChangeReason, CreationDate )
                        VALUES
                            ( @idTeam, @idRequestUser, @idRequestStatus, @idFile, @changeRequested, @changeReason, @creationDate )";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("idTeam", equipmentChangeRequest.Team.Id);
            QueryBuilder.AddParameter("idRequestUser", equipmentChangeRequest.RequestUser.Id);
            QueryBuilder.AddParameter("idRequestStatus", equipmentChangeRequest.Status.Id);
            QueryBuilder.AddParameter("changeRequested", equipmentChangeRequest.ChangeRequested);
            QueryBuilder.AddParameter("changeReason", equipmentChangeRequest.ChangeReason);
            QueryBuilder.AddParameter("creationDate", equipmentChangeRequest.CreationDate);
            QueryBuilder.AddParameter("idFile", equipmentChangeRequest.File?.Id);

            QueryBuilder.AddReturnLastInsertedId();

            base.Execute<int>(context);
        }

        #endregion
    }
}
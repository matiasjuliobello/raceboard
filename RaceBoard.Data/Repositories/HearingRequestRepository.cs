using Dapper;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data.Helpers.Interfaces;
using RaceBoard.Data.Repositories.Base.Abstract;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;

namespace RaceBoard.Data.Repositories
{
    public class HearingRequestRepository : AbstractRepository, IHearingRequestRepository
    {
        #region Private Members

        private readonly Dictionary<string, string> _columnsMapping = new()
        {
            { "Id", "[HearingRequest].Id" },
            { "ChangeReason", "[HearingRequest].ChangeReason" },
            { "CreationDate", "[HearingRequest].CreationDate" },
            { "ResolutionDate", "[HearingRequest].ResolutionDate" },
            { "ResolutionComments", "[HearingRequest].ResolutionComments"},
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

        public HearingRequestRepository(IContextResolver contextResolver, IQueryBuilder queryBuilder) : base(contextResolver, queryBuilder)
        {
        }

        #endregion

        #region IHearingRequestRepository implementation

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
            return base.Exists(id, "HearingRequest", "Id", context);
        }

        public bool ExistsDuplicate(HearingRequest hearingRequest, ITransactionalContext? context = null)
        {
            throw new NotImplementedException();
        }

        public PaginatedResult<HearingRequest> Get(HearingRequestSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            return this.GetHearingRequests(searchFilter: searchFilter, paginationFilter: paginationFilter, sorting: sorting, context: context);
        }

        public HearingRequest? Get(int id, ITransactionalContext? context = null)
        {
            var searchFilter = new HearingRequestSearchFilter() { Ids = new int[] { id } };

            return this.GetHearingRequests(searchFilter: searchFilter, paginationFilter: null, sorting: null, context: context).Results.FirstOrDefault();
        }

        public void Create(HearingRequest hearingRequest, ITransactionalContext? context = null)
        {
            this.CreateHearingRequest(hearingRequest, context);
        }

        public void CreateProtestor(HearingRequest hearingRequest, ITransactionalContext? context = null)
        {
            this.CreateHearingRequestProtestor(hearingRequest, context);
        }

        public void CreateProtestorNotice(HearingRequest hearingRequest, ITransactionalContext? context = null)
        {
            this.CreateHearingRequestProtestorNotice(hearingRequest, context);
        }

        public void CreateProtestees(HearingRequest hearingRequest, ITransactionalContext? context = null)
        {
            if (hearingRequest.Protestees == null)
                return;

            foreach(var protestee in hearingRequest.Protestees.Protestees)
            {
                this.CreateHearingRequestProtestee(hearingRequest, protestee, context);
            }
        }

        public void CreateIncident(HearingRequest hearingRequest, ITransactionalContext? context = null)
        {
            this.CreateHearingRequestIncident(hearingRequest, context);
        }

        #endregion

        #region Private Methods

        private PaginatedResult<HearingRequest> GetHearingRequests(HearingRequestSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            string sql = $@"SELECT
                                [HearingRequest].Id [Id],
                                [HearingRequest].ChangeReason [ChangeReason],
                                [HearingRequest].ChangeRequested [ChangeRequested],
                                [HearingRequest].CreationDate [CreationDate],
                                [HearingRequest].ResolutionDate [ResolutionDate],
                                [HearingRequest].ResolutionComments [ResolutionComments],
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
                            FROM [HearingRequest] [HearingRequest]
                            INNER JOIN [Team] [Team] ON [Team].Id = [HearingRequest].IdTeam
                            INNER JOIN [RaceClass] [RaceClass] ON [RaceClass].Id = [Team].IdRaceClass
                            INNER JOIN [RequestStatus] [RequestStatus] ON [RequestStatus].Id = [HearingRequest].IdRequestStatus
                            INNER JOIN [User] [RequestUser]  ON [RequestUser].Id  = [HearingRequest].IdRequestUser
                            INNER JOIN [User_Person] [User_Person1] ON [User_Person1].IdUser = [RequestUser].Id
                            INNER JOIN [Person] [RequestPerson] ON [RequestPerson].Id = [User_Person1].IdPerson
                            LEFT JOIN [File] [File] ON [File].Id = [HearingRequest].IdFile";

            QueryBuilder.AddCommand(sql);

            this.ProcessSearchFilter(searchFilter);

            QueryBuilder.AddSorting(sorting, _columnsMapping);
            QueryBuilder.AddPagination(paginationFilter);

            var hearingRequests = new List<HearingRequest>();

            PaginatedResult<HearingRequest> items = base.GetPaginatedResults<HearingRequest>
                (
                    (reader) =>
                    {
                        return reader.Read<HearingRequest, ChangeRequestStatus, Team, RaceClass, Person, RaceBoard.Domain.File, HearingRequest>
                        (
                            (hearingRequest, requestStatus, team, raceClass, requestPerson, file) =>
                            {
                                team.RaceClass = raceClass;

                                hearingRequest.Team = team;
                                //hearingRequest.Status = requestStatus;
                                hearingRequest.RequestPerson = requestPerson;

                                hearingRequests.Add(hearingRequest);

                                return hearingRequest;
                            },
                            splitOn: "Id, Id, Id, Id, Id, Id"
                        ).AsList();
                    },
                    context
                );

            items.Results = hearingRequests;

            return items;
        }

        private void ProcessSearchFilter(HearingRequestSearchFilter? searchFilter)
        {
            if (searchFilter == null)
                return;

            base.AddFilterCriteria(ConditionType.In, "HearingRequest", "Id", "ids", searchFilter.Ids);
            base.AddFilterCriteria(ConditionType.Equal, "Team", "Id", "idTeam", searchFilter.Team?.Id);
        }

        private void CreateHearingRequest(HearingRequest hearingRequest, ITransactionalContext? context = null)
        {
            string sql = @" INSERT INTO [HearingRequest]
                            ( IdTeam, IdRequestUser, IdRequestStatus, IdHearingRequestType, CreationDate, RaceNumber )
                        VALUES
                            ( @idTeam, @idRequestUser, @idRequestStatus, @idHearingRequestType, @creationDate, @raceNumber )";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("idTeam", hearingRequest.Team.Id);
            QueryBuilder.AddParameter("idRequestUser", hearingRequest.RequestUser.Id);
            QueryBuilder.AddParameter("idRequestStatus", hearingRequest.Status.Id);
            QueryBuilder.AddParameter("idHearingRequestType", hearingRequest.Type.Id);
            QueryBuilder.AddParameter("creationDate", hearingRequest.CreationDate);
            QueryBuilder.AddParameter("raceNumber", hearingRequest.RaceNumber);

            QueryBuilder.AddReturnLastInsertedId();

            hearingRequest.Id = base.Execute<int>(context);
        }

        private void CreateHearingRequestProtestor(HearingRequest hearingRequest, ITransactionalContext? context = null)
        {
            string sql = @" INSERT INTO [HearingRequest_Protestor]
                            ( IdHearingRequest, IdBoat, Address, PhoneNumber )
                        VALUES
                            ( @idHearingRequest, @idBoat, @address, @phoneNumber )";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("idHearingRequest", hearingRequest.Id);
            QueryBuilder.AddParameter("idBoat", hearingRequest.Protestor.Boat.Id);
            QueryBuilder.AddParameter("address", hearingRequest.Protestor.Address);
            QueryBuilder.AddParameter("phoneNumber", hearingRequest.Protestor.PhoneNumber);

            QueryBuilder.AddReturnLastInsertedId();

            hearingRequest.Protestor.Id = base.Execute<int>(context);
        }

        private void CreateHearingRequestProtestorNotice(HearingRequest hearingRequest, ITransactionalContext? context = null)
        {
            string sql = @" INSERT INTO [HearingRequest_ProtestorNotice]
                            ( IdHearingRequest, Hailing, HailingWordsUsed, HailingWhen, RedFlag, RedFlagWhen, Other, OtherWhen, OtherWhere, OtherHow )
                        VALUES
                            ( @idHearingRequest, @hailing, @hailingWordsUsed, @hailingWhen, @redFlag, @redFlagWhen, @other, @otherWhen, @otherWhere, @otherHow )";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("idHearingRequest", hearingRequest.Id);
            QueryBuilder.AddParameter("hailing", hearingRequest.Protestor.Notice.Hailing);
            QueryBuilder.AddParameter("hailingWordsUsed", hearingRequest.Protestor.Notice.HailingWordsUsed);
            QueryBuilder.AddParameter("hailingWhen", hearingRequest.Protestor.Notice.HailingWhen);
            QueryBuilder.AddParameter("redFlag", hearingRequest.Protestor.Notice.RedFlag);
            QueryBuilder.AddParameter("redFlagWhen", hearingRequest.Protestor.Notice.RedFlagWhen);
            QueryBuilder.AddParameter("other", hearingRequest.Protestor.Notice.Other);
            QueryBuilder.AddParameter("otherWhen", hearingRequest.Protestor.Notice.OtherWhen);
            QueryBuilder.AddParameter("otherWhere", hearingRequest.Protestor.Notice.OtherWhere);
            QueryBuilder.AddParameter("otherHow", hearingRequest.Protestor.Notice.OtherHow);

            QueryBuilder.AddReturnLastInsertedId();

            hearingRequest.Protestor.Notice.Id = base.Execute<int>(context);
        }

        private void CreateHearingRequestIncident(HearingRequest hearingRequest, ITransactionalContext? context = null)
        {
            string sql = @" INSERT INTO [HearingRequest_Incident]
                            ( IdHearingRequest, Time, Place, BrokenRules, Witnesses, Details )
                        VALUES
                            ( @idHearingRequest, @time, @place, @brokenRules, @witnesses, @details )";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("idHearingRequest", hearingRequest.Id);
            QueryBuilder.AddParameter("time", hearingRequest.Incident.Time);
            QueryBuilder.AddParameter("place", hearingRequest.Incident.Place);
            QueryBuilder.AddParameter("brokenRules", hearingRequest.Incident.BrokenRules);
            QueryBuilder.AddParameter("witnesses", hearingRequest.Incident.Witnesses);
            QueryBuilder.AddParameter("details", hearingRequest.Incident.Details);

            QueryBuilder.AddReturnLastInsertedId();

            hearingRequest.Incident.Id = base.Execute<int>(context);
        }

        private void CreateHearingRequestProtestee(HearingRequest hearingRequest, HearingRequestProtestee hearingRequestProtestee, ITransactionalContext? context = null)
        {
            string sql = @" INSERT INTO [HearingRequest_Protestee]
                            ( IdHearingRequest, IdBoat )
                        VALUES
                            ( @idHearingRequest, @idBoat )";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("idHearingRequest", hearingRequest.Id);
            QueryBuilder.AddParameter("idBoat", hearingRequestProtestee.Boat.Id);
            
            QueryBuilder.AddReturnLastInsertedId();

            hearingRequestProtestee.Id = base.Execute<int>(context);
        }

        #endregion
    }
}
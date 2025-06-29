using Dapper;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data.Helpers.Interfaces;
using RaceBoard.Data.Repositories.Base.Abstract;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;
using System;
using System.Text;
using static Dapper.SqlMapper;

namespace RaceBoard.Data.Repositories
{
    public class HearingRequestRepository : AbstractRepository, IHearingRequestRepository
    {
        #region Private Members

        private readonly Dictionary<string, string> _columnsMapping = new()
        {
            { "Id", "[Hearing].Id" },
            { "CreationDate", "[Hearing].CreationDate" },
            { "RequestNumber", "[Hearing].RequestNumber" },
            { "RaceNumber", "[Hearing].RaceNumber" },
            { "Status.Id", "[Hearing].IdRequestStatus" },
            { "Status.Name", "[Hearing].IdRequestStatus" },
            { "Type.Id", "[Hearing].IdRequestType" },
            { "Type.Name", "[Hearing].IdRequestType" },
            { "Protestor.User.Id", "[ProtestorUser].Id" },
            { "Protestor.Person.Id", "[ProtestorPerson].Id" },
            { "Protestor.Person.Firstname", "[ProtestorPerson].Firstname" },
            { "Protestor.Person.Lastname", "[ProtestorPerson].Lastname" },
            { "Protestor.Boat.Id", "[ProtestorBoat].Id" },
            { "Protestor.Boat.Name", "[ProtestorBoat].Name"},
            { "Protestor.Boat.SailNumber", "[ProtestorBoat].SailNumber" },
            { "Protestor.Boat.HullNumber", "[ProtestorBoat].HullNumber" },
            { "Team.Id", "[ProtestorTeam].Id" },
            { "Team.RaceClass.Id", "[ProtestorTeamRaceClass].Id" },
            { "Team.RaceClass.Name", "[ProtestorTeamRaceClass].Name"}
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

        public HearingRequestProtestor GetProtestor(int id, ITransactionalContext? context = null)
        {
            return this.GetHearingRequestProtestor(id, context);
        }

        public HearingRequestProtestees GetProtestees(int id, ITransactionalContext? context = null)
        {
            return this.GetHearingRequestProtestees(id, context);
        }

        public HearingRequestIncident GetIncident(int id, ITransactionalContext? context = null)
        {
            return this.GetHearingRequestIncident(id, context);
        }

        public CommitteeBoatReturn GetAssociatedCommitteeBoatReturn(int id, ITransactionalContext? context = null)
        {
            return this.GetHearingRequestAssociatedCommitteeBoatReturn(id, context);
        }

        public HearingRequestWithdrawal GetWithdrawal(int id, ITransactionalContext? context = null)
        {
            return this.GetHearingRequestWithdrawal(id, context);
        }

        public HearingRequestLodgement GetLodgement(int id, ITransactionalContext? context = null)
        {
            return this.GetHearingRequestLodgement(id, context);
        }

        public HearingRequestAttendees GetAttendees(int id, ITransactionalContext? context = null)
        {
            return this.GetHearingRequestAttendees(id, context);
        }

        public HearingRequestValidity GetValidity(int id, ITransactionalContext? context = null)
        {
            return this.GetHearingRequestValidity(id, context);
        }

        public HearingRequestResolution GetResolution(int id, ITransactionalContext? context = null)
        {
            return this.GetHearingRequestResolution(id, context);
        }

        public PaginatedResult<HearingRequest> FindHearingRequestsIncludingTeamBoat(int idTeamBoat, ITransactionalContext? context = null)
        {
            var searchFilter = new HearingRequestSearchFilter()
            {
                Protestees = new HearingRequestProtestees()
                {
                    Protestees = new List<HearingRequestProtestee>()
                    {
                        new HearingRequestProtestee() { TeamBoat = new TeamBoat() { Id = idTeamBoat } }
                    }
                }
            };

            return this.GetHearingRequests(searchFilter: searchFilter, paginationFilter: null, sorting: null, context: context);
        }

        public void Create(HearingRequest hearingRequest, ITransactionalContext? context = null)
        {
            this.CreateHearingRequest(hearingRequest, context);
        }

        public void UpdateStatus(HearingRequest hearingRequest, ITransactionalContext? context = null)
        {
            this.UpdateHearingRequestStatus(hearingRequest, context);
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

            foreach (var protestee in hearingRequest.Protestees.Protestees)
            {
                this.CreateHearingRequestProtestee(hearingRequest, protestee, context);
            }
        }

        public void CreateIncident(HearingRequest hearingRequest, ITransactionalContext? context = null)
        {
            this.CreateHearingRequestIncident(hearingRequest, context);
        }

        public void CreateCommitteeBoatReturnAssociation(HearingRequest hearingRequest, CommitteeBoatReturn commiteeBoatReturn, ITransactionalContext? context = null)
        {
            this.CreateHearingRequestCommitteeBoatReturn(hearingRequest, commiteeBoatReturn, context);
        }

        public void CreateRequestWithdrawal(HearingRequest hearingRequest, ITransactionalContext? context = null)
        {
            this.CreateHearingRequestWithdrawal(hearingRequest, context);
        }

        public void CreateRequestLodgement(HearingRequest hearingRequest, ITransactionalContext? context = null)
        {
            this.CreateHearingRequestLodgement(hearingRequest, context);
        }

        public void CreateRequestAttendees(HearingRequest hearingRequest, ITransactionalContext? context = null)
        {
            this.CreateHearingRequestAttendees(hearingRequest, context);
        }

        public void CreateRequestValidity(HearingRequest hearingRequest, ITransactionalContext? context = null)
        {
            this.CreateHearingRequestValidity(hearingRequest, context);
        }

        public void CreateRequestResolution(HearingRequest hearingRequest, ITransactionalContext? context = null)
        {
            this.CreateHearingRequestResolution(hearingRequest, context);
        }

        #endregion

        #region Private Methods

        private PaginatedResult<HearingRequest> GetHearingRequests(HearingRequestSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            string sql = $@"SELECT
                                [Hearing].Id [Id],                                
                                [Hearing].CreationDate,
                                [Hearing].RequestNumber [RequestNumber],
                                [Hearing].RaceNumber,
                                [RequestStatus].Id [Id],
                                [RequestStatus].Name [Name],
                                [RequestType].Id [Id],
                                [RequestType].Name [Name],
                                [RequestUser].Id [Id],
                                [RequestPerson].Id [Id],
                                [RequestPerson].Firstname [Firstname],
                                [RequestPerson].Lastname [Lastname],
                                [RequestTeam].Id [Id],
                                [RequestTeamRaceClass].Id [Id],
                                [RequestTeamRaceClass].Name [Name]
                            FROM [HearingRequest] [Hearing]
                            INNER JOIN [HearingRequestType] [RequestType]   ON [RequestType].Id = [Hearing].IdHearingRequestType
                            INNER JOIN [RequestStatus] [RequestStatus]      ON [RequestStatus].Id = [Hearing].IdRequestStatus                           
                            INNER JOIN [User] [RequestUser]                 ON [RequestUser].Id = [Hearing].IdRequestUser
                            INNER JOIN [User_Person] [User_Person]          ON [RequestUser].Id = [User_Person].IdUser
                            INNER JOIN [Person] [RequestPerson]             ON [RequestPerson].Id = [User_Person].IdPerson
                            INNER JOIN [Team] [RequestTeam]                 ON [RequestTeam].Id = [Hearing].IdTeam
                            INNER JOIN [RaceClass] [RequestTeamRaceClass]   ON [RequestTeamRaceClass].Id = [RequestTeam].IdRaceClass";

            QueryBuilder.AddCommand(sql);

            this.ProcessSearchFilter(searchFilter);

            QueryBuilder.AddSorting(sorting, _columnsMapping);
            QueryBuilder.AddPagination(paginationFilter);

            var hearingRequests = new List<HearingRequest>();

            PaginatedResult<HearingRequest> items = base.GetPaginatedResults<HearingRequest>
                (
                    (reader) =>
                    {
                        return reader.Read<HearingRequest, HearingRequestStatus, HearingRequestType, User, Person, Team, RaceClass, HearingRequest>
                        (
                            (hearingRequest, status, type, user, person, team, teamRaceClass) =>
                            {

                                hearingRequest.Status = status;
                                hearingRequest.Type = type;
                                hearingRequest.RequestUser = user;
                                hearingRequest.RequestPerson = person;
                                hearingRequest.Team = team;
                                hearingRequest.Team.RaceClass = teamRaceClass;

                                hearingRequests.Add(hearingRequest);

                                return hearingRequest;
                            },
                            splitOn: "Id, Id, Id, Id, Id, Id, Id"
                        ).AsList();
                    },
                    context
                );

            sql = $@"SELECT
                        [HearingRequest_CommitteeBoatReturn].[Id],
                        [Hearing].Id [Id], 
	                    [CommitteeBoatReturn].Id [Id],
	                    [CommitteeBoatReturn].ReturnTime [ReturnTime],
	                    [CommitteeBoatReturn].Name [Name]
                    FROM [HearingRequest] [Hearing]
                    INNER JOIN [HearingRequest_CommitteeBoatReturn] [HearingRequest_CommitteeBoatReturn] ON [HearingRequest_CommitteeBoatReturn].IdHearingRequest = [Hearing].Id
                    INNER JOIN [CommitteeBoatReturn] [CommitteeBoatReturn] ON [CommitteeBoatReturn].Id = [HearingRequest_CommitteeBoatReturn].IdCommitteeBoatReturn";

            QueryBuilder.AddCommand(sql);
            QueryBuilder.AddCondition("[Hearing].Id IN @idHearings");
            QueryBuilder.AddParameter("idHearings", hearingRequests.Select(x => x.Id));

            var committeeBoatReturns = new List<HearingRequestCommitteeBoatReturn>();
            QueryBuilder.AddPagination(paginationFilter);
            base.GetPaginatedResults<HearingRequestCommitteeBoatReturn>
               (
                   (reader) =>
                   {
                       return reader.Read<HearingRequestCommitteeBoatReturn, HearingRequest, CommitteeBoatReturn, HearingRequestCommitteeBoatReturn>
                       (
                           (hearingRequestCommitteeBoatReturn, hearingRequest, committeeBoatReturn) =>
                           {
                               hearingRequestCommitteeBoatReturn.HearingRequest = hearingRequest;
                               hearingRequestCommitteeBoatReturn.CommitteeBoatReturn = committeeBoatReturn;

                               committeeBoatReturns.Add(hearingRequestCommitteeBoatReturn);

                               return hearingRequestCommitteeBoatReturn;
                           },
                           splitOn: "Id, Id, Id"
                       ).AsList();
                   },
                   context
               );

            foreach (var item in hearingRequests)
            {
                var committeeBoatReturnAssociation = committeeBoatReturns.FirstOrDefault(x => x.HearingRequest.Id == item.Id);
                if (committeeBoatReturnAssociation != null)
                    item.CommitteeBoatReturn = committeeBoatReturnAssociation.CommitteeBoatReturn;
            }

            items.Results = hearingRequests;

            return items;
        }

        private HearingRequestProtestor GetHearingRequestProtestor(int id, ITransactionalContext? context = null)
        {
            string sql = $@"SELECT
                                [Protestor].Id [Id],
                                [Protestor].Address [Address],
                                [Protestor].PhoneNumber [PhoneNumber],
                                [ProtestorUser].Id [Id],
                                [ProtestorPerson].Id [Id],
                                [ProtestorPerson].Firstname [Firstname],
                                [ProtestorPerson].Lastname [Lastname],
                                [ProtestorTeam].Id [Id],
	                            [ProtestorTeamRaceClass].Id [Id],
	                            [ProtestorTeamRaceClass].Id [Name],
                                [ProtestorBoat].Id [Id],
                                [ProtestorBoat].Name [Name],
                                [ProtestorBoat].SailNumber [SailNumber],
                                [ProtestorNotice].Id [Id],                                
                                [ProtestorNotice].Hailing [Hailing],
                                [ProtestorNotice].HailingWordsUsed [HailingWordsUsed],
                                [ProtestorNotice].HailingWhen [HailingWhen],
                                [ProtestorNotice].RedFlag [RedFlag],
                                [ProtestorNotice].RedFlagWhen [RedFlagWhen],
                                [ProtestorNotice].Other [Other],
                                [ProtestorNotice].OtherWhen [OtherWhen],
                                [ProtestorNotice].OtherWhere [OtherWhere],
                                [ProtestorNotice].OtherHow [OtherHow]
                            FROM [HearingRequest] [Hearing]
                            INNER JOIN [HearingRequest_Protestor] [Protestor]               ON [Protestor].IdHearingRequest = [Hearing].Id
                            INNER JOIN [HearingRequest_ProtestorNotice] [ProtestorNotice]   ON [ProtestorNotice].IdHearingRequest = [Hearing].Id                            
                            INNER JOIN [User] [ProtestorUser]                               ON [ProtestorUser].Id = [Hearing].IdRequestUser
                            INNER JOIN [User_Person] [User_Person]                          ON [ProtestorUser].Id = [User_Person].IdUser
                            INNER JOIN [Person] [ProtestorPerson]                           ON [ProtestorPerson].Id = [User_Person].IdPerson
                            INNER JOIN [Team_Boat] [ProtestorTeamBoat]                      ON [ProtestorTeamBoat].Id = [Protestor].IdTeamBoat
                            INNER JOIN [Boat] [ProtestorBoat]								ON [ProtestorBoat].Id = [ProtestorTeamBoat].IdBoat
                            INNER JOIN [Team] [ProtestorTeam]                               ON [ProtestorTeam].Id = [Hearing].IdTeam
                            INNER JOIN [RaceClass] [ProtestorTeamRaceClass]					ON [ProtestorTeamRaceClass].Id = [ProtestorTeam].IdRaceClass";

            QueryBuilder.AddCommand(sql);
            QueryBuilder.AddCondition("[Hearing].Id = @idHearing");
            QueryBuilder.AddParameter("idHearing", id);

            var protestor = new HearingRequestProtestor();

            base.GetReader
                (
                    (x) =>
                    {
                        protestor = x.Read<HearingRequestProtestor, User, Person, Team, RaceClass, Boat, HearingRequestProtestorNotice, HearingRequestProtestor>
                        (
                            (protestor, user, person, team, raceClass, boat, notice) =>
                            {
                                team.RaceClass = raceClass;
                                protestor.HearingRequest = new HearingRequest()
                                {
                                    Team = team,
                                    RequestUser = user,
                                    RequestPerson = person
                                };
                                protestor.TeamBoat = new TeamBoat() { Boat = boat };
                                protestor.Notice = notice;

                                return protestor;
                            },
                            splitOn: "Id, Id, Id, Id, Id, Id, Id"
                        ).FirstOrDefault();
                    },
                    context
                );

            return protestor;
        }

        private HearingRequestProtestees GetHearingRequestProtestees(int id, ITransactionalContext? context = null)
        {
            string sql = $@"SELECT
                                [Protestee].Id [Id],
                                [ProtesteeTeamBoat].Id [Id],
                                [ProtesteeTeam].Id [Id],
                                [ProtesteeBoat].Id [Id],
                                [ProtesteeBoat].Name [Name],
                                [ProtesteeBoat].SailNumber [SailNumber],
                                [ProtesteeBoat].HullNumber [HullNumber],
                                [ProtesteeBoatRaceClass].Id [Id],
                                [ProtesteeBoatRaceClass].Name [Name]
                            FROM [HearingRequest] [Hearing]
                            INNER JOIN [HearingRequest_Protestee] [Protestee]    ON [Protestee].IdHearingRequest = [Hearing].Id
                            INNER JOIN [Team_Boat] [ProtesteeTeamBoat]           ON [ProtesteeTeamBoat].Id = [Protestee].IdTeamBoat
                            INNER JOIN [Team] [ProtesteeTeam]                    ON [ProtesteeTeam].Id = [ProtesteeTeamBoat].IdTeam
                            INNER JOIN [Boat] [ProtesteeBoat]                    ON [ProtesteeBoat].Id = [ProtesteeTeamBoat].IdBoat
                            INNER JOIN [RaceClass] [ProtesteeBoatRaceClass]      ON [ProtesteeBoatRaceClass].Id = [ProtesteeBoat].IdRaceClass";

            QueryBuilder.AddCommand(sql);
            QueryBuilder.AddCondition("[Hearing].Id = @idHearing");
            QueryBuilder.AddParameter("idHearing", id);

            var protestees = new HearingRequestProtestees();

            base.GetReader
                (
                    (x) =>
                    {
                        protestees.Protestees = x.Read<HearingRequestProtestee, TeamBoat, Team, Boat, RaceClass, HearingRequestProtestee>
                        (
                            (protestee, teamBoat, team, boat, raceClass) =>
                            {
                                boat.RaceClass = raceClass;
                                teamBoat.Boat = boat;
                                teamBoat.Team = team;
                                protestee.TeamBoat = teamBoat;

                                return protestee;
                            },
                            splitOn: "Id, Id, Id, Id, Id"
                        ).ToList();
                    },
                    context
                );

            return protestees;
        }

        private HearingRequestIncident GetHearingRequestIncident(int id, ITransactionalContext? context = null)
        {
            string sql = $@"SELECT
                                [Incident].Id [Id],
                                [Incident].Time [Time],
                                [Incident].Place [Place],
                                [Incident].BrokenRules [BrokenRules],
                                [Incident].Witnesses [Witnesses],
                                [Incident].Details [Details]
                            FROM [HearingRequest] [Hearing]
                            INNER JOIN [HearingRequest_Incident] [Incident] ON [Incident].IdHearingRequest = [Hearing].Id";

            QueryBuilder.AddCommand(sql);
            QueryBuilder.AddCondition("[Hearing].Id = @idHearing");
            QueryBuilder.AddParameter("idHearing", id);

            return base.GetSingleResult<HearingRequestIncident>(context);
        }

        private CommitteeBoatReturn GetHearingRequestAssociatedCommitteeBoatReturn(int id, ITransactionalContext? context = null)
        {
            string sql = $@"SELECT
	                            [CommitteeBoatReturn].Id [Id],
	                            [CommitteeBoatReturn].ReturnTime [ReturnTime],
	                            [CommitteeBoatReturn].Name [Name]
                            FROM [HearingRequest] [Hearing]
                            INNER JOIN [HearingRequest_CommitteeBoatReturn] [HearingRequest_CommitteeBoatReturn] ON [HearingRequest_CommitteeBoatReturn].IdHearingRequest = [Hearing].Id
                            INNER JOIN [CommitteeBoatReturn] [CommitteeBoatReturn] ON [CommitteeBoatReturn].Id = [HearingRequest_CommitteeBoatReturn].IdCommitteeBoatReturn";

            QueryBuilder.AddCommand(sql);
            QueryBuilder.AddCondition("[Hearing].Id = @idHearing");
            QueryBuilder.AddParameter("idHearing", id);

            return base.GetSingleResult<CommitteeBoatReturn>(context);
        }

        private HearingRequestWithdrawal GetHearingRequestWithdrawal(int id, ITransactionalContext? context = null)
        {
            string sql = $@"SELECT
                                [Withdrawal].Id [Id],
                                [Withdrawal].IsRequested [IsRequested],
                                [Withdrawal].IsAuthorized [IsAuthorized]
                            FROM [HearingRequest] [Hearing]
                            INNER JOIN [HearingRequest_Withdrawal] [Withdrawal] ON [Withdrawal].IdHearingRequest = [Hearing].Id";

            QueryBuilder.AddCommand(sql);
            QueryBuilder.AddCondition("[Hearing].Id = @idHearing");
            QueryBuilder.AddParameter("idHearing", id);

            return base.GetSingleResult<HearingRequestWithdrawal>(context);
        }

        private HearingRequestLodgement GetHearingRequestLodgement(int id, ITransactionalContext? context = null)
        {
            string sql = $@"SELECT
                            [Lodgement].Id [Id],
                            [Lodgement].Deadline [Deadline],
                            [Lodgement].IsInTerm [IsInTerm],
	                        [Lodgement].HasExtension [HasExtension]
                        FROM [HearingRequest] [Hearing]
                        INNER JOIN [HearingRequest_Lodgement] [Lodgement] ON [Lodgement].IdHearingRequest = [Hearing].Id";

            QueryBuilder.AddCommand(sql);
            QueryBuilder.AddCondition("[Hearing].Id = @idHearing");
            QueryBuilder.AddParameter("idHearing", id);

            return base.GetSingleResult<HearingRequestLodgement>(context);
        }

        private HearingRequestAttendees GetHearingRequestAttendees(int id, ITransactionalContext? context = null)
        {
            string sql = $@"SELECT
                            [Attendees].Id [Id],
                            [Attendees].Protestors [Protestors],
                            [Attendees].Protestees [Protestees],
	                        [Attendees].Witnesses [Witnesses],
	                        [Attendees].Interpreters [Interpreters]
                        FROM [HearingRequest] [Hearing]
                        INNER JOIN [HearingRequest_Attendees] [Attendees] ON [Attendees].IdHearingRequest = [Hearing].Id";

            QueryBuilder.AddCommand(sql);
            QueryBuilder.AddCondition("[Hearing].Id = @idHearing");
            QueryBuilder.AddParameter("idHearing", id);

            return base.GetSingleResult<HearingRequestAttendees>(context);
        }

        private HearingRequestValidity GetHearingRequestValidity(int id, ITransactionalContext? context = null)
        {
            string sql = $@"SELECT
                                [Validity].Id [Id],
	                            [Validity].IsInterestedPartyObjection,		[Validity].InterestedPartyObjectionObservations,
	                            [Validity].DidProtestIdentifyIncident,		[Validity].ProtestIdentifyObservations,
	                            [Validity].WasObjectionSaidAloud,			[Validity].ObjectionSaidAloudObservations,
	                            [Validity].DidProtestorGiveNotice,			[Validity].ProtestorGiveNoticeObservations,
	                            [Validity].WasRedFlagWasDisplayed,			[Validity].RedFlagWasDisplayedObservations,
	                            [Validity].WasRedFlagSeenByRaceCommission,	[Validity].RedFlagSeenByRaceCommissionObservations,
	                            [Validity].IsValidProtest
                            FROM [HearingRequest] [Hearing]
                            INNER JOIN [HearingRequest_Validity] [Validity] ON [Validity].IdHearingRequest = [Hearing].Id";

            QueryBuilder.AddCommand(sql);
            QueryBuilder.AddCondition("[Hearing].Id = @idHearing");
            QueryBuilder.AddParameter("idHearing", id);

            return base.GetSingleResult<HearingRequestValidity>(context);
        }

        private HearingRequestResolution GetHearingRequestResolution(int id, ITransactionalContext? context = null)
        {
            string sql = $@"SELECT
                                [Resolution].Id [Id],
                                [Resolution].AcceptedFacts [AcceptedFacts],	                            
                                [Resolution].CommissionAcceptsShipSchematic [CommissionAcceptsShipSchematic],
	                            [Resolution].CommissionAttachesOwnSchematic [CommissionAttachesOwnSchematic],
	                            [Resolution].Comments [Comments],
	                            [Resolution].Dismissed [Dismissed],
	                            [Resolution].ProtestedBoatsAreDisqualified [ProtestedBoatsAreDisqualified],
	                            [Resolution].PenaltiesAreAssessed [PenaltiesAreAssessed],
	                            [Resolution].PenaltiesDescription [PenaltiesDescription],
                                [Resolution].CommissionChairmanAndOthers [CommissionChairmanAndOthers],
                                [Resolution].ResolutionDate [ResolutionDate]
                            FROM [HearingRequest] [Hearing]
                            INNER JOIN [HearingRequest_Resolution] [Resolution] ON [Resolution].IdHearingRequest = [Hearing].Id";

            QueryBuilder.AddCommand(sql);
            QueryBuilder.AddCondition("[Hearing].Id = @idHearing");
            QueryBuilder.AddParameter("idHearing", id);

            return base.GetSingleResult<HearingRequestResolution>(context);
        }

        private void ProcessSearchFilter(HearingRequestSearchFilter? searchFilter)
        {
            if (searchFilter == null)
                return;

            base.AddFilterCriteria(ConditionType.In, "Hearing", "Id", "ids", searchFilter.Ids);
            base.AddFilterCriteria(ConditionType.Equal, "RequestTeam", "Id", "idTeam", searchFilter.Team?.Id);
            base.AddFilterCriteria(ConditionType.Equal, "RequestTeam", "IdChampionship", "idChampionship", searchFilter.Championship?.Id);

            base.AddFilterCriteria(ConditionType.Equal, "RequestTeam", "IdChampionship", "idChampionship", searchFilter.Championship?.Id);
        }

        private void CreateHearingRequest(HearingRequest hearingRequest, ITransactionalContext? context = null)
        {
            string req = @$"SELECT
                            COALESCE(MAX(RequestNumber)+1, 1)
                        FROM [HearingRequest]
                        INNER JOIN [Team] [Team] ON [Team].Id = [HearingRequest].IdTeam
                        INNER JOIN [Championship] [Championship] ON [Championship].Id = [Team].IdChampionship
                        WHERE [Championship].Id = {hearingRequest.Team.Championship.Id}";

            string sql = $@" INSERT INTO [HearingRequest]
                            ( IdTeam, IdRequestUser, IdRequestStatus, IdHearingRequestType, CreationDate, RequestNumber, RaceNumber )
                        VALUES
                            ( @idTeam, @idRequestUser, @idRequestStatus, @idHearingRequestType, @creationDate, ({req}), @raceNumber )";

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

        private void UpdateHearingRequestStatus(HearingRequest hearingRequest, ITransactionalContext? context = null)
        {
            string sql = $@"UPDATE [HearingRequest] SET
                                IdRequestStatus = @idRequestStatus";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("idRequestStatus", hearingRequest.Status.Id);

            QueryBuilder.AddParameter("id", hearingRequest.Id);
            QueryBuilder.AddCondition("Id = @id");

            base.ExecuteAndGetRowsAffected(context);
        }

        private void CreateHearingRequestProtestor(HearingRequest hearingRequest, ITransactionalContext? context = null)
        {
            string sql = @" INSERT INTO [HearingRequest_Protestor]
                            ( IdHearingRequest, IdTeamBoat, Address, PhoneNumber )
                        VALUES
                            ( @idHearingRequest, @idTeamBoat, @address, @phoneNumber )";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("idHearingRequest", hearingRequest.Id);
            QueryBuilder.AddParameter("idTeamBoat", hearingRequest.Protestor.TeamBoat.Id);
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
                            ( IdHearingRequest, IdTeamBoat )
                        VALUES
                            ( @idHearingRequest, @idTeamBoat )";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("idHearingRequest", hearingRequest.Id);
            QueryBuilder.AddParameter("idTeamBoat", hearingRequestProtestee.TeamBoat.Id);

            QueryBuilder.AddReturnLastInsertedId();

            hearingRequestProtestee.Id = base.Execute<int>(context);
        }

        private void CreateHearingRequestCommitteeBoatReturn(HearingRequest hearingRequest, CommitteeBoatReturn commiteeBoatReturn, ITransactionalContext? context = null)
        {
            string sql = @" INSERT INTO [HearingRequest_CommitteeBoatReturn]
                            ( IdHearingRequest, IdCommitteeBoatReturn )
                        VALUES
                            ( @idHearingRequest, @idCommitteeBoatReturn )";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("idHearingRequest", hearingRequest.Id);
            QueryBuilder.AddParameter("idCommitteeBoatReturn", commiteeBoatReturn.Id);

            QueryBuilder.AddReturnLastInsertedId();

            hearingRequest.Incident.Id = base.Execute<int>(context);
        }

        private void CreateHearingRequestWithdrawal(HearingRequest hearingRequest, ITransactionalContext? context = null)
        {
            string sql = @" INSERT INTO [HearingRequest_Withdrawal]
                            ( IdHearingRequest, IsRequested, IsAuthorized )
                        VALUES
                            ( @idHearingRequest, @isRequested, @isAuthorized )";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("idHearingRequest", hearingRequest.Id);
            QueryBuilder.AddParameter("isRequested", hearingRequest.Withdrawal.IsRequested);
            QueryBuilder.AddParameter("isAuthorized", hearingRequest.Withdrawal.IsAuthorized);

            QueryBuilder.AddReturnLastInsertedId();

            hearingRequest.Withdrawal.Id = base.Execute<int>(context);
        }

        private void CreateHearingRequestLodgement(HearingRequest hearingRequest, ITransactionalContext? context = null)
        {
            string sql = @" INSERT INTO [HearingRequest_Lodgement]
                            ( IdHearingRequest, Deadline, IsInTerm, HasExtension )
                        VALUES
                            ( @idHearingRequest, @deadline, @isInTerm, @hasExtension )";

            QueryBuilder.AddCommand(sql);

            var lodgement = hearingRequest.Lodgement;

            QueryBuilder.AddParameter("idHearingRequest", hearingRequest.Id);
            QueryBuilder.AddParameter("deadline", lodgement.Deadline);
            QueryBuilder.AddParameter("isInTerm", lodgement.IsInTerm);
            QueryBuilder.AddParameter("hasExtension", lodgement.HasExtension);

            QueryBuilder.AddReturnLastInsertedId();

            hearingRequest.Lodgement.Id = base.Execute<int>(context);
        }

        private void CreateHearingRequestAttendees(HearingRequest hearingRequest, ITransactionalContext? context = null)
        {
            string sql = @" INSERT INTO [HearingRequest_Attendees]
                            ( IdHearingRequest, Protestors, Protestees, Witnesses, Interpreters )
                        VALUES
                            ( @idHearingRequest, @protestors, @protestees, @witnesses, @interpreters )";

            QueryBuilder.AddCommand(sql);

            var attendees = hearingRequest.Attendees;

            QueryBuilder.AddParameter("idHearingRequest", hearingRequest.Id);
            QueryBuilder.AddParameter("protestors", attendees.Protestors);
            QueryBuilder.AddParameter("protestees", attendees.Protestees);
            QueryBuilder.AddParameter("witnesses", attendees.Witnesses);
            QueryBuilder.AddParameter("interpreters", attendees.Interpreters);

            QueryBuilder.AddReturnLastInsertedId();

            hearingRequest.Attendees.Id = base.Execute<int>(context);
        }

        private void CreateHearingRequestValidity(HearingRequest hearingRequest, ITransactionalContext? context = null)
        {
            string sql = @" INSERT INTO [HearingRequest_Validity]
                            (
                                IdHearingRequest, 
                                IsInterestedPartyObjection, InterestedPartyObjectionObservations, 
                                DidProtestIdentifyIncident, ProtestIdentifyObservations,
                                WasObjectionSaidAloud, ObjectionSaidAloudObservations,
                                DidProtestorGiveNotice, ProtestorGiveNoticeObservations,
                                WasRedFlagWasDisplayed, RedFlagWasDisplayedObservations,
                                WasRedFlagSeenByRaceCommission, RedFlagSeenByRaceCommissionObservations,
                                IsValidProtest
                            )
                        VALUES
                            (
                                @idHearingRequest, 
                                @isInterestedPartyObjection, @interestedPartyObjectionObservations, 
                                @didProtestIdentifyIncident, @protestIdentifyObservations,
                                @wasObjectionSaidAloud, @objectionSaidAloudObservations,
                                @didProtestorGiveNotice, @protestorGiveNoticeObservations,
                                @wasRedFlagWasDisplayed, @redFlagWasDisplayedObservations,
                                @wasRedFlagSeenByRaceCommission, @redFlagSeenByRaceCommissionObservations,
                                @isValidProtest
                            )";

            QueryBuilder.AddCommand(sql);

            var validity = hearingRequest.Validity;

            QueryBuilder.AddParameter("idHearingRequest", hearingRequest.Id);
            QueryBuilder.AddParameter("@isInterestedPartyObjection", validity.IsInterestedPartyObjection);
            QueryBuilder.AddParameter("@interestedPartyObjectionObservations", validity.InterestedPartyObjectionObservations);
            QueryBuilder.AddParameter("@didProtestIdentifyIncident", validity.DidProtestIdentifyIncident);
            QueryBuilder.AddParameter("@protestIdentifyObservations", validity.ProtestIdentifyObservations);
            QueryBuilder.AddParameter("@wasObjectionSaidAloud", validity.WasObjectionSaidAloud);
            QueryBuilder.AddParameter("@objectionSaidAloudObservations", validity.ObjectionSaidAloudObservations);
            QueryBuilder.AddParameter("@didProtestorGiveNotice", validity.DidProtestorGiveNotice);
            QueryBuilder.AddParameter("@protestorGiveNoticeObservations", validity.ProtestorGiveNoticeObservations);
            QueryBuilder.AddParameter("@wasRedFlagWasDisplayed", validity.WasRedFlagWasDisplayed);
            QueryBuilder.AddParameter("@redFlagWasDisplayedObservations", validity.RedFlagWasDisplayedObservations);
            QueryBuilder.AddParameter("@wasRedFlagSeenByRaceCommission", validity.WasRedFlagSeenByRaceCommission);
            QueryBuilder.AddParameter("@redFlagSeenByRaceCommissionObservations", validity.RedFlagSeenByRaceCommissionObservations);
            QueryBuilder.AddParameter("@isValidProtest", validity.IsValidProtest);

            QueryBuilder.AddReturnLastInsertedId();

            hearingRequest.Validity.Id = base.Execute<int>(context);
        }

        private void CreateHearingRequestResolution(HearingRequest hearingRequest, ITransactionalContext? context = null)
        {
            string sql = @" INSERT INTO [HearingRequest_Resolution]
                            (
                                IdHearingRequest,
                                AcceptedFacts,
                                CommissionAcceptsShipSchematic, CommissionAttachesOwnSchematic, Comments, Dismissed,
                                ProtestedBoatsAreDisqualified, PenaltiesAreAssessed, PenaltiesDescription,
                                CommissionChairmanAndOthers, ResolutionDate
                            )
                        VALUES
                            ( 
                                @idHearingRequest,
                                @acceptedFacts,
                                @commissionAcceptsShipSchematic, @commissionAttachesOwnSchematic, @comments, @dismissed, 
                                @protestedBoatsAreDisqualified, @penaltiesAreAssessed, @penaltiesDescription,
                                @commissionChairmanAndOthers, @resolutionDate
                            )";

            QueryBuilder.AddCommand(sql);

            var resolution = hearingRequest.Resolution;

            QueryBuilder.AddParameter("idHearingRequest", hearingRequest.Id);
            QueryBuilder.AddParameter("acceptedFacts", resolution.AcceptedFacts);
            QueryBuilder.AddParameter("commissionAcceptsShipSchematic", resolution.CommissionAcceptsShipSchematic);
            QueryBuilder.AddParameter("commissionAttachesOwnSchematic", resolution.CommissionAttachesOwnSchematic);
            QueryBuilder.AddParameter("comments", resolution.Comments);
            QueryBuilder.AddParameter("dismissed", resolution.Dismissed);
            QueryBuilder.AddParameter("protestedBoatsAreDisqualified", resolution.ProtestedBoatsAreDisqualified);
            QueryBuilder.AddParameter("penaltiesAreAssessed", resolution.PenaltiesAreAssessed);
            QueryBuilder.AddParameter("penaltiesDescription", resolution.PenaltiesDescription);
            QueryBuilder.AddParameter("commissionChairmanAndOthers", resolution.CommissionChairmanAndOthers);
            QueryBuilder.AddParameter("resolutionDate", resolution.ResolutionDate);

            QueryBuilder.AddReturnLastInsertedId();

            hearingRequest.Resolution.Id = base.Execute<int>(context);
        }

        #endregion
    }
}
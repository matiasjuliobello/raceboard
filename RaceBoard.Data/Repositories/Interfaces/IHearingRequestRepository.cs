using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Domain;

namespace RaceBoard.Data.Repositories.Interfaces
{
    public interface IHearingRequestRepository
    {
        ITransactionalContext GetTransactionalContext(TransactionContextScope scope = TransactionContextScope.Internal);

        void ConfirmTransactionalContext(ITransactionalContext context);

        void CancelTransactionalContext(ITransactionalContext context);

        bool Exists(int id, ITransactionalContext? context = null);
        bool ExistsDuplicate(HearingRequest hearingRequest, ITransactionalContext? context = null);

        PaginatedResult<HearingRequest> Get(HearingRequestSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null);
        HearingRequest? Get(int id, ITransactionalContext? context = null);
        HearingRequestProtestor GetProtestor(int id, ITransactionalContext? context = null);
        HearingRequestProtestees GetProtestees(int id, ITransactionalContext? context = null);
        HearingRequestIncident GetIncident(int id, ITransactionalContext? context = null);
        CommitteeBoatReturn GetAssociatedCommitteeBoatReturn(int id, ITransactionalContext? context = null);

        HearingRequestWithdrawal GetWithdrawal(int id, ITransactionalContext? context = null);
        HearingRequestLodgement GetLodgement(int id, ITransactionalContext? context = null);
        HearingRequestAttendees GetAttendees(int id, ITransactionalContext? context = null);
        HearingRequestValidity GetValidity(int id, ITransactionalContext? context = null);
        HearingRequestResolution GetResolution(int id, ITransactionalContext? context = null);

        PaginatedResult<HearingRequest> FindHearingRequestsIncludingTeamBoat(int idTeamBoat, ITransactionalContext? context = null);

        void Create(HearingRequest hearingRequest, ITransactionalContext? context = null);
        void UpdateStatus(HearingRequest hearingRequest, ITransactionalContext? context = null);
        void CreateProtestor(HearingRequest hearingRequest, ITransactionalContext? context = null);
        void CreateProtestorNotice(HearingRequest hearingRequest, ITransactionalContext? context = null);
        void CreateProtestees(HearingRequest hearingRequest, ITransactionalContext? context = null);
        void CreateIncident(HearingRequest hearingRequest, ITransactionalContext? context = null);
        void CreateCommitteeBoatReturnAssociation(HearingRequest hearingRequest, CommitteeBoatReturn commiteeBoatReturn, ITransactionalContext? context = null);
        void CreateRequestWithdrawal(HearingRequest hearingRequest, ITransactionalContext? context = null);
        void CreateRequestLodgement(HearingRequest hearingRequest, ITransactionalContext? context = null);
        void CreateRequestAttendees(HearingRequest hearingRequest, ITransactionalContext? context = null);
        void CreateRequestValidity(HearingRequest hearingRequest, ITransactionalContext? context = null);
        void CreateRequestResolution(HearingRequest hearingRequest, ITransactionalContext? context = null);
    }
}

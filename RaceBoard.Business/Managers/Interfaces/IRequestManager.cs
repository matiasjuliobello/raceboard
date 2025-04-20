using Microsoft.AspNetCore.Mvc;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data;
using RaceBoard.Domain;

namespace RaceBoard.Business.Managers.Interfaces
{
    public interface IRequestManager
    {
        PaginatedResult<EquipmentChangeRequest> GetEquipmentChangeRequests(ChangeRequestSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null);
        EquipmentChangeRequest GetEquipmentChangeRequest(int id, ITransactionalContext? context = null);
        void CreateEquipmentChangeRequest(EquipmentChangeRequest equipmentChangeRequest, ITransactionalContext? context = null);
        void UpdateEquipmentChangeRequest(CrewChangeRequest crewChangeRequest, ITransactionalContext? context = null);

        PaginatedResult<CrewChangeRequest> GetCrewChangeRequests(ChangeRequestSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null);
        CrewChangeRequest GetCrewChangeRequest(int id, ITransactionalContext? context = null);
        void CreateCrewChangeRequest(CrewChangeRequest crewChangeRequest, ITransactionalContext? context = null);
        void UpdateCrewChangeRequest(CrewChangeRequest crewChangeRequest, ITransactionalContext? context = null);

        PaginatedResult<HearingRequestType> GetHearingRequestTypes(PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null);
        PaginatedResult<HearingRequest> GetHearingRequests(HearingRequestSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null);
        HearingRequest GetHearingRequest(int id, ITransactionalContext? context = null);
        void CreateHearingRequest(HearingRequest hearingRequest, ITransactionalContext? context = null);
        void UpdateHearingRequest(HearingRequest hearingRequest, ITransactionalContext? context = null);

        byte[] RenderHearingRequest(HearingRequest hearingRequest, ITransactionalContext? context = null);
    }
}
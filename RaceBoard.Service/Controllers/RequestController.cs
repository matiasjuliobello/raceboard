using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Common;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Domain;
using RaceBoard.DTOs._Pagination.Request;
using RaceBoard.DTOs._Pagination.Response;
using RaceBoard.DTOs.ChangeRequest.Request;
using RaceBoard.DTOs.ChangeRequest.Response;
using RaceBoard.DTOs.CommitteeBoatReturn.Response;
using RaceBoard.DTOs.HearingRequest.Request;
using RaceBoard.DTOs.HearingRequest.Response;
using RaceBoard.Service.Controllers.Abstract;
using RaceBoard.Service.Helpers.Interfaces;
using RaceBoard.Translations.Interfaces;

namespace RaceBoard.Service.Controllers
{
    [Route("api/requests")]
    [ApiController]
    public class RequestController : AbstractController<RequestController>
    {
        private readonly IRequestManager _requestManager;
        private readonly ITeamManager _teamManager;

        public RequestController
            (
                IMapper mapper,
                ILogger<RequestController> logger,
                ITranslator translator,
                IRequestManager requestManager,
                ITeamManager teamManager,
                ISessionHelper sessionHelper,
                IRequestContextManager requestContextManager
            ) : base(mapper, logger, translator, sessionHelper, requestContextManager)
        {
            _requestManager = requestManager;
            _teamManager = teamManager;
        }

        #region Crew Changes

        [HttpGet("crew-changes")]
        public ActionResult<PaginatedResultResponse<CrewChangeRequestResponse>> GetCrewChangeRequests([FromQuery] ChangeRequestSearchFilterRequest? searchFilterRequest = null, [FromQuery] PaginationFilterRequest? paginationFilterRequest = null, [FromQuery] SortingRequest? sortingRequest = null)
        {
            var searchFilter = _mapper.Map<ChangeRequestSearchFilter>(searchFilterRequest);
            var paginationFilter = _mapper.Map<PaginationFilter>(paginationFilterRequest);
            var sorting = _mapper.Map<Sorting>(sortingRequest);

            var changeRequests = _requestManager.GetCrewChangeRequests(searchFilter, paginationFilter, sorting);

            var response = _mapper.Map<PaginatedResultResponse<CrewChangeRequestResponse>>(changeRequests);

            return Ok(response);
        }

        [HttpGet("crew-changes/{id}")]
        public ActionResult<CrewChangeRequestResponse> GetCrewChangeRequest(int id)
        {
            var changeRequest = _requestManager.GetCrewChangeRequest(id);

            var response = _mapper.Map<CrewChangeRequestResponse>(changeRequest);

            return Ok(response);
        }

        [HttpPost("crew-changes")]
        public ActionResult<int> SubmitCrewChangeRequest(IFormFile file, [FromForm] CrewChangeRequestRequest crewChangeRequestRequest)
        {
            var changeRequest = _mapper.Map<CrewChangeRequest>(crewChangeRequestRequest);

            var uploadedFile = _mapper.Map<IFormFile, FileUpload>(file);
            if (uploadedFile != null)
                changeRequest.File = base.CreateFileInstance(uploadedFile);

            _requestManager.CreateCrewChangeRequest(changeRequest);

            return Ok(changeRequest.Id);
        }

        #endregion

        #region Equipment Changes

        [HttpGet("equipment-changes")]
        public ActionResult<PaginatedResultResponse<EquipmentChangeRequestResponse>> GetEquipmentChangeRequests([FromQuery] ChangeRequestSearchFilterRequest? searchFilterRequest = null, [FromQuery] PaginationFilterRequest? paginationFilterRequest = null, [FromQuery] SortingRequest? sortingRequest = null)
        {
            var searchFilter = _mapper.Map<ChangeRequestSearchFilter>(searchFilterRequest);
            var paginationFilter = _mapper.Map<PaginationFilter>(paginationFilterRequest);
            var sorting = _mapper.Map<Sorting>(sortingRequest);

            var changeRequests = _requestManager.GetEquipmentChangeRequests(searchFilter, paginationFilter, sorting);

            var response = _mapper.Map<PaginatedResultResponse<EquipmentChangeRequestResponse>>(changeRequests);

            return Ok(response);
        }

        [HttpGet("equipment-changes/{id}")]
        public ActionResult<EquipmentChangeRequestResponse> GetEquipmentChangeRequest(int id)
        {
            var changeRequest = _requestManager.GetEquipmentChangeRequest(id);

            var response = _mapper.Map<EquipmentChangeRequestResponse>(changeRequest);

            return Ok(response);
        }

        [HttpPost("equipment-changes")]
        public ActionResult<int> SubmitEquipmentChangeRequest(IFormFile file, [FromForm] EquipmentChangeRequestRequest equipmentChangeRequestRequest)
        {
            var changeRequest = _mapper.Map<EquipmentChangeRequest>(equipmentChangeRequestRequest);

            var uploadedFile = _mapper.Map<IFormFile, FileUpload>(file);
            if (uploadedFile != null)
                changeRequest.File = base.CreateFileInstance(uploadedFile);

            _requestManager.CreateEquipmentChangeRequest(changeRequest);

            return Ok(changeRequest.Id);
        }

        #endregion

        #region Hearings

        [HttpGet("hearing-types")]
        public ActionResult<PaginatedResultResponse<HearingRequestTypeResponse>> GetHearingRequestTypes([FromQuery] PaginationFilterRequest? paginationFilterRequest = null, [FromQuery] SortingRequest? sortingRequest = null)
        {
            var paginationFilter = _mapper.Map<PaginationFilter>(paginationFilterRequest);
            var sorting = _mapper.Map<Sorting>(sortingRequest);

            var data = _requestManager.GetHearingRequestTypes(paginationFilter, sorting);

            var response = _mapper.Map<PaginatedResultResponse<HearingRequestTypeResponse>>(data);

            return Ok(response);
        }

        [HttpGet("hearings")]
        public ActionResult<PaginatedResultResponse<HearingRequestResponse>> GetHearingRequests([FromQuery] HearingRequestSearchFilterRequest? searchFilterRequest = null, [FromQuery] PaginationFilterRequest? paginationFilterRequest = null, [FromQuery] SortingRequest? sortingRequest = null)
        {
            var searchFilter = _mapper.Map<HearingRequestSearchFilter>(searchFilterRequest);

            //if (searchFilter.Championship == null || searchFilter.Championship.Id == 0)
            //    return ReturnBadRequestResponse("ChampionshipIsRequired");

            var paginationFilter = _mapper.Map<PaginationFilter>(paginationFilterRequest);
            var sorting = _mapper.Map<Sorting>(sortingRequest);

            var changeRequests = _requestManager.GetHearingRequests(searchFilter, paginationFilter, sorting);

            var response = _mapper.Map<PaginatedResultResponse<HearingRequestResponse>>(changeRequests);

            return Ok(response);
        }

        [HttpGet("hearings/{id}")]
        public ActionResult<HearingRequestResponse> GetHearingRequest(int id)
        {
            var changeRequest = _requestManager.GetHearingRequest(id);

            var response = _mapper.Map<HearingRequestResponse>(changeRequest);

            return Ok(response);
        }

        [HttpPost("hearings")]
        public ActionResult<int> CreateHearingRequest([FromBody] HearingRequestRequest hearingRequestRequest)
        {
            var hearingRequest = _mapper.Map<HearingRequest>(hearingRequestRequest);

            hearingRequest.Team = _teamManager.Get(hearingRequest.Team.Id);

            _requestManager.CreateHearingRequest(hearingRequest);

            return Ok(hearingRequest.Id);
        }

        [HttpPut("hearings")]
        public ActionResult<int> EditHearingRequest([FromBody] HearingRequestRequest hearingRequestRequest)
        {
            var hearingRequest = _mapper.Map<HearingRequest>(hearingRequestRequest);

            _requestManager.UpdateHearingRequest(hearingRequest);

            return Ok(hearingRequest.Id);
        }

        [HttpGet("hearings/{id}/printable-forms")]
        public ActionResult GetHearingRequestPrintableForm(int id)
        {
            HearingRequest hearingRequest = null;

            string hearingNumber = "";

            if (id > 0)
            {
                hearingRequest = _requestManager.GetHearingRequest(id);
                hearingRequest.Team = _teamManager.Get(hearingRequest.Team.Id);
                hearingNumber = $" #{hearingRequest.RequestNumber}";
            }

            var fileContent = _requestManager.RenderHearingRequest(hearingRequest!);

            var stream = new MemoryStream(fileContent);

            return new FileStreamResult(stream, CommonValues.MimeTypes.ApplicationOctetStream)
            {
                FileDownloadName = $"{Translate("HearingRequest").ToUpper()}{hearingNumber}.pdf"
            };
        }

        [HttpGet("hearings/{id}/committee-boat-return")]
        public ActionResult GetHearingRequestAssociatedCommitteeBoatReturn(int id)
        {
            CommitteeBoatReturn committeeBoatReturn = _requestManager.GetHearingRequestAssociatedCommitteeBoatReturn(id);

            var response = _mapper.Map<CommitteeBoatReturnResponse>(committeeBoatReturn);

            return Ok(response);
        }

        #endregion

        #region Private Methods

        #endregion
    }
}

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Domain;
using RaceBoard.DTOs._Pagination.Request;
using RaceBoard.DTOs._Pagination.Response;
using RaceBoard.DTOs.ChangeRequest.Request;
using RaceBoard.DTOs.ChangeRequest.Response;
using RaceBoard.Service.Controllers.Abstract;
using RaceBoard.Service.Helpers.Interfaces;
using RaceBoard.Translations.Interfaces;

namespace RaceBoard.Service.Controllers
{
    [Route("api/requests")]
    [ApiController]
    public class ChangeRequestController : AbstractController<ChangeRequestController>
    {
        private readonly IChangeRequestManager _changeRequestManager;

        public ChangeRequestController
            (
                IMapper mapper,
                ILogger<ChangeRequestController> logger,
                ITranslator translator,
                IChangeRequestManager changeRequestManager,
                ISessionHelper sessionHelper,
                IRequestContextManager requestContextManager
            ) : base(mapper, logger, translator, sessionHelper, requestContextManager)
        {
            _changeRequestManager = changeRequestManager;
        }

        [HttpGet("crew-change")]
        public ActionResult<PaginatedResultResponse<CrewChangeRequestResponse>> GetCrewChangeRequests([FromQuery] ChangeRequestSearchFilterRequest? searchFilterRequest = null, [FromQuery] PaginationFilterRequest? paginationFilterRequest = null, [FromQuery] SortingRequest? sortingRequest = null)
        {
            var searchFilter = _mapper.Map<ChangeRequestSearchFilter>(searchFilterRequest);
            var paginationFilter = _mapper.Map<PaginationFilter>(paginationFilterRequest);
            var sorting = _mapper.Map<Sorting>(sortingRequest);

            var changeRequests = _changeRequestManager.GetCrewChangeRequests(searchFilter, paginationFilter, sorting);

            var response = _mapper.Map<PaginatedResultResponse<CrewChangeRequestResponse>>(changeRequests);

            return Ok(response);
        }

        [HttpGet("equipment-change")]
        public ActionResult<PaginatedResultResponse<EquipmentChangeRequestResponse>> GetEquipmentChangeRequests([FromQuery] ChangeRequestSearchFilterRequest? searchFilterRequest = null, [FromQuery] PaginationFilterRequest? paginationFilterRequest = null, [FromQuery] SortingRequest? sortingRequest = null)
        {
            var searchFilter = _mapper.Map<ChangeRequestSearchFilter>(searchFilterRequest);
            var paginationFilter = _mapper.Map<PaginationFilter>(paginationFilterRequest);
            var sorting = _mapper.Map<Sorting>(sortingRequest);

            var changeRequests = _changeRequestManager.GetEquipmentChangeRequests(searchFilter, paginationFilter, sorting);

            var response = _mapper.Map<PaginatedResultResponse<EquipmentChangeRequestResponse>>(changeRequests);

            return Ok(response);
        }

        [HttpGet("equipment-change/{id}")]
        public ActionResult<EquipmentChangeRequestResponse> GetEquipmentChangeRequest(int id)
        {
            var changeRequest = _changeRequestManager.GetEquipmentChangeRequest(id);

            var response = _mapper.Map<EquipmentChangeRequestResponse>(changeRequest);

            return Ok(response);
        }

        [HttpPost("equipment-change")]
        public ActionResult<int> SubmitEquipmentChangeRequest(IFormFile file, [FromForm] EquipmentChangeRequestRequest equipmentChangeRequestRequest)
        {
            var changeRequest = _mapper.Map<EquipmentChangeRequest>(equipmentChangeRequestRequest);

            var uploadedFile = _mapper.Map<IFormFile, FileUpload>(file);
            if (uploadedFile != null)
                changeRequest.File = base.CreateFileInstance(uploadedFile);

            _changeRequestManager.CreateEquipmentChangeRequest(changeRequest);

            return Ok(changeRequest.Id);
        }

        [HttpPost("crew-change")]
        public ActionResult<int> SubmitCrewChangeRequest(IFormFile file, [FromForm] CrewChangeRequestRequest crewChangeRequestRequest)
        {
            var changeRequest = _mapper.Map<CrewChangeRequest>(crewChangeRequestRequest);

            var uploadedFile = _mapper.Map<IFormFile, FileUpload>(file);
            if (uploadedFile != null)
            {
                changeRequest.File = base.CreateFileInstance(uploadedFile);
                changeRequest.File.Description = "";
            }

            _changeRequestManager.CreateCrewChangeRequest(changeRequest);

            return Ok(changeRequest.Id);
        }

        //[HttpPut()]
        //public ActionResult Update(ChangeRequestRequest changeRequestRequest)
        //{
        //    var changeRequest = _mapper.Map<ChangeRequest>(changeRequestRequest);

        //    _changeRequestManager.Update(changeRequest);

        //    return Ok();
        //}

        //[HttpDelete("{id}")]
        //public ActionResult Delete([FromRoute] int id)
        //{
        //    _changeRequestManager.Delete(id);

        //    return Ok();
        //}

        #region Private Methods

        #endregion
    }
}

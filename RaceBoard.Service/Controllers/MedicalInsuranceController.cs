using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Domain;
using RaceBoard.DTOs._Pagination.Request;
using RaceBoard.DTOs._Pagination.Response;
using RaceBoard.DTOs.MedicalInsurance.Request;
using RaceBoard.DTOs.MedicalInsurance.Response;
using RaceBoard.Service.Controllers.Abstract;
using RaceBoard.Service.Helpers.Interfaces;
using RaceBoard.Translations.Interfaces;

namespace RaceBoard.Service.Controllers
{
    [Route("api/medical-insurances")]
    [ApiController]
    public class MedicalInsuranceController : AbstractController<MedicalInsuranceController>
    {
        private readonly IMedicalInsuranceManager _medicalInsuranceManager;

        public MedicalInsuranceController
            (
                IMapper mapper,
                ILogger<MedicalInsuranceController> logger,
                ITranslator translator,
                IMedicalInsuranceManager medicalInsuranceManager,
                ISessionHelper sessionHelper,
                IRequestContextManager requestContextManager
            ) : base(mapper, logger, translator, sessionHelper, requestContextManager)
        {
            _medicalInsuranceManager = medicalInsuranceManager;
        }

        [HttpGet()]
        public ActionResult<List<MedicalInsuranceResponse>> Get([FromQuery] MedicalInsuranceSearchFilterRequest? searchFilterRequest = null, [FromQuery] PaginationFilterRequest? paginationFilterRequest = null, [FromQuery] SortingRequest? sortingRequest = null)
        {
            var searchFilter = _mapper.Map<MedicalInsuranceSearchFilterRequest, MedicalInsuranceSearchFilter>(searchFilterRequest);
            var paginationFilter = _mapper.Map<PaginationFilter>(paginationFilterRequest);
            var sorting = _mapper.Map<Sorting>(sortingRequest);

            var data = _medicalInsuranceManager.Get(searchFilter, paginationFilter, sorting);

            var response = _mapper.Map<PaginatedResultResponse<MedicalInsuranceResponse>>(data);

            return Ok(response);
        }

        #region Private Methods

        #endregion
    }
}

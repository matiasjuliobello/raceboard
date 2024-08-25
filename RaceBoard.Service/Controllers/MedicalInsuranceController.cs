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
    public class MedicalInsuranceControllerController : AbstractController<MedicalInsuranceControllerController>
    {
        private readonly IMedicalInsuranceManager _medicalInsuranceManager;

        public MedicalInsuranceControllerController
            (
                IMapper mapper,
                ILogger<MedicalInsuranceControllerController> logger,
                ITranslator translator,
                IMedicalInsuranceManager medicalInsuranceManager,
                ISessionHelper sessionHelper,
                IRequestContextHelper requestContextHelper
            ) : base(mapper, logger, translator, sessionHelper, requestContextHelper)
        {
            _medicalInsuranceManager = medicalInsuranceManager;
        }

        [HttpGet()]
        public ActionResult<List<MedicalInsuranceResponse>> GetMedicalInsurances([FromQuery] MedicalInsuranceSearchFilterRequest searchFilterRequest, [FromQuery] PaginationFilterRequest paginationFilterRequest, [FromQuery] SortingRequest sortingRequest)
        {
            var searchFilter = _mapper.Map<MedicalInsuranceSearchFilterRequest, MedicalInsuranceSearchFilter>(searchFilterRequest);
            var paginationFilter = _mapper.Map<PaginationFilter>(paginationFilterRequest);
            var sorting = _mapper.Map<Sorting>(sortingRequest);

            var medicalInsurances = _medicalInsuranceManager.Get(searchFilter, paginationFilter, sorting);

            var response = _mapper.Map<PaginatedResultResponse<MedicalInsuranceResponse>>(medicalInsurances);

            return Ok(response);
        }

        #region Private Methods

        #endregion
    }
}

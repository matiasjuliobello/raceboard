using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RaceBoard.Business.Managers;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Domain;
using RaceBoard.DTOs._Pagination.Request;
using RaceBoard.DTOs._Pagination.Response;
using RaceBoard.DTOs.Person.Request;
using RaceBoard.DTOs.Person.Response;
using RaceBoard.DTOs.Race.Response;
using RaceBoard.Service.Controllers.Abstract;
using RaceBoard.Service.Helpers.Interfaces;
using RaceBoard.Translations.Interfaces;

namespace RaceBoard.Service.Controllers
{
    [Route("api/persons")]
    [ApiController]
    public class PersonController : AbstractController<PersonController>
    {
        private readonly IPersonManager _personManager;

        public PersonController
            (
                IMapper mapper,
                ILogger<PersonController> logger,
                ITranslator translator,
                IPersonManager personManager,
                ISessionHelper sessionHelper,
                IRequestContextHelper requestContextHelper
            ) : base(mapper, logger, translator, sessionHelper, requestContextHelper)
        {
            _personManager = personManager;
        }

        [HttpGet("search")]
        public ActionResult<PaginatedResultResponse<PersonResponse>> Search([FromQuery] string searchTerm, [FromQuery] PaginationFilterRequest? paginationFilterRequest = null, [FromQuery] SortingRequest? sortingRequest = null)
        {
            var paginationFilter = _mapper.Map<PaginationFilter>(paginationFilterRequest);
            var sorting = _mapper.Map<Sorting>(sortingRequest);

            var data = _personManager.Search(searchTerm, paginationFilter, sorting);

            var response = _mapper.Map<PaginatedResultResponse<PersonResponse>>(data);

            return Ok(response);
        }

        [HttpGet()]
        public ActionResult<PaginatedResultResponse<PersonResponse>> Get([FromQuery] PersonSearchFilterRequest? searchFilterRequest = null, [FromQuery] PaginationFilterRequest? paginationFilterRequest = null, [FromQuery] SortingRequest? sortingRequest = null)
        {
            var searchFilter = _mapper.Map<PersonSearchFilter>(searchFilterRequest);
            var paginationFilter = _mapper.Map<PaginationFilter>(paginationFilterRequest);
            var sorting = _mapper.Map<Sorting>(sortingRequest);

            var data = _personManager.Get(searchFilter, paginationFilter, sorting);

            var response = _mapper.Map<PaginatedResultResponse<PersonResponse>>(data);

            return Ok(response);
        }

        [HttpGet("{id}")]
        public ActionResult<PersonResponse> Get([FromRoute] int id)
        {
            var data = _personManager.Get(id);

            var response = _mapper.Map<PersonResponse>(data);

            return Ok(response);
        }

        [HttpPost()]
        public ActionResult<int> Create(PersonRequest personRequest)
        {
            var person = _mapper.Map<Person>(personRequest);

            _personManager.Create(person);

            return Ok(person.Id);
        }

        [HttpPut()]
        public ActionResult Update(PersonRequest personRequest)
        {
            var person = _mapper.Map<Person>(personRequest);

            _personManager.Update(person);

            return Ok();
        }

        [HttpDelete("id")]
        public ActionResult Delete(int id)
        {
            _personManager.Delete(id);

            return Ok();
        }

        #region Private Methods

        #endregion
    }
}

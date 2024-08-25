using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Domain;
using RaceBoard.DTOs._Pagination.Request;
using RaceBoard.DTOs._Pagination.Response;
using RaceBoard.DTOs.Person.Request;
using RaceBoard.DTOs.Person.Response;
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

        [HttpGet()]
        public ActionResult<PaginatedResultResponse<PersonResponse>> GetPersons([FromQuery] PersonSearchFilterRequest searchFilterRequest, [FromQuery] PaginationFilterRequest paginationFilterRequest, [FromQuery] SortingRequest sortingRequest)
        {
            var searchFilter = _mapper.Map<PersonSearchFilter>(searchFilterRequest);
            var paginationFilter = _mapper.Map<PaginationFilter>(paginationFilterRequest);
            var sorting = _mapper.Map<Sorting>(sortingRequest);

            var persons = _personManager.Get(searchFilter, paginationFilter, sorting);

            var response = _mapper.Map<PaginatedResultResponse<PersonResponse>>(persons);

            return Ok(response);
        }

        [HttpPost()]
        public ActionResult<int> CreatePerson(PersonRequest personRequest)
        {
            var person = _mapper.Map<Person>(personRequest);

            _personManager.Create(person);

            return Ok(person.Id);
        }

        [HttpPut()]
        public ActionResult UpdatePerson(PersonRequest personRequest)
        {
            var person = _mapper.Map<Person>(personRequest);

            _personManager.Update(person);

            return Ok();
        }

        [HttpDelete("id")]
        public ActionResult DeletePerson(int id)
        {
            _personManager.Delete(id);

            return Ok();
        }

        #region Private Methods

        #endregion
    }
}

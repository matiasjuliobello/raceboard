using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Domain;
using RaceBoard.DTOs.Person.Request;
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

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Domain;
using RaceBoard.DTOs.RaceClass.Request;
using RaceBoard.Service.Controllers.Abstract;
using RaceBoard.Service.Helpers.Interfaces;
using RaceBoard.Translations.Interfaces;

namespace RaceBoard.Service.Controllers
{
    [Route("api/race-classes")]
    [ApiController]
    public class RaceClassController : AbstractController<RaceClassController>
    {
        private readonly IRaceClassManager _organizationManager;

        public RaceClassController
            (
                IMapper mapper,
                ILogger<RaceClassController> logger,
                ITranslator translator,
                IRaceClassManager organizationManager,
                ISessionHelper sessionHelper,
                IRequestContextHelper requestContextHelper
            ) : base(mapper, logger, translator, sessionHelper, requestContextHelper)
        {
            _organizationManager = organizationManager;
        }

        [HttpPost()]
        public ActionResult<int> CreateRaceClass(RaceClassRequest organizationRequest)
        {
            var organization = _mapper.Map<RaceClass>(organizationRequest);

            _organizationManager.Create(organization);

            return Ok(organization.Id);
        }

        [HttpPut()]
        public ActionResult UpdateRaceClass(RaceClassRequest organizationRequest)
        {
            var organization = _mapper.Map<RaceClass>(organizationRequest);

            _organizationManager.Update(organization);

            return Ok();
        }

        [HttpDelete("id")]
        public ActionResult DeleteRaceClass(int id)
        {
            _organizationManager.Delete(id);

            return Ok();
        }

        #region Private Methods

        #endregion
    }
}

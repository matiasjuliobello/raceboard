using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Domain;
using RaceBoard.DTOs.ContestantRole.Request;
using RaceBoard.Service.Controllers.Abstract;
using RaceBoard.Service.Helpers.Interfaces;
using RaceBoard.Translations.Interfaces;

namespace RaceBoard.Service.Controllers
{
    [Route("api/contestant-roles")]
    [ApiController]
    public class ContestantRoleController : AbstractController<ContestantRoleController>
    {
        private readonly IContestantRoleManager _contestantRoleManager;

        public ContestantRoleController
            (
                IMapper mapper,
                ILogger<ContestantRoleController> logger,
                ITranslator translator,
                IContestantRoleManager contestantRoleManager,
                ISessionHelper sessionHelper,
                IRequestContextHelper requestContextHelper
            ) : base(mapper, logger, translator, sessionHelper, requestContextHelper)
        {
            _contestantRoleManager = contestantRoleManager;
        }

        [HttpPost()]
        public ActionResult<int> CreateContestantRole(ContestantRoleRequest contestantRoleRequest)
        {
            var contestantRole = _mapper.Map<ContestantRole>(contestantRoleRequest);

            _contestantRoleManager.Create(contestantRole);

            return Ok(contestantRole.Id);
        }

        [HttpPut()]
        public ActionResult UpdateContestantRole(ContestantRoleRequest contestantRoleRequest)
        {
            var contestantRole = _mapper.Map<ContestantRole>(contestantRoleRequest);

            _contestantRoleManager.Update(contestantRole);

            return Ok();
        }

        [HttpDelete("id")]
        public ActionResult DeleteContestantRole(int id)
        {
            _contestantRoleManager.Delete(id);

            return Ok();
        }

        #region Private Methods

        #endregion
    }
}

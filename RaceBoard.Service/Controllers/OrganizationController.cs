using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Domain;
using RaceBoard.DTOs.Organization.Request;
using RaceBoard.Service.Controllers.Abstract;
using RaceBoard.Service.Helpers.Interfaces;
using RaceBoard.Translations.Interfaces;

namespace RaceBoard.Service.Controllers
{
    [Route("api/organizations")]
    [ApiController]
    public class OrganizationController : AbstractController<OrganizationController>
    {
        private readonly IOrganizationManager _organizationManager;

        public OrganizationController
            (
                IMapper mapper,
                ILogger<OrganizationController> logger,
                ITranslator translator,
                IOrganizationManager organizationManager,
                ISessionHelper sessionHelper,
                IRequestContextHelper requestContextHelper
            ) : base(mapper, logger, translator, sessionHelper, requestContextHelper)
        {
            _organizationManager = organizationManager;
        }

        [HttpPost()]
        public ActionResult<int> CreateOrganization(OrganizationRequest organizationRequest)
        {
            var organization = _mapper.Map<Organization>(organizationRequest);

            _organizationManager.Create(organization);

            return Ok(organization.Id);
        }

        [HttpPut()]
        public ActionResult UpdateOrganization(OrganizationRequest organizationRequest)
        {
            var organization = _mapper.Map<Organization>(organizationRequest);

            _organizationManager.Update(organization);

            return Ok();
        }

        [HttpDelete("id")]
        public ActionResult DeleteOrganization(int id)
        {
            _organizationManager.Delete(id);

            return Ok();
        }

        #region Private Methods

        #endregion
    }
}

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Domain;
using RaceBoard.DTOs.Competition.Request;
using RaceBoard.DTOs.Competition.Response;
using RaceBoard.Service.Controllers.Abstract;
using RaceBoard.Service.Helpers.Interfaces;
using RaceBoard.Translations.Interfaces;

namespace RaceBoard.Service.Controllers
{
    [Route("api/competitions")]
    [ApiController]
    public class CompetitionTermController : AbstractController<CompetitionTermController>
    {
        private readonly ICompetitionManager _competitionManager;

        public CompetitionTermController
            (
                IMapper mapper,
                ILogger<CompetitionTermController> logger,
                ITranslator translator,
                ICompetitionManager competitionManager,
                ISessionHelper sessionHelper,
                IRequestContextHelper requestContextHelper
            ) : base(mapper, logger, translator, sessionHelper, requestContextHelper)
        {
            _competitionManager = competitionManager;
        }

        [HttpGet("{id}/registration-terms")]
        public ActionResult<List<CompetitionTermResponse>> GetRegistrationTerms([FromRoute] int id)
        {
            var data = _competitionManager.GetRegistrationTerms(id);

            var response = _mapper.Map<List<CompetitionTermResponse>>(data);

            return Ok(response);
        }

        [HttpPost("registration-terms")]
        public ActionResult SetRegistrationTerms(CompetitionTermsRequest termsRequest)
        {
            var terms = new List<CompetitionRegistrationTerm>();

            foreach (var term in termsRequest.Terms)
            {
                foreach (var idRaceClass in term.IdsRaceClass)
                {
                    terms.Add(new CompetitionRegistrationTerm()
                    {
                        Competition = new Competition() { Id = termsRequest.IdCompetition },
                        RaceClass = new RaceClass() { Id = idRaceClass },
                        StartDate = term.StartDate,
                        EndDate = term.EndDate
                    });
                }
            }

            _competitionManager.SetRegistrationTerms(terms);

            return Ok();
        }

        [HttpGet("{id}/accreditation-terms")]
        public ActionResult<List<CompetitionTermResponse>> GetAccreditationTerms([FromRoute] int id)
        {
            var data = _competitionManager.GetAccreditationTerms(id);

            var response = _mapper.Map<List<CompetitionTermResponse>>(data);

            return Ok(response);
        }

        [HttpPost("accreditation-terms")]
        public ActionResult SetAccreditationTerms(CompetitionTermsRequest termsRequest)
        {
            var terms = new List<CompetitionAccreditationTerm>();

            foreach (var term in termsRequest.Terms)
            {
                foreach (var idRaceClass in term.IdsRaceClass)
                {
                    terms.Add(new CompetitionAccreditationTerm()
                    {
                        Competition = new Competition() { Id = termsRequest.IdCompetition },
                        RaceClass = new RaceClass() { Id = idRaceClass },
                        StartDate = term.StartDate,
                        EndDate = term.EndDate
                    });
                }
            }

            _competitionManager.SetAccreditationTerms(terms);

            return Ok();
        }

        #region Private Methods

        #endregion
    }
}


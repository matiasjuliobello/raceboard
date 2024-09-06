﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RaceBoard.Business.Managers;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Domain;
using RaceBoard.DTOs._Pagination.Request;
using RaceBoard.DTOs._Pagination.Response;
using RaceBoard.DTOs.Contestant.Request;
using RaceBoard.DTOs.Contestant.Response;
using RaceBoard.DTOs.Contestant.Request;
using RaceBoard.Service.Controllers.Abstract;
using RaceBoard.Service.Helpers.Interfaces;
using RaceBoard.Translations.Interfaces;

namespace RaceBoard.Service.Controllers
{
    [Route("api/contestants")]
    [ApiController]
    public class ContestantController : AbstractController<ContestantController>
    {
        private readonly IContestantManager _contestantManager;

        public ContestantController
            (
                IMapper mapper,
                ILogger<ContestantController> logger,
                ITranslator translator,
                IContestantManager contestantManager,
                ISessionHelper sessionHelper,
                IRequestContextHelper requestContextHelper
            ) : base(mapper, logger, translator, sessionHelper, requestContextHelper)
        {
            _contestantManager = contestantManager;
        }

        [HttpGet()]
        public ActionResult<PaginatedResultResponse<ContestantResponse>> GetContestants([FromQuery] ContestantSearchFilterRequest searchFilterRequest, [FromQuery] PaginationFilterRequest paginationFilterRequest, [FromQuery] SortingRequest sortingRequest)
        {
            var searchFilter = _mapper.Map<ContestantSearchFilter>(searchFilterRequest);
            var paginationFilter = _mapper.Map<PaginationFilter>(paginationFilterRequest);
            var sorting = _mapper.Map<Sorting>(sortingRequest);

            var contestants = _contestantManager.Get(searchFilter, paginationFilter, sorting);

            var response = _mapper.Map<PaginatedResultResponse<ContestantResponse>>(contestants);

            return Ok(response);
        }

        [HttpPost()]
        public ActionResult<int> CreateContestant(ContestantRequest contestantRequest)
        {
            var contestant = _mapper.Map<Contestant>(contestantRequest);

            _contestantManager.Create(contestant);

            return Ok(contestant.Id);
        }

        [HttpPut()]
        public ActionResult UpdateContestant(ContestantRequest contestantRequest)
        {
            var contestant = _mapper.Map<Contestant>(contestantRequest);

            _contestantManager.Update(contestant);

            return Ok();
        }

        [HttpDelete("id")]
        public ActionResult DeleteContestant(int id)
        {
            _contestantManager.Delete(id);

            return Ok();
        }

        #region Private Methods

        #endregion
    }
}

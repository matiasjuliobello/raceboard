using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.DTOs.User.Request.Password;
using RaceBoard.Service.Helpers.Interfaces;
using RaceBoard.Service.Attributes;
using RaceBoard.Service.Controllers.Abstract;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Domain;
using RaceBoard.DTOs._Pagination.Request;
using RaceBoard.DTOs.User.Request;
using RaceBoard.DTOs._Pagination.Response;
using RaceBoard.DTOs.User.Response;
using RaceBoard.Translations.Interfaces;

namespace RaceBoard.Service.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : AbstractController<UserController>
    {
        private readonly IUserManager _userManager;

        public UserController
            (
                IMapper mapper,
                ILogger<UserController> logger,
                ITranslator translator,
                IUserManager userManager,
                ISessionHelper sessionHelper,
                IRequestContextManager requestContextManager
            ) : base(mapper, logger, translator, sessionHelper, requestContextManager)
        {
            _userManager = userManager;
        }

        [HttpGet()]
        public ActionResult<List<UserResponse>> Get([FromQuery] UserSearchFilterRequest userSearchFilterRequest, [FromQuery] PaginationFilterRequest? paginationFilterRequest = null, [FromQuery] SortingRequest? sortingRequest = null)
        {
            var searchFilter = _mapper.Map<UserSearchFilterRequest, UserSearchFilter>(userSearchFilterRequest);
            var paginationFilter = _mapper.Map<PaginationFilter>(paginationFilterRequest);
            var sorting = _mapper.Map<Sorting>(sortingRequest);

            var data = _userManager.Get(searchFilter, paginationFilter, sorting);

            var response = _mapper.Map<PaginatedResultResponse<UserResponse>>(data);

            return Ok(response);
        }

        [HttpPost()]
        public ActionResult<int> Create(UserRequest userRequest)
        {
            var user = _mapper.Map<User>(userRequest);

            _userManager.Create(user);

            return Ok(user.Id);
        }

        [HttpPut()]
        public ActionResult Update(UserRequest userRequest)
        {
            var user = _mapper.Map<User>(userRequest);

            _userManager.Update(user);

            return Ok();
        }

        [HttpDelete("id")]
        public ActionResult Delete(int id)
        {
            var user = _userManager.Delete(id);

            return Ok();
        }

        [HttpPut("password")]
        public ActionResult UpdatePassword(UserPasswordRequest userPasswordRequest)
        {
            var currentUser = base.GetUserFromRequestContext();
            
            _userManager.SavePassword(currentUser.Id, userPasswordRequest.Password);

            return Ok();
        }

        #region Private Methods

        #endregion
    }
}

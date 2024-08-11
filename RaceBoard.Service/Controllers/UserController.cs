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
        private readonly IRequestContextHelper _requestContextHelper;

        public UserController
            (
                IMapper mapper,
                ILogger<UserController> logger,
                ITranslator translator,
                IUserManager userManager,
                ISessionHelper sessionHelper,
                IRequestContextHelper requestContextHelper
            ) : base(mapper, logger, translator, sessionHelper, requestContextHelper)
        {
            _userManager = userManager;
            _requestContextHelper = requestContextHelper;
        }

        [HttpGet()]
        public ActionResult<List<UserResponse>> GetUsers([FromQuery] UserSearchFilterRequest userSearchFilterRequest, [FromQuery] PaginationFilterRequest paginationFilterRequest, [FromQuery] SortingRequest sortingRequest)
        {
            var searchFilter = _mapper.Map<UserSearchFilterRequest, UserSearchFilter>(userSearchFilterRequest);
            var paginationFilter = _mapper.Map<PaginationFilter>(paginationFilterRequest);
            var sorting = _mapper.Map<Sorting>(sortingRequest);

            var users = _userManager.Get(searchFilter, paginationFilter, sorting);

            var response = _mapper.Map<PaginatedResultResponse<UserResponse>>(users);

            return Ok(response);
        }

        [HttpPost()]
        public ActionResult<int> CreateUser(UserRequest userRequest)
        {
            var user = _mapper.Map<User>(userRequest);

            _userManager.Create(user);

            return Ok(user.Id);
        }

        [HttpPut()]
        public ActionResult UpdateUser(UserRequest userRequest)
        {
            var user = _mapper.Map<User>(userRequest);

            _userManager.Update(user);

            return Ok();
        }

        [HttpDelete("id")]
        public ActionResult DeleteUser(int id)
        {
            var user = _userManager.Delete(id);

            return Ok();
        }

        [HttpPut("password")]
        public ActionResult UpdatePassword(UserPasswordRequest userPasswordRequest)
        {
            var currentUser = _requestContextHelper.GetUser();
            
            _userManager.SavePassword(currentUser.Id, userPasswordRequest.Password);

            return Ok();
        }

        #region Private Methods

        #endregion
    }
}

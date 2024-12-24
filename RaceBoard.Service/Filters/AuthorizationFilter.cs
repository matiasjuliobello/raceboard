using System.Net;
using Microsoft.AspNetCore.Mvc.Filters;
using RaceBoard.Service.Filters.Interfaces;
using RaceBoard.Common.Helpers.Interfaces;
using RaceBoard.Translations.Interfaces;
using RaceBoard.Service.Helpers.Interfaces;
using RaceBoard.Business.Managers.Interfaces;
using Enums = RaceBoard.Domain.Enums;

namespace RaceBoard.Service.Filters
{
    public class AuthorizationFilter : BaseCustomActionFilter, IAuthorizationActionFilter
    {
        #region Private Members

        private readonly IAuthorizationManager _authorizationManager;

        #endregion

        #region Constructors

        public AuthorizationFilter
            (
                ISessionHelper sessionManager,
                IAuthorizationManager authorizationManager,
                IHttpHeaderHelper httpHeaderHelper,
                IDateTimeHelper dateTimeHelper,
                ITranslator translator
            )
            : base(httpHeaderHelper, sessionManager, dateTimeHelper, translator)
        {
            _authorizationManager = authorizationManager;
        }

        #endregion

        #region IAuthorizationActionFilter implementation

        public Enums.Action Action { get; set; }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            var condition = _authorizationManager.GetUserPermissionToPerformAction((int)this.Action, base.CurrentUser.Id);

            //_httpHeaderHelper.SetAuthorizationContext(new Common.AuthorizationContext() { Condition = condition });

            if (condition == Enums.AuthorizationCondition.Deny)
            {
                context.Result = base.CreateObjectResult(HttpStatusCode.Forbidden, base.Translate("NeedPermissions"));
                return;
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
        }

        #endregion

        #region Private Methods

        #endregion
    }
}
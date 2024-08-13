using System.Net;
using Microsoft.AspNetCore.Mvc.Filters;
using RaceBoard.Service.Filters.Interfaces;
using RaceBoard.Common.Helpers.Interfaces;
using RaceBoard.Translations.Interfaces;
using RaceBoard.Service.Helpers.Interfaces;

namespace RaceBoard.Service.Filters
{
    public class AuthorizationFilter : BaseCustomActionFilter, IAuthorizationActionFilter
    {
        #region Private Members

        #endregion

        #region Constructors

        public AuthorizationFilter
            (
                ISessionHelper sessionManager,
                IHttpHeaderHelper httpHeaderHelper,
                IDateTimeHelper dateTimeHelper,
                ITranslator translator
            )
            : base(httpHeaderHelper, sessionManager, dateTimeHelper, translator)
        {
        }

        #endregion

        #region ICustomActionFilter implementation

        public void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            //context.Result = base.CreateObjectResult(HttpStatusCode.Forbidden, base.Translate("NeedPermissions"));
            //return;
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
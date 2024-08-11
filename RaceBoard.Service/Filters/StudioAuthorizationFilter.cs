using System.Net;
using Microsoft.AspNetCore.Mvc.Filters;
using RaceBoard.Service.Filters.Interfaces;
using RaceBoard.Common.Helpers.Interfaces;
using RaceBoard.Translations.Interfaces;
using RaceBoard.Service.Helpers.Interfaces;
using Enums = RaceBoard.Common.Enums;

namespace RaceBoard.Service.Filters
{
    public class StudioAuthorizationFilter : BaseCustomActionFilter, IStudioAuthorizationActionFilter
    {
        #region Private Members

        #endregion

        #region Constructors

        public StudioAuthorizationFilter
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

        public Enums.Action Action { get; set; }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            if (base.CurrentRequestContext.IdStudio == null)
            {
                context.Result = base.CreateObjectResult(HttpStatusCode.Unauthorized, base.Translate("MissingStudioId"));
                return;
            }

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
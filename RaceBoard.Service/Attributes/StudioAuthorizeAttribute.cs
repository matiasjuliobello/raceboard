using RaceBoard.Service.Filters.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;

namespace RaceBoard.Service.Attributes
{
    public class StudioAuthorizeAttribute : ActionFilterAttribute, IFilterFactory
    {
        public Common.Enums.Action Action { get; set; }

        #region IFilterFactory implementation

        public bool IsReusable => false;

        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            var filter = serviceProvider.GetService<IStudioAuthorizationActionFilter>();
                filter.Action = this.Action;

            return filter;
        }

        #endregion
    }
}

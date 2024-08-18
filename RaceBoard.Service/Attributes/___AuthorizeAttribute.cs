using RaceBoard.Service.Filters.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;

namespace RaceBoard.Service.Attributes
{
    public class ___AuthorizeAttribute : ActionFilterAttribute, IFilterFactory
    {
        #region IFilterFactory implementation

        public bool IsReusable => false;

        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            var filter = serviceProvider.GetService<IAuthorizationActionFilter>();

            return filter;
        }

        #endregion
    }
}

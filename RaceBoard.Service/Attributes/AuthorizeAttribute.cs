using RaceBoard.Service.Filters.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;
using Enums = RaceBoard.Domain.Enums;

namespace RaceBoard.Service.Attributes
{
    public class AuthorizeAttribute : ActionFilterAttribute, IFilterFactory
    {
        public Enums.Action Action { get; set; }

        #region IFilterFactory implementation

        public bool IsReusable => false;

        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            var filter = serviceProvider.GetService<IAuthorizationActionFilter>();
            filter.Action = this.Action;

            return filter;
        }

        #endregion
    }
}

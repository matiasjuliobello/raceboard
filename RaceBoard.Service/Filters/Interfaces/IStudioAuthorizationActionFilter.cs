using Microsoft.AspNetCore.Mvc.Filters;

namespace RaceBoard.Service.Filters.Interfaces
{
    public interface IStudioAuthorizationActionFilter : IActionFilter
    {
        Common.Enums.Action Action { get; set; }
    }
}

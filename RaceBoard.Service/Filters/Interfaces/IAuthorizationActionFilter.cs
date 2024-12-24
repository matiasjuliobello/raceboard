using Microsoft.AspNetCore.Mvc.Filters;
using Enums = RaceBoard.Domain.Enums;

namespace RaceBoard.Service.Filters.Interfaces
{
    public interface IAuthorizationActionFilter : IActionFilter
    {
        Enums.Action Action { get; set; }
    }
}
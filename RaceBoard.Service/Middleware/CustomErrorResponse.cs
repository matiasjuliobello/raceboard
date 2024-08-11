using System.Diagnostics.CodeAnalysis;

namespace RaceBoard.Service.Middleware
{
    [ExcludeFromCodeCoverage]
    public class CustomErrorResponse
    {
        public string Message { get; set; }
        public IEnumerable<string> Errors { get; set; }

        public CustomErrorResponse(string message, IEnumerable<string> errors)
        {
            Message = message;
            Errors = errors;
        }
    }
}

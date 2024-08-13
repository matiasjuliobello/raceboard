using RaceBoard.Common;
using RaceBoard.Common.Exceptions;
using RaceBoard.Common.Helpers;
using RaceBoard.Common.Helpers.Interfaces;
using RaceBoard.Service.Helpers.Interfaces;
using Microsoft.Extensions.Primitives;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Text;

namespace RaceBoard.Service.Middleware
{
    [ExcludeFromCodeCoverage]
    public class CustomMiddleware
    {
        #region Private Members

        private readonly RequestDelegate _next;
        private readonly ILogger<CustomMiddleware> _logger;
        private readonly bool _useEnrichedLogging = false;
        private readonly string[] _ignoredPaths = new string[]
        {
            "/swagger/"
        };
        private readonly string[] _httpMethodsWithBody = new string[]
        {
            WebRequestMethods.Http.Post,
            WebRequestMethods.Http.Put
        };

        #endregion

        public CustomMiddleware(RequestDelegate next, ILogger<CustomMiddleware> logger, IConfiguration configuration)
        {
            _next = next;

            _logger = logger;

            _useEnrichedLogging = Convert.ToBoolean(configuration["Logging_Enriched"]);
        }

        public async Task Invoke(HttpContext httpContext, ICorrelationIdGenerator correlationIdGenerator, IHttpHeaderHelper httpHeaderHelper)
        {
            try
            {
                string correlationId = GetCorrelationId(httpContext, correlationIdGenerator);
                AddCorrelationIdHeaderToResponse(httpContext, correlationId);

                //AddCorsHeaderToResponse(httpContext);

                LogInformation(httpContext, httpHeaderHelper);

                await _next.Invoke(httpContext);
            }
            catch (Exception e)
            {
                await HandleExceptionAsync(httpContext, e);
            }
        }

        #region Private Methods

        #region Exception Handling

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            HttpStatusCode statusCode;
            string message;
            List<string> errors;

            bool isFunctionalError = false;

            if (exception is FunctionalException)
            {
                isFunctionalError = true;

                var functionalException = (FunctionalException)exception;
                message = functionalException.Message;
                statusCode = (HttpStatusCode)functionalException.FunctionalError;
                errors = functionalException.Errors;
            }
            else
            {
                isFunctionalError = false;
                message = "Unexpected Error";
                statusCode = HttpStatusCode.InternalServerError;
                errors = new List<string> { message };
            }

            if (isFunctionalError)
            {
                message = statusCode.ToString();
                _logger.LogWarning(message);
            }
            else
            {
                _logger.LogError(exception, "");
            }

            var customErrorResponse = new CustomErrorResponse(message, errors);
            var serialiationHelper = new SerializationHelper();
            var serialized = serialiationHelper.Serialize(customErrorResponse);

            var response = context.Response;
            response.ContentType = "application/json";
            response.StatusCode = (int)statusCode;

            await response.WriteAsync(serialized);
        }

        #endregion

        #region Logging

        private void LogInformation(HttpContext httpContext, IHttpHeaderHelper httpHeaderHelper)
        {
            string path = httpContext.Request.Path;
            
            foreach(var ignored in _ignoredPaths)
            {
                if (path.StartsWith(ignored))
                    return;
            }

            bool isLogin = path == "/api/login";

            path += $"{httpContext.Request.QueryString.Value}";

            string method = httpContext.Request.Method;
            string info = "";
            string body = "";

            if (_useEnrichedLogging)
            {
                string protocol = httpContext.Request.Protocol;
                string host = httpContext.Request.Host.Host;

                info = $"{protocol} {method} {host}{path}";

                if (!isLogin)
                    body = GetHttpRequestBodyAsString(method, httpContext.Request);
            }
            else
            {
                info = $"{method.PadRight(4, ' ')} {path}";
            }

            RequestContext requestContext = httpHeaderHelper.GetContext();
            if (requestContext != null)
            {
                info += $" [Username: {requestContext.Username}]";
            }

            if (!string.IsNullOrEmpty(body))
                info += Environment.NewLine + body;

            _logger.LogInformation(info);
        }

        private string GetHttpRequestBodyAsString(string method, HttpRequest httpRequest)
        {
            try
            {
                if (!_httpMethodsWithBody.Contains(method))
                    return string.Empty;

                return ReadBodyFromHttpRequest(httpRequest);
            }
            catch (Exception)
            {
            }

            return null;
        }

        private string ReadBodyFromHttpRequest(HttpRequest httpRequest)
        {
            string body = null;

            httpRequest.EnableBuffering();

            Encoding encoding = Encoding.UTF8;
            int bufferSize = -1;
            bool leaveOpen = true; // Leave the body open so the next middleware can read it.
            using (var reader = new StreamReader(httpRequest.Body, encoding: encoding, detectEncodingFromByteOrderMarks: false, bufferSize: bufferSize, leaveOpen: leaveOpen))
            {
                body = reader.ReadToEndAsync().Result;

                if (httpRequest.Body.CanSeek)
                    httpRequest.Body.Position = 0; // Reset the request body stream position so the next middleware can read it
            }

            return body;
        }

        #endregion

        #region Correlation ID

        private static string GetCorrelationId(HttpContext context, ICorrelationIdGenerator correlationIdGenerator)
        {
            StringValues correlationId;

            if (context.Request.Headers.TryGetValue(CommonValues.HttpCustomHeaders.CorrelationId, out correlationId))
            {
                correlationIdGenerator.Set(correlationId);
            }
            else
            {
                correlationId = correlationIdGenerator.Get();
            }

            return correlationId;
        }

        private static void AddCorrelationIdHeaderToResponse(HttpContext context, string correlationId)
        {
            context.Response.OnStarting(() =>
            {
                if (!context.Response.Headers.ContainsKey(CommonValues.HttpCustomHeaders.CorrelationId))
                    context.Response.Headers.Add(CommonValues.HttpCustomHeaders.CorrelationId, new[] { correlationId.ToString() });

                return Task.CompletedTask;
            });
        }

        //private static void AddCorsHeaderToResponse(HttpContext context)
        //{
        //    context.Response.OnStarting(() =>
        //    {
        //        if (!context.Response.Headers.ContainsKey("Access-Control-Allow-Origin"))
        //            context.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "http://localhost:4200" });

        //        return Task.CompletedTask;
        //    });
        //}

        #endregion

        #endregion
    }
}


namespace RaceBoard.Common
{
    public static class CommonValues
    {
        public static class AuthenticationTokenTypes
        {
            public const string Bearer = "Bearer";
        }

        public static class HttpMethods
        {
            public const string GET = "GET";
            public const string POST = "POST";
            public const string PUT = "PUT";
            public const string DELETE = "DELETE";
            public const string OPTIONS = "OPTIONS";
        }

        public static class HttpCustomHeaders
        {
            public const string Authorization = "Authorization";
            public const string Language = "X-Language";
            public const string CorrelationId = "X-Correlation-Id";
        }

        public static class MimeTypes
        {
            public const string ApplicationOctetStream = "application/octet-stream";
        }

        public static class SystemDefaults
        {
            public const string Culture = "es-AR";
        }
    }
}

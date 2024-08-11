using RaceBoard.Common;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace RaceBoard.Service.Swagger.CustomParameters
{
    public class LanguageOperationFilter : IOperationFilter
    {
        private const string _DEFAULT_LANGUAGE = CommonValues.SystemDefaults.Culture;
        private readonly string _defaultValue;

        public LanguageOperationFilter(IConfiguration configuration)
        {
            _defaultValue = configuration["Translator_DefaultLanguage"] ?? _DEFAULT_LANGUAGE;
        }

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var parameter = CreateHeaderParameter(CommonValues.HttpCustomHeaders.Language, "Language");

            operation.Parameters.Add(parameter);
        }

        #region Private Methods

        private OpenApiParameter CreateHeaderParameter(string name, string description)
        {
            return new OpenApiParameter
            {
                Name = name,
                In = ParameterLocation.Header,
                Description = description,
                Required = false,
                AllowEmptyValue = true,
                Schema = new OpenApiSchema
                {
                    Type = "string",
                    Default = new Microsoft.OpenApi.Any.OpenApiString(_defaultValue)
                }
            };
        }

        #endregion
    }
}

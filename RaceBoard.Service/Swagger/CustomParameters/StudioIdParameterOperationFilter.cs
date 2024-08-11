using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using RaceBoard.Common;
using RaceBoard.Service.Attributes;

namespace RaceBoard.Service.Swagger.CustomParameters
{
    public class StudioIdParameterOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            //var allowAnonymous = HasAnonymousAttribute(context);
            //if (!allowAnonymous)
            //{
            //    var parameter = CreateHeaderParameter(CommonKeys.Headers.Id_Studio, "Studio identifier");

            //    operation.Parameters.Add(parameter);
            //}
            
            var requiresStudio = HasStudioAuthorizeAttribute(context);
            if (requiresStudio)
            {
                var parameter = CreateHeaderParameter(CommonValues.HttpCustomHeaders.StudioId, "Studio identifier");

                operation.Parameters.Add(parameter);
            }
        }

        #region Private Methods

        private bool HasAnonymousAttribute(OperationFilterContext context)
        {
            return  context.MethodInfo
                        .DeclaringType
                        .GetCustomAttributes(true)
                        .OfType<AllowAnonymousAttribute>().Any()
                        ||
                    context.MethodInfo
                        .GetCustomAttributes(true)
                        .OfType<AllowAnonymousAttribute>().Any();
        }

        private bool HasStudioAuthorizeAttribute(OperationFilterContext context)
        {
            return context.MethodInfo
                        .DeclaringType
                        .GetCustomAttributes(true)
                        .OfType<StudioAuthorizeAttribute>().Any()
                        ||
                    context.MethodInfo
                        .GetCustomAttributes(true)
                        .OfType<StudioAuthorizeAttribute>().Any();
        }

        private OpenApiParameter CreateHeaderParameter(string name, string description)
        {
            return new OpenApiParameter
            {
                Name = name,
                In = ParameterLocation.Header,
                Description = description,
                Required = true,
                AllowEmptyValue = false,
                Schema = new OpenApiSchema
                {
                    Type = "integer"
                }
            };
        }

        #endregion
    }
}

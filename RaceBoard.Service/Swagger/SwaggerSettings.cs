using RaceBoard.Service.Swagger.CustomParameters;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace RaceBoard.Service.Swagger
{
    public static class SwaggerSettings
    {
        #region Private Members

        private static string _PRODUCT_NAME
        {
            get
            {
                return "Race Board";
            }
        }
        private static string _PRODUCT_VERSION
        {
            get
            {
                return Environment.GetEnvironmentVariable("BuildId");
            }
        }
        private static string _ENVIRONMENT_SERVER_URL
        {
            get
            {
                return Environment.GetEnvironmentVariable("SwaggerEnvironmentServerUrl");
            }
        }

        #endregion

        #region Public Methods

        public static void ConfigureSwagger(this IServiceCollection services, IWebHostEnvironment environment, string xmlFileName)
        {
            bool isProduction = environment.IsProduction();
            bool addAuthorization = true;

            services.AddSwaggerGen(options =>
            {
                if (!isProduction)
                {
                    AddSwaggerServers(options, environment);
                }

                if (addAuthorization)
                {
                    AddAuthorization(options);
                }

                AddCustomFilters(options);

                AddDocumentation(options);

                SetSchemas(options);

                IncludeXmlComments(options, xmlFileName);
            });
        }

        public static void UseSwagger(this IApplicationBuilder app, IWebHostEnvironment environment, IConfiguration configuration)
        {
            bool isProduction = environment.IsProduction();

            var swaggerOptions = new SwaggerOptions();

            configuration.GetSection(nameof(SwaggerOptions)).Bind(swaggerOptions);

            // Enable the middleware for serving the generated JSON document and the Swagger UI,
            if (!isProduction)
            {
                app.UseSwagger(option => 
                {
                    option.RouteTemplate = swaggerOptions.RouteTemplate; 
                });

                app.UseSwaggerUI(option =>
                {
                    option.SwaggerEndpoint("/swagger/v1/swagger.json", $"{_PRODUCT_NAME} API v{_PRODUCT_VERSION}");
                    option.EnableTryItOutByDefault();
                    option.DisplayRequestDuration();
                    option.DocExpansion(DocExpansion.None);
                    option.EnablePersistAuthorization();
                    //option.EnableFilter();
                    option.DefaultModelRendering(ModelRendering.Example);
                    //option.HeadContent = "<span>Hello!</span>";
                    option.DocumentTitle = "Race Board API documentation";
                    option.InjectStylesheet("/swagger/css/swagger-ui-custom.css");
                    option.InjectJavascript("/swagger/js/swagger-ui-custom.js", "text/javascript");
                });
            }
        }

        #endregion

        #region Private Methods

        private static void AddAuthorization(SwaggerGenOptions options)
        {
            options.AddSecurityDefinition
            (
                Common.CommonValues.AuthenticationTokenTypes.Bearer,
                new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = Common.CommonValues.AuthenticationTokenTypes.Bearer
                }
            );

            options.AddSecurityRequirement
            (
                new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Id = Common.CommonValues.AuthenticationTokenTypes.Bearer,
                                Type = ReferenceType.SecurityScheme
                            }
                        },
                        new List<string>()
                    }
                }
            );
        }

        private static void AddDocumentation(SwaggerGenOptions options)
        {
            options.SwaggerDoc
                (
                    "v1",
                    new OpenApiInfo
                    {
                        Title = $"{_PRODUCT_NAME} API",
                        Description = $".NET Core API designed to serve as backend for {_PRODUCT_NAME} site.",
                        Version = _PRODUCT_VERSION
                    }
                );
        }

        private static void SetSchemas(SwaggerGenOptions options)
        {
            options.CustomSchemaIds(x => x.FullName);
            options.UseAllOfToExtendReferenceSchemas();
        }

        private static void IncludeXmlComments(SwaggerGenOptions options, string xmlFileName)
        {
            string filePath = Path.Combine(AppContext.BaseDirectory, xmlFileName);

            options.IncludeXmlComments(filePath, includeControllerXmlComments: false);
        }

        private static void AddSwaggerServers(SwaggerGenOptions options, IWebHostEnvironment environment)
        {
            string[] urls = new[]
            {
                GetLocalApplicationUrl(environment),
                _ENVIRONMENT_SERVER_URL
            };

            foreach (string url in urls)
            {
                if (!string.IsNullOrEmpty(url))
                    options.AddServer(new OpenApiServer() { Url = url });
            }
        }

        private static string GetLocalApplicationUrl(IWebHostEnvironment env)
        {
            string localApplicationUrl = string.Empty;

            try
            {
                string propertiesFolder = $"{env.ContentRootPath}/Properties";
                string launchSettingsFile = "launchSettings.json";

                var config = new ConfigurationBuilder()
                      .SetBasePath(propertiesFolder)
                      .AddJsonFile(launchSettingsFile)
                      .Build();

                IConfigurationSection configSection = config.GetSection("iisSettings:iisExpress:applicationUrl");
                if (configSection == null)
                    return string.Empty;

                localApplicationUrl = configSection.Value;
            }
            catch (Exception)
            {
                // swallow exception intentionally
            }

            return localApplicationUrl;
        }

        private static void AddCustomFilters(SwaggerGenOptions options)
        {
            options.OperationFilter<LanguageOperationFilter>();
        }


        #endregion
    }
}
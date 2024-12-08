using System.Globalization;
using RaceBoard.IoC;
using RaceBoard.Service.Filters;
using RaceBoard.Service.Filters.Interfaces;
using RaceBoard.Service.Mappings;
using RaceBoard.Service.Middleware;
using RaceBoard.Service.Swagger;
using RaceBoard.Service.Helpers.Interfaces;
using RaceBoard.Service.Helpers;
using RaceBoard.Common;

namespace RaceBoard.Service
{
    public class Startup
    {
        private const string _CURRENT_CULTURE = CommonValues.SystemDefaults.Culture;

        #region Private Members

        private IConfiguration _configuration { get; }
        private IWebHostEnvironment _environment { get; }

        private string _cors_policy_name = "CORS_raceboard.com";
        private readonly string[] _cors_allowed_origins;

        #endregion

        #region Constructors

        public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            _configuration = configuration;
            _environment = webHostEnvironment;

            _cors_allowed_origins =  _configuration["CORS_Allowed_Origins"].Split(";", StringSplitOptions.RemoveEmptyEntries);
        }

        #endregion

        #region Public Methods

        //private DiskFileStorageProvider GetDiskFileStorageProvider(IConfiguration configuration)
        //{
        //    return new DiskFileStorageProvider();
        //}

        //private BlobFileStorageProvider GetBlobStorageProvider(IConfiguration configuration)
        //{
        //    return new BlobFileStorageProvider(configuration);
        //}

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AutoMapperProfile));

            services.AddMemoryCache();

            services.AddControllers(options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true);

            services.ConfigureIoC(_configuration);
            services.AddTransient<IAuthorizationActionFilter, AuthorizationFilter>();
            services.AddScoped<IRequestContextHelper, RequestContextHelper>();
            services.AddScoped<ICorrelationIdGenerator, CorrelationIdGenerator>();
            services.AddScoped<ISessionHelper, SessionHelper>();

            //var fileStorageProviderFactory = new FileStorageProviderFactory();
            //fileStorageProviderFactory.Register(FileStorage.Enums.FileStorageProviderType.Disk, GetDiskFileStorageProvider(_configuration));
            //fileStorageProviderFactory.Register(FileStorage.Enums.FileStorageProviderType.Blob, GetBlobStorageProvider(_configuration));
            //services.AddTransient<IFileStorageProviderFactory>(x => fileStorageProviderFactory);


            services.ConfigureSwagger(_environment, "RaceBoard.Service.xml");

            services.AddHttpContextAccessor();

            services.AddCors
                (
                    options => options.AddPolicy
                    (
                        _cors_policy_name, 
                        p => p
                            .WithOrigins(_cors_allowed_origins)
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                    )
                );
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            SetCurrentCulture(_CURRENT_CULTURE);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<CustomMiddleware>();

            app.UseRouting();

            app.UseCors(_cors_policy_name);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseStaticFiles();

            app.UseSwagger(env, _configuration);
        }

        #endregion

        #region Private Methods

        private void SetCurrentCulture(string name)
        {
            var cultureInfo = new CultureInfo(name);

            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
            CultureInfo.CurrentCulture = cultureInfo;
            CultureInfo.CurrentUICulture = cultureInfo;
        }

        #endregion
    }
}
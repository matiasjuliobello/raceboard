using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using RaceBoard.Business.Managers;
using RaceBoard.Data.Repositories;
using RaceBoard.Common.Helpers;
using RaceBoard.Data.Helpers;
using RaceBoard.Common.Helpers.Interfaces;
using RaceBoard.Data.Helpers.Interfaces;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Data.Repositories.Base.Interfaces;
using RaceBoard.Business.Validators.Interfaces;
using RaceBoard.Business.Validators;
using RaceBoard.Translations.Interfaces;
using RaceBoard.Translations;
using RaceBoard.Domain;
using RaceBoard.FileStorage.Interfaces;
using RaceBoard.FileStorage;
using RaceBoard.Mailing.Interfaces;
using RaceBoard.Mailing.Providers;

namespace RaceBoard.IoC
{
    public static class IoC
    {
        public static void ConfigureIoC(this IServiceCollection services, IConfiguration configuration)
        {
            #region Managers

            services.AddScoped<IAuthenticationManager, AuthenticationManager>();
            services.AddScoped<IUserManager, UserManager>();
            services.AddScoped<IUserSettingsManager, UserSettingsManager>();
            services.AddScoped<IUserPasswordResetManager, UserPasswordResetManager>();
            services.AddScoped<ITimeZoneManager, TimeZoneManager>();
            services.AddScoped<ICompetitionManager, CompetitionManager>();
            services.AddScoped<IOrganizationManager, OrganizationManager>();
            services.AddScoped<IBoatManager, BoatManager>();
            services.AddScoped<IRaceClassManager, RaceClassManager>();
            services.AddScoped<IRaceManager, RaceManager>();
            services.AddScoped<IPersonManager, PersonManager>();
            services.AddScoped<IContestantManager, ContestantManager>();
            services.AddScoped<IContestantRoleManager, ContestantRoleManager>();
            services.AddScoped<ITeamManager, TeamManager>();

            #endregion

            #region Validators

            services.AddTransient<ICustomValidator<User>, UserValidator>();
            services.AddTransient<ICustomValidator<UserPassword>, UserPasswordValidator>();
            services.AddTransient<ICustomValidator<UserPasswordReset>, UserPasswordResetValidator>();
            
            #endregion

            #region Repositories

            services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserSettingsRepository, UserSettingsRepository>();
            services.AddScoped<IUserPasswordResetRepository, UserPasswordResetRepository>();
            services.AddScoped<ITranslationRepository, FakeTranslationRepository>();
            services.AddScoped<ITimeZoneRepository, TimeZoneRepository>();
            services.AddScoped<ICompetitionRepository, CompetitionRepository>();
            services.AddScoped<IOrganizationRepository, OrganizationRepository>();
            services.AddScoped<IBoatRepository, BoatRepository>();
            services.AddScoped<IRaceClassRepository, RaceClassRepository>();
            services.AddScoped<IRaceRepository, RaceRepository>();
            services.AddScoped<IPersonRepository, PersonRepository>();
            services.AddScoped<IContestantRepository, ContestantRepository>();
            services.AddScoped<IContestantRoleRepository, ContestantRoleRepository>();
            services.AddScoped<ITeamRepository, TeamRepository>();

            #endregion

            #region Helpers

            services.AddScoped<IConfigurationHelper, ConfigurationHelper>();
            services.AddTransient<IQueryBuilder, SqlQueryBuilder>();
            services.AddTransient<ICryptographyHelper, CryptographyHelper>();
            services.AddScoped<IHttpHeaderHelper, HttpHeaderHelper>();
            services.AddScoped<ISecurityTicketHelper, SecurityTicketHelper>();
            services.AddScoped<ICacheHelper, CacheHelper>();
            services.AddScoped<IStringHelper, StringHelper>();
            services.AddScoped<IFileHelper, FileHelper>();
            services.AddScoped<ICompressionHelper, CompressionHelper>();
            services.AddScoped<IDateTimeHelper, DateTimeHelper>();
            services.AddScoped<IFormatHelper, FormatHelper>();

            #endregion

            #region Providers

            services.AddScoped<ITranslationProvider, TranslationProvider>();
            services.AddScoped<IFileStorageProvider, BlobFileStorageProvider>();
            services.AddScoped<IFileStorageProvider, DiskFileStorageProvider>();
            services.AddScoped<IEmailProvider, MailSenderEmailProvider>();

            #endregion

            services.AddScoped<IContextResolver, ContextResolver>();
            services.AddScoped<ITranslator, Translator>();
        }
    }
}
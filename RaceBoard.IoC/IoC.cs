﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using RaceBoard.Business.Managers;
using RaceBoard.Data.Repositories;
using RaceBoard.Common.Helpers;
using RaceBoard.Data.Helpers;
using RaceBoard.Common.Helpers.Interfaces;
using RaceBoard.Data.Helpers.Interfaces;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Business.Validators.Interfaces;
using RaceBoard.Business.Validators;
using RaceBoard.Translations.Interfaces;
using RaceBoard.Translations;
using RaceBoard.Domain;
using RaceBoard.FileStorage.Interfaces;
using RaceBoard.FileStorage;
using RaceBoard.Mailing.Interfaces;
using RaceBoard.Mailing.Providers;
using RaceBoard.Data.Helpers.SqlBulkHelper;
using RaceBoard.Messaging.Interfaces;
using RaceBoard.Messaging.Providers;

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
            services.AddScoped<IDeviceManager, DeviceManager>();
            services.AddScoped<ITimeZoneManager, TimeZoneManager>();
            services.AddScoped<ICityManager, CityManager>();
            services.AddScoped<ICountryManager, CountryManager>();
            services.AddScoped<ICompetitionManager, CompetitionManager>();
            services.AddScoped<ICompetitionFileManager, CompetitionFileManager>();
            services.AddScoped<ICompetitionFlagManager, CompetitionFlagManager>();
            services.AddScoped<ICompetitionNotificationManager, CompetitionNotificationManager>();
            services.AddScoped<IOrganizationManager, OrganizationManager>();
            services.AddScoped<IBoatManager, BoatManager>();
            services.AddScoped<IRaceClassManager, RaceClassManager>();
            services.AddScoped<IRaceCategoryManager, RaceCategoryManager>();
            services.AddScoped<IRaceManager, RaceManager>();
            services.AddScoped<IPersonManager, PersonManager>();
            services.AddScoped<IContestantRoleManager, ContestantRoleManager>();
            services.AddScoped<ITeamManager, TeamManager>();
            services.AddScoped<ITeamBoatManager, TeamBoatManager>();
            services.AddScoped<ITeamContestantManager, TeamContestantManager>();
            services.AddScoped<ITeamCheckManager, TeamCheckManager>();
            services.AddScoped<IBloodTypeManager, BloodTypeManager>();
            services.AddScoped<IMedicalInsuranceManager, MedicalInsuranceManager>();
            //services.AddScoped<ICountryManager, CountryManager>();
            services.AddScoped<IFlagManager, FlagManager>();
            services.AddScoped<ILanguageManager, LanguageManager>();
            services.AddScoped<IFormatManager, FormatManager>();
            services.AddScoped<IFileTypeManager, FileTypeManager>();
            services.AddScoped<INotificationManager, NotificationManager>();
            #endregion

            #region Validators
            services.AddTransient<ICustomValidator<User>, UserValidator>();
            services.AddTransient<ICustomValidator<UserPassword>, UserPasswordValidator>();
            services.AddTransient<ICustomValidator<UserPasswordReset>, UserPasswordResetValidator>();
            services.AddTransient<ICustomValidator<Person>, PersonValidator>();
            services.AddTransient<ICustomValidator<Boat>, BoatValidator>();
            services.AddTransient<ICustomValidator<Competition>, CompetitionValidator>();
            services.AddTransient<ICustomValidator<CompetitionNotification>, CompetitionNotificationValidator>();
            services.AddTransient<ICustomValidator<CompetitionGroup>, CompetitionGroupValidator>();
            services.AddTransient<ICustomValidator<CompetitionFile>, CompetitionFileValidator>();
            services.AddTransient<ICustomValidator<CompetitionFlag>, CompetitionFlagValidator>();
            services.AddTransient<ICustomValidator<Organization>, OrganizationValidator>();
            services.AddTransient<ICustomValidator<Race>, RaceValidator>();
            services.AddTransient<ICustomValidator<RaceProtest>, RaceProtestValidator>();
            services.AddTransient<ICustomValidator<CommitteeBoatReturn>, RaceCommitteeBoatReturnValidator>();
            services.AddTransient<ICustomValidator<Team>, TeamValidator>();
            services.AddTransient<ICustomValidator<TeamBoat>, TeamBoatValidator>();
            services.AddTransient<ICustomValidator<TeamContestant>, TeamContestantValidator>();
            services.AddTransient<ICustomValidator<TeamContestantCheck>, TeamCheckValidator>();
            #endregion

            #region Repositories
            services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserSettingsRepository, UserSettingsRepository>();
            services.AddScoped<IUserPasswordResetRepository, UserPasswordResetRepository>();
            services.AddScoped<IDeviceRepository, DeviceRepository>();
            services.AddScoped<IDeviceSubscriptionRepository, DeviceSubscriptionRepository>();
            services.AddScoped<ITranslationRepository, FakeTranslationRepository>();
            services.AddScoped<ITimeZoneRepository, TimeZoneRepository>();
            services.AddScoped<ICityRepository, CityRepository>();
            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped<ICompetitionRepository, CompetitionRepository>();
            services.AddScoped<ICompetitionGroupRepository, CompetitionGroupRepository>();
            services.AddScoped<ICompetitionFlagRepository, CompetitionFlagRepository>();
            services.AddScoped<ICompetitionFileRepository, CompetitionFileRepository>();
            services.AddScoped<ICompetitionNotificationRepository, CompetitionNotificationRepository>();
            services.AddScoped<IFileRepository, FileRepository>();
            services.AddScoped<IFileTypeRepository, FileTypeRepository>();
            services.AddScoped<IOrganizationRepository, OrganizationRepository>();
            services.AddScoped<IBoatRepository, BoatRepository>();
            services.AddScoped<IRaceClassRepository, RaceClassRepository>();
            services.AddScoped<IRaceCategoryRepository, RaceCategoryRepository>();
            services.AddScoped<IRaceRepository, RaceRepository>();
            services.AddScoped<IRaceProtestRepository, RaceProtestRepository>();
            services.AddScoped<ICommitteeBoatReturnRepository, CommitteeBoatReturnRepository>();
            services.AddScoped<IPersonRepository, PersonRepository>();
            services.AddScoped<IContestantRoleRepository, ContestantRoleRepository>();
            services.AddScoped<ITeamRepository, TeamRepository>();
            services.AddScoped<ITeamBoatRepository, TeamBoatRepository>();
            services.AddScoped<ITeamContestantRepository, TeamContestantRepository>();
            services.AddScoped<ITeamCheckRepository, TeamCheckRepository>();
            services.AddScoped<IBloodTypeRepository, BloodTypeRepository>();
            services.AddScoped<IMedicalInsuranceRepository, MedicalInsuranceRepository>();
            services.AddScoped<IFlagRepository, FlagRepository>();
            services.AddScoped<ILanguageRepository, LanguageRepository>();
            services.AddScoped<IFormatRepository, FormatRepository>();
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
            services.AddScoped<ISqlBulkInsertHelper, SqlBulkInsertHelper>();
            #endregion

            #region Providers
            services.AddScoped<ITranslationProvider, TranslationProvider>();
            services.AddScoped<INotificationProvider, GoogleFirebaseNotificationProvider>();
            services.AddScoped<IFileStorageProvider, BlobFileStorageProvider>();
            services.AddScoped<IFileStorageProvider, DiskFileStorageProvider>();
            services.AddScoped<IEmailProvider, MailSenderEmailProvider>();
            #endregion

            services.AddScoped<IContextResolver, ContextResolver>();
            services.AddScoped<ITranslator, Translator>();
        }
    }
}
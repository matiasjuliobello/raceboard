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
using RaceBoard.Business.Validators.Interfaces;
using RaceBoard.Business.Validators;
using RaceBoard.Translations.Interfaces;
using RaceBoard.Translations;
using RaceBoard.Domain;
using RaceBoard.FileStorage.Interfaces;
using RaceBoard.FileStorage;
using RaceBoard.Data.Helpers.SqlBulkHelper;
using RaceBoard.Messaging.Interfaces;
using RaceBoard.Messaging.Providers;
using RaceBoard.Managers.Interfaces;
using RaceBoard.Notification.Interfaces;
using RaceBoard.Business.Factories;
using RaceBoard.Business.Helpers.Interfaces;
using RaceBoard.Business.Helpers;
using RaceBoard.Mailing.Interfaces;
using RaceBoard.Mailing.Providers;
using EmailStrategies = RaceBoard.Business.Strategies.Notifications.Email;
using PushStrategies = RaceBoard.Business.Strategies.Notifications.Push;

namespace RaceBoard.IoC
{
    public static class IoC
    {
        public static void ConfigureIoC(this IServiceCollection services, IConfiguration configuration)
        {
            #region Managers
            services.AddScoped<IAuthenticationManager, AuthenticationManager>();
            services.AddScoped<IAuthorizationManager, AuthorizationManager>();
            services.AddScoped<IUserManager, UserManager>();
            services.AddScoped<IUserSettingsManager, UserSettingsManager>();
            services.AddScoped<IUserPasswordResetManager, UserPasswordResetManager>();
            services.AddScoped<IRoleManager, RoleManager>();
            services.AddScoped<IDeviceManager, DeviceManager>();
            services.AddScoped<ITimeZoneManager, TimeZoneManager>();
            services.AddScoped<ICityManager, CityManager>();
            services.AddScoped<ICountryManager, CountryManager>();
            services.AddScoped<IChampionshipManager, ChampionshipManager>();
            services.AddScoped<IChampionshipMemberManager, ChampionshipMemberManager>();
            services.AddScoped<IChampionshipFileManager, ChampionshipFileManager>();
            services.AddScoped<IChampionshipFlagManager, ChampionshipFlagManager>();
            services.AddScoped<IChampionshipNotificationManager, ChampionshipNotificationManager>();
            services.AddScoped<IOrganizationManager, OrganizationManager>();
            services.AddScoped<IOrganizationMemberManager, OrganizationMemberManager>();
            services.AddScoped<IBoatManager, BoatManager>();
            services.AddScoped<IBoatOrganizationManager, BoatOrganizationManager>();
            services.AddScoped<IRaceClassManager, RaceClassManager>();
            services.AddScoped<IRaceCategoryManager, RaceCategoryManager>();
            services.AddScoped<IRaceManager, RaceManager>();
            services.AddScoped<IPersonManager, PersonManager>();
            services.AddScoped<ITeamMemberRoleManager, TeamMemberRoleManager>();
            services.AddScoped<ITeamManager, TeamManager>();
            services.AddScoped<ITeamMemberManager, TeamMemberManager>();
            services.AddScoped<ITeamBoatManager, TeamBoatManager>();
            services.AddScoped<ITeamCheckManager, TeamCheckManager>();
            services.AddScoped<IBloodTypeManager, BloodTypeManager>();
            services.AddScoped<IMedicalInsuranceManager, MedicalInsuranceManager>();
            //services.AddScoped<ICountryManager, CountryManager>();
            services.AddScoped<IFlagManager, FlagManager>();
            services.AddScoped<ILanguageManager, LanguageManager>();
            services.AddScoped<IFormatManager, FormatManager>();
            services.AddScoped<IFileTypeManager, FileTypeManager>();
            services.AddScoped<IPushNotificationManager, PushNotificationManager>();
            services.AddScoped<IMailManager, MailManager>();
            //services.AddScoped<IInvitationManager, InvitationManager>();
            services.AddScoped<IRequestManager, RequestManager>();
            services.AddScoped<IFileManager, FileManager>();
            services.AddScoped<IMemberManager, MemberManager>();
            #endregion

            #region Validators
            services.AddTransient<ICustomValidator<RolePermissions>, RolePermissionsValidator>();
            services.AddTransient<ICustomValidator<User>, UserValidator>();
            services.AddTransient<ICustomValidator<UserPassword>, UserPasswordValidator>();
            services.AddTransient<ICustomValidator<UserPasswordReset>, UserPasswordResetValidator>();
            services.AddTransient<ICustomValidator<Person>, PersonValidator>();
            services.AddTransient<ICustomValidator<Boat>, BoatValidator>();
            services.AddTransient<ICustomValidator<BoatOrganization>, BoatOrganizationValidator>();
            services.AddTransient<ICustomValidator<Championship>, ChampionshipValidator>();
            services.AddTransient<ICustomValidator<ChampionshipMember>, ChampionshipMemberValidator>();
            services.AddTransient<ICustomValidator<ChampionshipMemberInvitation>, ChampionshipMemberInvitationValidator>();
            services.AddTransient<ICustomValidator<ChampionshipNotification>, ChampionshipNotificationValidator>();
            services.AddTransient<ICustomValidator<ChampionshipGroup>, ChampionshipGroupValidator>();
            services.AddTransient<ICustomValidator<ChampionshipFile>, ChampionshipFileValidator>();
            services.AddTransient<ICustomValidator<ChampionshipFlag>, ChampionshipFlagValidator>();
            services.AddTransient<ICustomValidator<Organization>, OrganizationValidator>();
            services.AddTransient<ICustomValidator<OrganizationMember>, OrganizationMemberValidator>();
            services.AddTransient<ICustomValidator<OrganizationMemberInvitation>, OrganizationMemberInvitationValidator>();
            services.AddTransient<ICustomValidator<Race>, RaceValidator>();
            services.AddTransient<ICustomValidator<RaceProtest>, RaceProtestValidator>();
            services.AddTransient<ICustomValidator<CommitteeBoatReturn>, CommitteeBoatReturnValidator>();
            services.AddTransient<ICustomValidator<Team>, TeamValidator>();
            services.AddTransient<ICustomValidator<TeamMember>, TeamMemberValidator>();
            services.AddTransient<ICustomValidator<TeamMemberInvitation>, TeamMemberInvitationValidator>();
            services.AddTransient<ICustomValidator<TeamBoat>, TeamBoatValidator>();
            services.AddTransient<ICustomValidator<TeamMemberCheck>, TeamCheckValidator>();
            services.AddTransient<ICustomValidator<EquipmentChangeRequest>, EquipmentChangeRequestValidator>();
            services.AddTransient<ICustomValidator<CrewChangeRequest>, CrewChangeRequestValidator>();
            services.AddTransient<ICustomValidator<HearingRequest>, HearingRequestValidator>();
            #endregion

            #region Repositories
            services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();
            services.AddScoped<IAuthorizationRepository, AuthorizationRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserAccessRepository, UserAccessRepository>();
            services.AddScoped<IUserSettingsRepository, UserSettingsRepository>();
            services.AddScoped<IUserPasswordResetRepository, UserPasswordResetRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IDeviceRepository, DeviceRepository>();
            services.AddScoped<IDeviceSubscriptionRepository, DeviceSubscriptionRepository>();
            services.AddScoped<ITranslationRepository, FakeTranslationRepository>();
            services.AddScoped<ITimeZoneRepository, TimeZoneRepository>();
            services.AddScoped<ICityRepository, CityRepository>();
            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped<IChampionshipRepository, ChampionshipRepository>();
            services.AddScoped<IChampionshipMemberRepository, ChampionshipMemberRepository>();
            services.AddScoped<IChampionshipGroupRepository, ChampionshipGroupRepository>();
            services.AddScoped<IChampionshipFlagRepository, ChampionshipFlagRepository>();
            services.AddScoped<IChampionshipFileRepository, ChampionshipFileRepository>();
            services.AddScoped<IChampionshipNotificationRepository, ChampionshipNotificationRepository>();
            services.AddScoped<IFileRepository, FileRepository>();
            services.AddScoped<IFileTypeRepository, FileTypeRepository>();
            services.AddScoped<IOrganizationRepository, OrganizationRepository>();
            services.AddScoped<IOrganizationMemberRepository, OrganizationMemberRepository>();
            services.AddScoped<IBoatRepository, BoatRepository>();
            services.AddScoped<IBoatOrganizationRepository, BoatOrganizationRepository>();
            services.AddScoped<IRaceClassRepository, RaceClassRepository>();
            services.AddScoped<IRaceCategoryRepository, RaceCategoryRepository>();
            services.AddScoped<IRaceRepository, RaceRepository>();
            services.AddScoped<IRaceProtestRepository, RaceProtestRepository>();
            services.AddScoped<ICommitteeBoatReturnRepository, CommitteeBoatReturnRepository>();
            services.AddScoped<IPersonRepository, PersonRepository>();
            services.AddScoped<ITeamMemberRoleRepository, TeamMemberRoleRepository>();
            services.AddScoped<ITeamRepository, TeamRepository>();
            services.AddScoped<ITeamMemberRepository, TeamMemberRepository>();
            services.AddScoped<ITeamBoatRepository, TeamBoatRepository>();
            services.AddScoped<ITeamMemberCheckRepository, TeamMemberCheckRepository>();
            services.AddScoped<IBloodTypeRepository, BloodTypeRepository>();
            services.AddScoped<IMedicalInsuranceRepository, MedicalInsuranceRepository>();
            services.AddScoped<IFlagRepository, FlagRepository>();
            services.AddScoped<ILanguageRepository, LanguageRepository>();
            services.AddScoped<IFormatRepository, FormatRepository>();
            services.AddScoped<IEquipmentChangeRequestRepository, EquipmentChangeRequestRepository>();
            services.AddScoped<ICrewChangeRequestRepository, CrewChangeRequestRepository>();
            services.AddScoped<IHearingRequestRepository, HearingRequestRepository>();
            services.AddScoped<IHearingRequestTypeRepository, HearingRequestTypeRepository>();
            services.AddScoped<IHearingRequestStatusRepository, HearingRequestStatusRepository>();
            services.AddScoped<IMemberRepository, MemberRepository>();
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
            services.AddScoped<INotificationHelper, NotificationHelper>();
            #endregion

            #region Providers
            services.AddScoped<ITranslationProvider, TranslationProvider>();
            services.AddScoped<IFileStorageProvider, BlobFileStorageProvider>();
            services.AddScoped<IFileStorageProvider, DiskFileStorageProvider>();
            services.AddScoped<IEmailProvider, MailSmtpClientEmailProvider>();
            services.AddScoped<IPushNotificationProvider, GoogleFirebasePushNotificationProvider>();
            #endregion

            #region Factories & Strategies
            //services.AddScoped<INotificationStrategyFactory, EmailNotificationStrategyFactory>();
            //services.AddScoped<INotificationStrategyFactory, PushNotificationStrategyFactory>();
            services.AddScoped<INotificationStrategyFactory, NotificationStrategyFactory>();

            services.AddScoped<INotificationStrategy, EmailStrategies.UserCreationStrategy>();

            services.AddScoped<INotificationStrategy, EmailStrategies.OrganizationCreationStrategy>();
            services.AddScoped<INotificationStrategy, EmailStrategies.OrganizationMemberInvitationStrategy>();
            //services.AddScoped<INotificationStrategy, PushStrategies.OrganizationMemberInvitationStrategy>();

            services.AddScoped<INotificationStrategy, EmailStrategies.ChampionshipMemberInvitationStrategy>();
            services.AddScoped<INotificationStrategy, PushStrategies.ChampionshipMemberInvitationStrategy>();

            services.AddScoped<INotificationStrategy, EmailStrategies.ChampionshipFileUploadStrategy>();
            services.AddScoped<INotificationStrategy, PushStrategies.ChampionshipFileUploadStrategy>();

            services.AddScoped<INotificationStrategy, EmailStrategies.TeamCreationStrategy>();
            services.AddScoped<INotificationStrategy, EmailStrategies.TeamMemberInvitationStrategy>();
            //services.AddScoped<INotificationStrategy, PushStrategies.TeamMemberInvitationStrategy>();
            #endregion

            services.AddScoped<IContextResolver, ContextResolver>();
            services.AddScoped<ITranslator, Translator>();
        }
    }
}
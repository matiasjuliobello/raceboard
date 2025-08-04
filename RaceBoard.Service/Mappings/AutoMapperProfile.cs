using AutoMapper;
using RaceBoard.Translations.Entities;
using RaceBoard.Common;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.DTOs;
using RaceBoard.DTOs._Pagination.Request;
using RaceBoard.DTOs._Pagination.Response;
using RaceBoard.DTOs.Authentication.Response;
using RaceBoard.DTOs.File.Response;
using RaceBoard.DTOs.Language.Response;
using RaceBoard.DTOs.Password.Response;
using RaceBoard.DTOs.Permissions.Request;
using RaceBoard.DTOs.Permissions.Response;
using RaceBoard.DTOs.PrivacyPolicy.Response;
using RaceBoard.DTOs.Translation.Response;
using RaceBoard.DTOs.User.Request;
using RaceBoard.DTOs.User.Response;
using RaceBoard.DTOs.User.Response.Settings;
using RaceBoard.DTOs.Championship.Request;
using RaceBoard.DTOs.Organization.Request;
using RaceBoard.DTOs.Boat.Request;
using RaceBoard.DTOs.RaceClass.Request;
using RaceBoard.DTOs.Race.Request;
using RaceBoard.DTOs.TeamMemberRole.Request;
using RaceBoard.DTOs.Person.Request;
using RaceBoard.DTOs.BloodType.Request;
using RaceBoard.DTOs.MedicalInsurance.Request;
using RaceBoard.DTOs.BloodType.Response;
using RaceBoard.DTOs.MedicalInsurance.Response;
using RaceBoard.DTOs.Person.Response;
using RaceBoard.DTOs.Country.Response;
using RaceBoard.DTOs.Boat.Response;
using RaceBoard.DTOs.Championship.Response;
using RaceBoard.DTOs.City.Response;
using RaceBoard.DTOs.Organization.Response;
using RaceBoard.DTOs.TeamMemberRole.Response;
using RaceBoard.DTOs.Flag.Request;
using RaceBoard.DTOs.Flag.Response;
using RaceBoard.DTOs.RaceCategory.Request;
using RaceBoard.DTOs.RaceCategory.Response;
using RaceBoard.DTOs.RaceClass.Response;
using RaceBoard.DTOs.Race.Response;
using RaceBoard.DTOs.Team.Request;
using RaceBoard.DTOs.Team.Response;
using RaceBoard.DTOs.City.Request;
using RaceBoard.DTOs.Country.Request;
using RaceBoard.DTOs.Format.Response;
using RaceBoard.DTOs.Device.Request;
using RaceBoard.DTOs.Device.Response;
using RaceBoard.DTOs.Invitation.Response;
using RaceBoard.DTOs.ChangeRequest.Request;
using RaceBoard.DTOs.ChangeRequest.Response;
using RaceBoard.DTOs.HearingRequest.Request;
using RaceBoard.DTOs.HearingRequest.Response;
using RaceBoard.Domain;
using File = RaceBoard.Domain.File;
using TimeZone = RaceBoard.Domain.TimeZone;
using Action = RaceBoard.Domain.Action;
using Enums = RaceBoard.Domain.Enums;
using RaceBoard.DTOs.Gender.Response;
using RaceBoard.DTOs.Coach.Request;
using RaceBoard.DTOs.Coach.Response;


namespace RaceBoard.Service.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            #region Requests

            CreateMap<DateTimeRangeRequest, DateTimeRange>()
                .ForMember(dest => dest.Start, opt => opt.MapFrom(src => src.Start))
                .ForMember(dest => dest.End, opt => opt.MapFrom(src => src.End));

            CreateMap<IFormFile, FileUpload>()
                //.ForMember(dest => dest.Tenant, opt => opt.MapFrom(src => src.Headers[_TenantHeader]))
                .ForMember(dest => dest.ContentType, opt => opt.MapFrom(src => src.ContentType))
                .ForMember(dest => dest.Filename, opt => opt.MapFrom(src => src.FileName))
                .ForMember(dest => dest.UniqueFilename, opt => opt.MapFrom(src => CreateUniqueFilename(src)))
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => GetContent(src)));

            CreateMap<SortingRequest, Sorting>().ConvertUsing(typeof(SortingRequestToSortingConverter<SortingRequest, Sorting>));
            CreateMap<PaginationFilterRequest, PaginationFilter>();

            CreateMap<UserLoginRequest, UserLogin>();

            CreateMap<UserRequest, User>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(x => x.UserRole, opt => opt.MapFrom(x => new UserRole() { Role = new Role() { Id = x.IdRole } }));

            CreateMap<UserSearchFilterRequest, UserSearchFilter>();

            CreateMap<UserSettingsRequest, UserSettings>()
                .ForMember(dest => dest.Language, opt => opt.MapFrom(src => CreateObject<Domain.Language>(src.IdLanguage)))
                .ForMember(dest => dest.TimeZone, opt => opt.MapFrom(src => CreateObject<Domain.TimeZone>(src.IdTimeZone)))
                .ForMember(dest => dest.DateFormat, opt => opt.MapFrom(src => CreateObject<Domain.DateFormat>(src.IdDateFormat)));

            CreateMap<UserIdentificationRequest, UserIdentification>()
                .ForMember(x => x.User, opt => opt.MapFrom(x => CreateObject<User>(x.IdUser)))
                .ForMember(x => x.Type, opt => opt.MapFrom(x => CreateObject<IdentificationType>(x.IdType)));

            CreateMap<UserIdentificationSearchFilterRequest, UserIdentificationSearchFilter>()
                .ForMember(x => x.User, opt => opt.MapFrom(x => CreateObject<User>(x.IdUser)))
                .ForMember(x => x.Type, opt => opt.MapFrom(x => CreateObject<IdentificationType>(x.IdType)));

            CreateMap<PrivacyPolicyAgreementRequest, PrivacyPolicyAgreement>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.AgreementDate, opt => opt.Ignore())
                .ForMember(dest => dest.PrivacyPolicy, opt => opt.MapFrom(src => CreateObject<PrivacyPolicy>(src.IdPrivacyPolicy)));

            CreateMap<int, Role>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src));

            CreateMap<ActionSearchFilterRequest, ActionSearchFilter>();

            CreateMap<ActionRoleRequest, ActionRole>()
                .ForMember(dest => dest.Action, opt => opt.MapFrom(src => CreateObject<Action>(src.IdAction)))
                .ForMember(dest => dest.Condition, opt => opt.MapFrom(src => CreateObject<AuthorizationCondition>(src.IdCondition)));

            CreateMap<RolePermissionsRequest, RolePermissions>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => CreateObject<Role>(src.IdRole)))
                .ForMember(dest => dest.Permissions, opt => opt.MapFrom(src => src.Permissions));

            CreateMap<CountrySearchFilterRequest, CountrySearchFilter>();
            CreateMap<CitySearchFilterRequest, CitySearchFilter>()
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => CreateObject<Country>(src.IdCountry)));

            CreateMap<ChampionshipRequest, Championship>()
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => CreateObject<City>(src.IdCity)))
                .ForMember(t => t.Organizations, opt =>
                {
                    opt.PreCondition(s => s.IdsOrganization?.Length > 0);
                    opt.MapFrom(s => s.IdsOrganization.ToList());
                });

            CreateMap<ChampionshipSearchFilterRequest, ChampionshipSearchFilter>()
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => CreateObject<City>(src.IdCity)));
            CreateMap<ChampionshipMemberInvitationRequest, ChampionshipMemberInvitation>()
                .ForMember(dest => dest.Championship, opt => opt.MapFrom(src => CreateObject<Championship>(src.IdChampionship)))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => CreateObject<Role>(src.IdRole)))
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => CreateObject<User>(src.IdUser)));

            CreateMap<ChampionshipGroupRequest, ChampionshipGroup>()
                .ForMember(dest => dest.Championship, opt => opt.MapFrom(src => CreateObject<Championship>(src.IdChampionship)));

            CreateMap<ChampionshipSearchFilterRequest, ChampionshipSearchFilter>()
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => CreateObject<City>(src.IdCity)))
                .ForMember(dest => dest.Organizations, opt => opt.MapFrom(src => CreateObject<Organization>(src.IdsOrganization)));

            CreateMap<ChampionshipNotificationRequest, ChampionshipNotification>()
                .ForMember(dest => dest.Championship, opt => opt.MapFrom(src => CreateObject<Championship>(src.IdChampionship)));

            CreateMap<ChampionshipNotificationSearchFilterRequest, ChampionshipNotificationSearchFilter>()
                .ForMember(dest => dest.Championship, opt => opt.MapFrom(src => CreateObject<Championship>(src.IdChampionship)));

            CreateMap<InvitationRequest, Invitation>();

            CreateMap<int, Organization>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src));
            CreateMap<OrganizationRequest, Organization>()
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => CreateObject<City>(src.IdCity)));
            CreateMap<OrganizationSearchFilterRequest, OrganizationSearchFilter>()
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => CreateObject<City>(src.IdCity)));
            CreateMap<OrganizationMemberInvitationRequest, OrganizationMemberInvitation>()
                .ForMember(dest => dest.Organization, opt => opt.MapFrom(src => CreateObject<Organization>(src.IdOrganization)))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => CreateObject<Role>(src.IdRole)))
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => CreateObject<User>(src.IdUser)));


            CreateMap<BoatRequest, Boat>()
                .ForMember(dest => dest.RaceClass, opt => opt.MapFrom(src => CreateObject<RaceClass>(src.IdRaceClass)))
                .ForMember(dest => dest.Owners, opt => opt.MapFrom(src => CreateObject<BoatOwner>(src.IdsOwner)));            
            CreateMap<BoatSearchFilterRequest, BoatSearchFilter>()
                .ForMember(dest => dest.RaceCategory, opt => opt.MapFrom(src => CreateObject<RaceCategory>(src.IdRaceCategory)))
                .ForMember(dest => dest.RaceClass, opt => opt.MapFrom(src => CreateObject<RaceClass>(src.IdRaceClass)));

            CreateMap<BoatOrganizationRequest, BoatOrganization>()
                .ForMember(dest => dest.Boat, opt => opt.MapFrom(src => CreateObject<Boat>(src.IdBoat)))
                .ForMember(dest => dest.Organization, opt => opt.MapFrom(src => CreateObject<Organization>(src.IdOrganization)));
            CreateMap<BoatOrganizationSearchFilterRequest, BoatOrganizationSearchFilter>()
                .ForMember(dest => dest.Boat, opt => opt.MapFrom(src => CreateObject<Boat>(src.IdBoat)))
                .ForMember(dest => dest.Organization, opt => opt.MapFrom(src => CreateObject<Organization>(src.IdOrganization)));

            CreateMap<BoatOwnerRequest, BoatOwner>()
                .ForMember(dest => dest.Boat, opt => opt.MapFrom(src => CreateObject<Boat>(src.IdBoat)))
                .ForMember(dest => dest.Person, opt => opt.MapFrom(src => CreateObject<Person>(src.IdPerson)));
            CreateMap<BoatOwnerSearchFilterRequest, BoatOwnerSearchFilter>()
                .ForMember(dest => dest.Boat, opt => opt.MapFrom(src => CreateObject<Boat>(src.IdBoat)))
                .ForMember(dest => dest.Person, opt => opt.MapFrom(src => CreateObject<Person>(src.IdPerson)));


            CreateMap<RaceCategoryRequest, RaceCategory>();
            CreateMap<RaceCategorySearchFilterRequest, RaceCategorySearchFilter>();

            CreateMap<RaceClassRequest, RaceClass>()
                .ForMember(dest => dest.RaceCategory, opt => opt.MapFrom(src => CreateObject<RaceCategory>(src.IdRaceCategory)));
            CreateMap<RaceClassSearchFilterRequest, RaceClassSearchFilter>();

            CreateMap<RaceRequest, Race>()
                .ForMember(dest => dest.Championship, opt => opt.MapFrom(src => CreateObject<Championship>(src.IdChampionship)))
                .ForMember(dest => dest.RaceClasses, opt => opt.MapFrom(src => CreateObject<RaceClass>(src.IdsRaceClass)));
            CreateMap<RaceSearchFilterRequest, RaceSearchFilter>()
                .ForMember(dest => dest.Championship, opt => opt.MapFrom(src => CreateObject<Championship>(src.IdChampionship)))
                .ForMember(dest => dest.RaceClass, opt => opt.MapFrom(src => CreateObject<RaceClass>(src.IdRaceClass)));

            CreateMap<RaceProtestRequest, RaceProtest>()
                .ForMember(dest => dest.Race, opt => opt.MapFrom(src => CreateObject<Race>(src.IdRace)))
                .ForMember(dest => dest.TeamMember, opt => opt.MapFrom(src => CreateObject<TeamMember>(src.IdTeamMember)));

            CreateMap<ChampionshipCommitteeBoatReturnRequest, ChampionshipCommitteeBoatReturn>()
                .ForMember(dest => dest.Championship, opt => opt.MapFrom(src => CreateObject<Championship>(src.IdChampionship)))
                .ForMember(dest => dest.RaceClasses, opt => opt.MapFrom(src => CreateObject<RaceClass>(src.IdsRaceClass)));

            CreateMap<ChampionshipCommitteeBoatReturnSearchFilterRequest, ChampionshipCommitteeBoatReturnSearchFilter>()
                .ForMember(dest => dest.Championship, opt => opt.MapFrom(src => CreateObject<Championship>(src.IdChampionship)))
                .ForMember(dest => dest.RaceClasses, opt => opt.MapFrom(src => CreateObject<RaceClass>(src.IdsRaceClass)));

            CreateMap<ChampionshipFileRequest, ChampionshipFile>()
                .ForMember(dest => dest.FileType, opt => opt.MapFrom(src => CreateObject<FileType>(src.IdFileType)))
                .ForMember(dest => dest.Championship, opt => opt.MapFrom(src => CreateObject<Championship>(src.IdChampionship)));
            //.ForMember(dest => dest.RaceClasses, opt => opt.MapFrom(src => CreateObject<RaceClass>(src.IdsRaceClass)));

            CreateMap<ChampionshipFileSearchFilterRequest, ChampionshipFileSearchFilter>()
                .ForMember(dest => dest.Championship, opt => opt.MapFrom(src => CreateObject<Championship>(src.IdChampionship)))
                .ForMember(dest => dest.RaceClasses, opt => opt.MapFrom(src => CreateObject<RaceClass>(src.IdsRaceClass)))
                .ForMember(dest => dest.FileType, opt => opt.MapFrom(src => CreateObject<FileType>(src.IdFileType)));

            CreateMap<ChampionshipNotificationRequest, ChampionshipNotification>()
                .ForMember(dest => dest.Championship, opt => opt.MapFrom(src => CreateObject<Championship>(src.IdChampionship)));

            CreateMap<ChampionshipNotificationSearchFilterRequest, ChampionshipNotificationSearchFilter>()
                .ForMember(dest => dest.Championship, opt => opt.MapFrom(src => CreateObject<Championship>(src.IdChampionship)))
                .ForMember(dest => dest.RaceClasses, opt => opt.MapFrom(src => CreateObject<RaceClass>(src.IdsRaceClass)));

            CreateMap<BloodTypeSearchFilterRequest, BloodTypeSearchFilter>();

            CreateMap<MedicalInsuranceSearchFilterRequest, MedicalInsuranceSearchFilter>();

            CreateMap<PersonRequest, Person>()
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => CreateObject<Gender>(src.IdGender)))
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => CreateObject<User>(src.IdUser)))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => CreateObject<Country>(src.IdCountry)))
                .ForMember(dest => dest.BloodType, opt => opt.MapFrom(src => CreateObject<BloodType>(src.IdBloodType)))
                .ForMember(dest => dest.MedicalInsurance, opt => opt.MapFrom(src => CreateObject<MedicalInsurance>(src.IdMedicalInsurance)));

            CreateMap<PersonSearchFilterRequest, PersonSearchFilter>();

            CreateMap<TeamMemberRoleSearchFilterRequest, TeamMemberRoleSearchFilter>();

            CreateMap<TeamRequest, Team>()
                .ForMember(dest => dest.Championship, opt => opt.MapFrom(src => CreateObject<Championship>(src.IdChampionship)))
                .ForMember(dest => dest.Organization, opt => opt.MapFrom(src => CreateObject<Organization>(src.IdOrganization)))
                .ForMember(dest => dest.RaceClass, opt => opt.MapFrom(src => CreateObject<RaceClass>(src.IdRaceClass)));
            CreateMap<TeamSearchFilterRequest, TeamSearchFilter>()
                .ForMember(dest => dest.Championship, opt => opt.MapFrom(src => CreateObject<Championship>(src.IdChampionship)))
                .ForMember(dest => dest.RaceClass, opt => opt.MapFrom(src => CreateObject<RaceClass>(src.IdRaceClass)));

            CreateMap<TeamMemberSearchFilterRequest, TeamMemberSearchFilter>()
                .ForMember(dest => dest.Team, opt => opt.MapFrom(src => CreateObject<Team>(src.IdTeam)))
                .ForMember(dest => dest.Member, opt => opt.MapFrom(src => CreateObject<Person>(src.IdTeamMember)))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => CreateObject<TeamMemberRole>(src.IdTeamMemberRole)));
            CreateMap<TeamMemberInvitationRequest, TeamMemberInvitation>()
                .ForMember(dest => dest.Team, opt => opt.MapFrom(src => CreateObject<Team>(src.IdTeam)))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => CreateObject<TeamMemberRole>(src.IdRole)))
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => CreateObject<User>(src.IdUser)));

            CreateMap<TeamBoatRequest, TeamBoat>()
                .ForMember(dest => dest.Team, opt => opt.MapFrom(src => CreateObject<Team>(src.IdTeam)))
                .ForMember(dest => dest.Boat, opt => opt.MapFrom(src => CreateObject<Boat>(src.IdBoat)));
            CreateMap<TeamBoatSearchFilterRequest, TeamBoatSearchFilter>()
                .ForMember(dest => dest.Team, opt => opt.MapFrom(src => CreateObject<Team>(src.IdTeam)))
                .ForMember(dest => dest.RaceClass, opt => opt.MapFrom(src => CreateObject<RaceClass>(src.IdRaceClass)))
                .ForMember(dest => dest.Championship, opt => opt.MapFrom(src => CreateObject<Championship>(src.IdChampionship)))
                .ForMember(dest => dest.Boat, opt => opt.MapFrom(src => CreateBoat(src)));

            CreateMap<TeamCheckRequest, TeamMemberCheck>()
                .ForMember(dest => dest.Championship, opt => opt.MapFrom(src => CreateObject<Championship>(src.IdChampionship)))
                .ForMember(dest => dest.CheckType, opt => opt.MapFrom(src => CreateEnum<Enums.TeamMemberCheckType>(src.IdCheckType)));

            CreateMap<TeamCheckSearchFilterRequest, TeamCheckSearchFilter>()
                .ForMember(dest => dest.Team, opt => opt.MapFrom(src => CreateObject<Team>(src.IdTeam)))
                .ForMember(dest => dest.CheckType, opt => opt.MapFrom(src => CreateEnum<Enums.TeamMemberCheckType>(src.IdCheckType)))
                .ForMember(dest => dest.Championship, opt => opt.MapFrom(src => CreateObject<Championship>(src.IdChampionship)))
                .ForMember(dest => dest.RaceClasses, opt => opt.MapFrom(src => CreateObject<RaceClass>(src.IdsRaceClass)));

            CreateMap<FlagSearchFilterRequest, FlagSearchFilter>();

            CreateMap<ChampionshipFlagGroupRequest, ChampionshipFlagGroup>()
                .ForMember(dest => dest.Championship, opt => opt.MapFrom(src => CreateObject<Championship>(src.IdChampionship)));

            CreateMap<ChampionshipFlagRequest, ChampionshipFlag>()
                .ForMember(dest => dest.Flag, opt => opt.MapFrom(src => CreateObject<Flag>(src.IdFlag)));

            CreateMap<ChampionshipFlagSearchFilterRequest, ChampionshipFlagSearchFilter>()
                .ForMember(dest => dest.Flag, opt => opt.MapFrom(src => CreateObject<Flag>(src.IdFlag)))
                .ForMember(dest => dest.Person, opt => opt.MapFrom(src => CreateObject<Person>(src.IdPerson)));

            //CreateMap<NotificationRequest, PushNotification>()
            //    .ForMember(dest => dest.NotificationType, opt => opt.MapFrom(src => (PushNotificationType)src.IdNotificationType));

            CreateMap<DeviceRequest, Device>()
                .ForMember(dest => dest.Platform, opt => opt.MapFrom(src => CreateObject<Platform>(src.IdPlatform)));

            CreateMap<DeviceSubscriptionRequest, DeviceSubscription>()
                .ForMember(dest => dest.Device, opt => opt.MapFrom(src => CreateObject<Device>(src.IdDevice)))
                .ForMember(dest => dest.Championship, opt => opt.MapFrom(src => CreateObject<Championship>(src.IdChampionship)))
                .ForMember(dest => dest.RaceClasses, opt => opt.MapFrom(src => CreateObject<RaceClass>(src.IdsRaceClass)));

            CreateMap<ChangeRequestRequest, ChangeRequest>()
                //.ForMember(dest => dest.Status, opt => opt.MapFrom(src => (Enums.RequestStatus)src.IdRequestStatus))
                //.ForMember(dest => dest.RequestUser, opt => opt.MapFrom(src => CreateObject<User>(src.IdRequestUser)))
                .ForMember(dest => dest.Team, opt => opt.MapFrom(src => CreateObject<Team>(src.IdTeam)));

            CreateMap<EquipmentChangeRequestRequest, EquipmentChangeRequest>()
                .IncludeBase<ChangeRequestRequest, ChangeRequest>();

            CreateMap<CrewChangeRequestRequest, CrewChangeRequest>()
                .IncludeBase<ChangeRequestRequest, ChangeRequest>()
                .ForMember(dest => dest.ReplacedUser, opt => opt.MapFrom(src => CreateObject<User>(src.IdReplacedUser)));

            CreateMap<ChangeRequestSearchFilterRequest, ChangeRequestSearchFilter>()
                .ForMember(dest => dest.Team, opt => opt.MapFrom(src => CreateObject<Team>(src.IdTeam)));

            CreateMap<HearingRequestSearchFilterRequest, HearingRequestSearchFilter>()
                .ForMember(dest => dest.Championship, opt => opt.MapFrom(src => CreateObject<Championship>(src.IdChampionship)))
                .ForMember(dest => dest.Team, opt => opt.MapFrom(src => CreateObject<Team>(src.IdTeam)))
                .ForMember(dest => dest.RequestUser, opt => opt.MapFrom(src => CreateObject<User>(src.IdRequestUser)));
            CreateMap<HearingRequestRequest, HearingRequest>()
                .ForMember(dest => dest.Team, opt => opt.MapFrom(src => CreateObject<Team>(src.IdTeam)))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => CreateObject<HearingRequestStatus>(src.IdHearingRequestStatus)))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => CreateObject<HearingRequestType>(src.IdHearingRequestType)));
            CreateMap<HearingRequestProtestorRequest, HearingRequestProtestor>();
            CreateMap<HearingRequestProtestorNoticeRequest, HearingRequestProtestorNotice>();
            CreateMap<HearingRequestProtesteesRequest, HearingRequestProtestees>();
            CreateMap<HearingRequestProtesteeRequest, HearingRequestProtestee>();
            CreateMap<HearingRequestIncidentRequest, HearingRequestIncident>();
            CreateMap<HearingRequestWithdrawalRequest, HearingRequestWithdrawal>();
            CreateMap<HearingRequestLodgementRequest, HearingRequestLodgement>()
                .ForMember(t => t.Deadline, opt =>
                {
                    opt.PreCondition(s => s.Deadline?.Trim().Length > 0);
                    opt.MapFrom(s => TimeSpan.Parse(s.Deadline));
                });
            CreateMap<HearingRequestAttendeesRequest, HearingRequestAttendees>();
            CreateMap<HearingRequestValidityRequest, HearingRequestValidity>();
            CreateMap<HearingRequestResolutionRequest, HearingRequestResolution>();

            CreateMap<CoachRequest, Coach>()
                .ForMember(dest => dest.Person, opt => opt.MapFrom(src => CreateObject<Person>(src.IdPerson)));
            CreateMap<CoachSearchFilterRequest, CoachSearchFilter>()
                .ForMember(dest => dest.Person, opt => opt.MapFrom(src => CreateObject<Person>(src.IdPerson)));

            CreateMap<CoachOrganizationRequest, CoachOrganization>()
                .ForMember(dest => dest.Coach, opt => opt.MapFrom(src => CreateObject<Coach>(src.IdCoach)))
                .ForMember(dest => dest.Organization, opt => opt.MapFrom(src => CreateObject<Organization>(src.IdOrganization)));
            CreateMap<CoachOrganizationSearchFilterRequest, CoachOrganizationSearchFilter>()
                .ForMember(dest => dest.Coach, opt => opt.MapFrom(src => CreateObject<Coach>(src.IdCoach)))
                .ForMember(dest => dest.Organization, opt => opt.MapFrom(src => CreateObject<Organization>(src.IdOrganization)));

            CreateMap<CoachTeamRequest, CoachTeam>()
                .ForMember(dest => dest.Coach, opt => opt.MapFrom(src => CreateObject<Coach>(src.IdCoach)))
                .ForMember(dest => dest.Team, opt => opt.MapFrom(src => CreateObject<Team>(src.IdTeam)));
            CreateMap<CoachTeamSearchFilterRequest, CoachTeamSearchFilter>()
                .ForMember(dest => dest.Coach, opt => opt.MapFrom(src => CreateObject<Coach>(src.IdCoach)))
                .ForMember(dest => dest.Team, opt => opt.MapFrom(src => CreateObject<Team>(src.IdTeam)));


            #endregion

            #region Responses

            CreateMap<DateTimeRange, DateTimeRangeResponse>();

            CreateMap<Translation, TranslationResponse>().ConvertUsing(typeof(TranslationToTranslationResponseConverter<Translation, TranslationResponse>));

            CreateMap(typeof(PaginatedResult<>), typeof(PaginatedResultResponse<>));

            CreateMap<AccessToken, AccessTokenResponse>();

            CreateMap<TimeZone, TimeZoneResponse>();
            CreateMap<Domain.Language, LanguageResponse>();
            CreateMap<DateFormat, DateFormatResponse>();
            CreateMap<UserSettings, UserSettingsResponse>();

            CreateMap<PrivacyPolicy, PrivacyPolicyResponse>();
            CreateMap<PrivacyPolicyAgreement, PrivacyPolicyAgreementResponse>();

            CreateMap<PasswordPolicy, PasswordPolicyResponse>();

            CreateMap<Role, RoleResponse>();

            CreateMap<User, UserResponse>();
            CreateMap<User, UserSimpleResponse>();
            CreateMap<UserRole, UserRoleResponse>();
            
            CreateMap<Gender, GenderResponse>();

            CreateMap<File, FileResponse>();
            CreateMap<FileType, FileTypeResponse>();

            CreateMap<UserIdentification, UserIdentificationResponse>();

            CreateMap<Action, ActionResponse>();
            CreateMap<AuthorizationCondition, AuthorizationConditionResponse>();
            CreateMap<ActionRole, ActionRoleResponse>()
                .ForMember(dest => dest.IdAction, opt => opt.MapFrom(src => src.Action.Id))
                .ForMember(dest => dest.IdCondition, opt => opt.MapFrom(src => src.Condition.Id));
            CreateMap<RolePermissions, RolePermissionsResponse>()
                .ForMember(dest => dest.IdRole, opt => opt.MapFrom(src => src.Role.Id))
                .ForMember(dest => dest.Permissions, opt => opt.MapFrom(src => src.Permissions));

            CreateMap<Invitation, InvitationResponse>();

            CreateMap<Organization, OrganizationResponse>();
            CreateMap<OrganizationMember, OrganizationMemberResponse>();
            CreateMap<OrganizationMemberInvitation, OrganizationMemberInvitationResponse>();

            CreateMap<BloodType, BloodTypeResponse>();

            CreateMap<MedicalInsurance, MedicalInsuranceResponse>();

            CreateMap<Country, CountryResponse>();
            CreateMap<City, CityResponse>();

            CreateMap<Person, PersonResponse>();
            CreateMap<Person, PersonSimpleResponse>();

            CreateMap<Boat, BoatResponse>();
            CreateMap<BoatOrganization, BoatOrganizationResponse>();
            CreateMap<BoatOwner, BoatOwnerResponse>();

            CreateMap<Championship, ChampionshipResponse>();
            CreateMap<Championship, ChampionshipSimpleResponse>();
            CreateMap<ChampionshipGroup, ChampionshipGroupResponse>();
            CreateMap<ChampionshipNotification, ChampionshipNotificationResponse>();
            CreateMap<ChampionshipCommitteeBoatReturn, ChampionshipCommitteeBoatReturnResponse>();
            CreateMap<ChampionshipFile, ChampionshipFileResponse>();
            //CreateMap<ChampionshipRaceClass, ChampionshipRaceClassResponse>();
            CreateMap<ChampionshipMember, ChampionshipMemberResponse>();
            CreateMap<ChampionshipMemberInvitation, ChampionshipMemberInvitationResponse>();


            CreateMap<TeamMemberRole, TeamMemberRoleResponse>();

            CreateMap<Flag, FlagResponse>();
            CreateMap<ChampionshipFlag, ChampionshipFlagResponse>();
            CreateMap<ChampionshipFlagGroup, ChampionshipFlagGroupResponse>();

            CreateMap<RaceCategory, RaceCategoryResponse>();
            CreateMap<RaceClass, RaceClassResponse>();
            CreateMap<Race, RaceResponse>();

            CreateMap<Team, TeamResponse>();
            CreateMap<Team, TeamSimpleResponse>();
            CreateMap<TeamMember, TeamMemberResponse>();
            CreateMap<TeamMemberInvitation, TeamMemberInvitationResponse>();
            CreateMap<TeamMemberCheck, TeamMemberCheckResponse>();
            CreateMap<TeamBoat, TeamBoatResponse>();
            CreateMap<DeviceSubscription, DeviceSubscriptionResponse>();

            CreateMap<ChangeRequestStatus, RequestStatusResponse>();
            CreateMap<ChangeRequest, ChangeRequestResponse>();
            CreateMap<CrewChangeRequest, CrewChangeRequestResponse>();
            CreateMap<EquipmentChangeRequest, EquipmentChangeRequestResponse>();

            CreateMap<HearingRequest, HearingRequestResponse>();
            CreateMap<HearingRequestStatus, HearingRequestStatusResponse>();
            CreateMap<HearingRequestType, HearingRequestTypeResponse>();
            CreateMap<HearingRequestProtestor, HearingRequestProtestorResponse>();
            CreateMap<HearingRequestProtestorNotice, HearingRequestProtestorNoticeResponse>();
            CreateMap<HearingRequestProtestees, HearingRequestProtesteesResponse>();
            CreateMap<HearingRequestProtestee, HearingRequestProtesteeResponse>();
            CreateMap<HearingRequestIncident, HearingRequestIncidentResponse>();
            CreateMap<HearingRequestWithdrawal, HearingRequestWithdrawalResponse>();
            CreateMap<HearingRequestLodgement, HearingRequestLodgementResponse>();
            CreateMap<HearingRequestAttendees, HearingRequestAttendeesResponse>();
            CreateMap<HearingRequestValidity, HearingRequestValidityResponse>();
            CreateMap<HearingRequestResolution, HearingRequestResolutionResponse>();

            CreateMap<Coach, CoachResponse>();
            CreateMap<CoachOrganization, CoachOrganizationResponse>();
            CreateMap<CoachTeam, CoachTeamResponse>();

            #endregion
        }

        #region Custom Converters

        public class SortingRequestToSortingConverter<TValue, TVersion> : ITypeConverter<SortingRequest, Sorting>
        {
            public Sorting Convert(SortingRequest source, Sorting destination, ResolutionContext context)
            {
                var sorting = new Sorting()
                {
                    OrderByClauses = new List<OrderByClause>()
                };

                if (source == null || source.OrderBy == null || source.OrderBy.Length == 0)
                    return sorting;

                string[] orderByClauses = source.OrderBy.Trim().Split(",", StringSplitOptions.RemoveEmptyEntries);

                foreach (string orderByClause in orderByClauses)
                {
                    string orderBy = orderByClause.TrimStart().TrimEnd();

                    int pos = orderBy.IndexOf(" ");
                    string columnName = orderBy.Substring(0, pos);
                    var direction = orderBy.Remove(0, pos + 1).ToLower() == "desc" ? OrderByDirection.Descending : OrderByDirection.Ascending;

                    sorting.OrderByClauses.Add(new OrderByClause(columnName, direction));
                }

                return sorting;
            }
        }

        public class TranslationToTranslationResponseConverter<TValue, TVersion> : ITypeConverter<Translation, TranslationResponse>
        {
            public TranslationResponse Convert(Translation source, TranslationResponse destination, ResolutionContext context)
            {
                var translation = new TranslationResponse();

                if (source == null)
                    return translation;

                translation.Key = source.Key;
                translation.Value = source.Translations.Count > 0 ? source.Translations.FirstOrDefault().Text : "";

                return translation;
            }
        }

        #endregion

        #region Private Methods

        private Boat CreateBoat(TeamBoatSearchFilterRequest searchFilterRequest)
        {
            var boat = new Boat();

            if (searchFilterRequest.IdBoat.HasValue)
                boat.Id = searchFilterRequest.IdBoat.Value;
            
            if (!String.IsNullOrEmpty(searchFilterRequest.BoatName))
                boat.Name = searchFilterRequest.BoatName;
            
            if (!String.IsNullOrEmpty(searchFilterRequest.BoatSailNumber))
                boat.SailNumber = searchFilterRequest.BoatSailNumber;

            if (!String.IsNullOrEmpty(searchFilterRequest.BoatHullNumber))
                boat.HullNumber = searchFilterRequest.BoatHullNumber;

            return boat;
        }

        private T? CreateObject<T>(int? id) where T : class
        {
            if (!id.HasValue)
                return null;

            string typeFullName = typeof(T).AssemblyQualifiedName;

            Type type = Type.GetType(typeFullName);

            dynamic instance = Activator.CreateInstance(type);

            if (instance == null)
                return null;

            if (id.HasValue)
                instance.Id = id.Value;

            return (T)instance;
        }

        private List<T>? CreateObject<T>(int[]? ids) where T : class
        {
            if (ids == null || ids.Length == 0)
                return null;

            string typeFullName = typeof(T).AssemblyQualifiedName;

            Type type = Type.GetType(typeFullName);

            var instances = new List<T>();

            foreach (var id in ids)
            {
                dynamic instance = Activator.CreateInstance(type);
                if (instance == null)
                    return null;

                instance.Id = id;

                instances.Add(instance);
            }

            return instances;
        }

        private T? CreateEnum<T>(int? id) where T : struct, Enum
        {
            if (!id.HasValue)
                return null;

            T result = default(T);

            Enum.TryParse<T>(id.Value.ToString(), out result);

            return result;
        }

        private byte[] GetContent(IFormFile src)
        {
            if (src.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    src.CopyTo(ms);

                    return ms.ToArray();
                }
            }

            return new byte[0];
        }

        private string CreateUniqueFilename(IFormFile src)
        {
            var uniqueId = IdGenerator.BuildUniqueId();
            var extension = Path.GetExtension(src.FileName);

            return $"{uniqueId}{extension}";
        }

        #endregion
    }
}

using AutoMapper;
//using static RaceBoard.Service.Mappings.AutoMapperProfile;
using RaceBoard.Common;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Domain;
using RaceBoard.Domain.Upload;
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
using RaceBoard.Translations.Entities;
using RaceBoard.DTOs.Competition.Request;
using RaceBoard.DTOs.Organization.Request;
using RaceBoard.DTOs.Boat.Request;
using RaceBoard.DTOs.RaceClass.Request;
using RaceBoard.DTOs.Race.Request;
using RaceBoard.DTOs.Contestant.Request;
using RaceBoard.DTOs.ContestantRole.Request;
using RaceBoard.DTOs.Person.Request;
using RaceBoard.DTOs.BloodType.Request;
using RaceBoard.DTOs.MedicalInsurance.Request;
using RaceBoard.DTOs.BloodType.Response;
using RaceBoard.DTOs.MedicalInsurance.Response;
using RaceBoard.DTOs.Person.Response;
using RaceBoard.DTOs.Country.Response;
using RaceBoard.DTOs.Boat.Response;
using File = RaceBoard.Domain.File;
using TimeZone = RaceBoard.Domain.TimeZone;
using Action = RaceBoard.Domain.Action;
using RaceBoard.DTOs.Competition.Response;
using RaceBoard.DTOs.City.Response;
using RaceBoard.DTOs.Organization.Response;
using RaceBoard.DTOs.Contestant.Response;
using RaceBoard.DTOs.ContestantRole.Response;
using RaceBoard.DTOs.Flag.Request;
using RaceBoard.DTOs.Flag.Response;
using RaceBoard.DTOs.Mast.Request;
using RaceBoard.DTOs.Mast.Response;
using RaceBoard.DTOs.RaceCategory.Request;
using RaceBoard.DTOs.RaceCategory.Response;
using RaceBoard.DTOs.RaceClass.Response;
using RaceBoard.DTOs.Race.Response;
using RaceBoard.DTOs.Team.Request;
using RaceBoard.DTOs.Team.Response;

namespace RaceBoard.Service.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            #region Requests

            CreateMap<IFormFile, UploadedFile>()
                //.ForMember(dest => dest.Tenant, opt => opt.MapFrom(src => src.Headers[_TenantHeader]))
                .ForMember(dest => dest.ContentType, opt => opt.MapFrom(src => src.ContentType))
                .ForMember(dest => dest.Filename, opt => opt.MapFrom(src => src.FileName))
                .ForMember(dest => dest.UniqueFilename, opt => opt.MapFrom(src => CreateUniqueFilename(src)))
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => GetContent(src)));

            CreateMap<SortingRequest, Sorting>().ConvertUsing(typeof(SortingRequestToSortingConverter<SortingRequest, Sorting>));
            CreateMap<PaginationFilterRequest, PaginationFilter>();

            CreateMap<UserLoginRequest, UserLogin>();

            CreateMap<UserRequest, User>();

            CreateMap<UserSearchFilterRequest, UserSearchFilter>();

            CreateMap<UserSettingsRequest, UserSettings>()
                .ForMember(dest => dest.Culture, opt => opt.MapFrom(src => CreateObject<Culture>(src.Id)))
                .ForMember(dest => dest.Language, opt => opt.MapFrom(src => CreateObject<Domain.Language>(src.IdLanguage)))
                .ForMember(dest => dest.TimeZone, opt => opt.MapFrom(src => CreateObject<TimeZone>(src.IdTimeZone)));

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

            CreateMap<CompetitionRequest, Competition>()
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => CreateObject<City>(src.IdCity)))
                .ForMember(t => t.Organizations, opt =>
                {
                    opt.PreCondition(s => s.IdsOrganization?.Length > 0);
                    opt.MapFrom(s => s.IdsOrganization.ToList());
                });

            CreateMap<CompetitionSearchFilterRequest, CompetitionSearchFilter>()
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => CreateObject<City>(src.IdCity)))
                .ForMember(dest => dest.Organizations, opt => opt.MapFrom(src => CreateObject<Organization>(src.IdsOrganization)));

            CreateMap<int, Organization>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src));

            CreateMap<OrganizationRequest, Organization>()
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => CreateObject<City>(src.IdCity)));
            CreateMap<OrganizationSearchFilterRequest, OrganizationSearchFilter>()
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => CreateObject<City>(src.IdCity)));

            CreateMap<BoatRequest, Boat>()
                .ForMember(dest => dest.RaceClass, opt => opt.MapFrom(src => CreateObject<RaceClass>(src.IdRaceClass)));

            CreateMap<BoatSearchFilterRequest, BoatSearchFilter>()
                .ForMember(dest => dest.RaceCategory, opt => opt.MapFrom(src => CreateObject<RaceCategory>(src.IdRaceCategory)))
                .ForMember(dest => dest.RaceClass, opt => opt.MapFrom(src => CreateObject<RaceClass>(src.IdRaceClass)));

            CreateMap<RaceCategoryRequest, RaceCategory>();
            CreateMap<RaceCategorySearchFilterRequest, RaceCategorySearchFilter>();

            CreateMap<RaceClassRequest, RaceClass>()
                .ForMember(dest => dest.RaceCategory, opt => opt.MapFrom(src => CreateObject<City>(src.IdRaceCategory)));
            CreateMap<RaceClassSearchFilterRequest, RaceClassSearchFilter>();

            CreateMap<RaceRequest, Race>()
                .ForMember(dest => dest.Competition, opt => opt.MapFrom(src => CreateObject<Competition>(src.IdCompetition)))
                .ForMember(dest => dest.RaceClass, opt => opt.MapFrom(src => CreateObject<RaceClass>(src.IdRaceClass)));
            CreateMap<RaceSearchFilterRequest, RaceSearchFilter>()
                .ForMember(dest => dest.Competition, opt => opt.MapFrom(src => CreateObject<Competition>(src.IdCompetition)))
                .ForMember(dest => dest.RaceClass, opt => opt.MapFrom(src => CreateObject<RaceClass>(src.IdRaceClass)));

            CreateMap<BloodTypeSearchFilterRequest, BloodTypeSearchFilter>();

            CreateMap<MedicalInsuranceSearchFilterRequest, MedicalInsuranceSearchFilter>();

            CreateMap<PersonRequest, Person>()
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => CreateObject<User>(src.IdUser)))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => CreateObject<Country>(src.IdCountry)))
                .ForMember(dest => dest.BloodType, opt => opt.MapFrom(src => CreateObject<BloodType>(src.IdBloodType)))
                .ForMember(dest => dest.MedicalInsurance, opt => opt.MapFrom(src => CreateObject<MedicalInsurance>(src.IdMedicalInsurance)));

            CreateMap<PersonSearchFilterRequest, PersonSearchFilter>();

            CreateMap<ContestantRequest, Contestant>()
                .ForMember(dest => dest.Person, opt => opt.MapFrom(src => CreateObject<Person>(src.IdPerson)));

            CreateMap<ContestantRoleSearchFilterRequest, ContestantRoleSearchFilter>();

            CreateMap<ContestantSearchFilterRequest, ContestantSearchFilter>()
                .ForMember(dest => dest.Person, opt => opt.MapFrom(src => CreateObject<Person>(src.IdPerson)));

            CreateMap<TeamRequest, Team>()
                .ForMember(dest => dest.Competition, opt => opt.MapFrom(src => CreateObject<Competition>(src.IdCompetition)))
                .ForMember(dest => dest.RaceClass, opt => opt.MapFrom(src => CreateObject<RaceClass>(src.IdRaceClass)));

            CreateMap<TeamSearchFilterRequest, TeamSearchFilter>()
                .ForMember(dest => dest.Competition, opt => opt.MapFrom(src => CreateObject<Competition>(src.IdCompetition)))
                .ForMember(dest => dest.RaceClass, opt => opt.MapFrom(src => CreateObject<RaceClass>(src.IdRaceClass)));

            CreateMap<TeamBoatRequest, TeamBoat>()
                .ForMember(dest => dest.Team, opt => opt.MapFrom(src => CreateObject<Team>(src.IdTeam)))
                .ForMember(dest => dest.Boat, opt => opt.MapFrom(src => CreateObject<Boat>(src.IdBoat)));

            CreateMap<TeamContestantRequest, TeamContestant>()
                .ForMember(dest => dest.Contestant, opt => opt.MapFrom(src => CreateObject<Contestant>(src.IdContestant)))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => CreateObject<ContestantRole>(src.IdContestantRole)));

            CreateMap<FlagSearchFilterRequest, FlagSearchFilter>();

            CreateMap<MastSearchFilterRequest, MastSearchFilter>()
                .ForMember(dest => dest.Competition, opt => opt.MapFrom(src => CreateObject<Competition>(src.IdCompetition)));

            CreateMap<MastFlagRequest, MastFlag>()
                .ForMember(dest => dest.Mast, opt => opt.MapFrom(src => CreateObject<Mast>(src.IdMast)))
                .ForMember(dest => dest.Flag, opt => opt.MapFrom(src => CreateObject<Flag>(src.IdFlag)))
                .ForMember(dest => dest.Person, opt => opt.MapFrom(src => CreateObject<Person>(src.IdPerson)));

            CreateMap<MastFlagSearchFilterRequest, MastFlagSearchFilter>()
                .ForMember(dest => dest.Mast, opt => opt.MapFrom(src => CreateObject<Mast>(src.IdMast)))
                .ForMember(dest => dest.Flag, opt => opt.MapFrom(src => CreateObject<Flag>(src.IdFlag)))
                .ForMember(dest => dest.Person, opt => opt.MapFrom(src => CreateObject<Person>(src.IdPerson)));

            #endregion

            #region Responses

            CreateMap<Translation, TranslationResponse>().ConvertUsing(typeof(TranslationToTranslationResponseConverter<Translation, TranslationResponse>));

            CreateMap(typeof(PaginatedResult<>), typeof(PaginatedResultResponse<>));

            CreateMap<AccessToken, AccessTokenResponse>();

            CreateMap<TimeZone, TimeZoneResponse>();
            CreateMap<Culture, CultureResponse>();
            CreateMap<Domain.Language, LanguageResponse>();
            CreateMap<UserSettings, UserSettingsResponse>();

            CreateMap<PrivacyPolicy, PrivacyPolicyResponse>();
            CreateMap<PrivacyPolicyAgreement, PrivacyPolicyAgreementResponse>();

            CreateMap<PasswordPolicy, PasswordPolicyResponse>();

            CreateMap<User, UserResponse>();
            CreateMap<User, UserSimpleResponse>();

            CreateMap<Role, RoleResponse>();

            CreateMap<File, FileResponse>();

            CreateMap<UserIdentification, UserIdentificationResponse>();

            CreateMap<Action, ActionResponse>();
            CreateMap<AuthorizationCondition, AuthorizationConditionResponse>();
            CreateMap<ActionRole, ActionRoleResponse>()
                .ForMember(dest => dest.IdAction, opt => opt.MapFrom(src => src.Action.Id))
                .ForMember(dest => dest.IdCondition, opt => opt.MapFrom(src => src.Condition.Id));
            CreateMap<RolePermissions, RolePermissionsResponse>()
                .ForMember(dest => dest.IdRole, opt => opt.MapFrom(src => src.Role.Id))
                .ForMember(dest => dest.Permissions, opt => opt.MapFrom(src => src.Permissions));

            CreateMap<Organization, OrganizationResponse>();

            CreateMap<BloodType, BloodTypeResponse>();

            CreateMap<MedicalInsurance, MedicalInsuranceResponse>();

            CreateMap<Country, CountryResponse>();

            CreateMap<City, CityResponse>();

            CreateMap<Person, PersonResponse>();
            CreateMap<Person, PersonSimpleResponse>();

            CreateMap<Boat, BoatResponse>();

            CreateMap<Competition, CompetitionResponse>();
            CreateMap<Competition, CompetitionSimpleResponse>();

            CreateMap<Contestant, ContestantResponse>();

            CreateMap<ContestantRole, ContestantRoleResponse>();

            CreateMap<Flag, FlagResponse>();

            CreateMap<Mast, MastResponse>();

            CreateMap<MastFlag, MastFlagResponse>();

            CreateMap<RaceCategory, RaceCategoryResponse>();

            CreateMap<RaceClass, RaceClassResponse>();

            CreateMap<Race, RaceResponse>();

            CreateMap<Team, TeamResponse>();

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

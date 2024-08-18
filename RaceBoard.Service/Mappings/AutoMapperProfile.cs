using AutoMapper;
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
using File = RaceBoard.Domain.File;
using TimeZone = RaceBoard.Domain.TimeZone;
using Action = RaceBoard.Domain.Action;
using RaceBoard.DTOs.Competition.Request;
using RaceBoard.DTOs.Organization.Request;
using RaceBoard.DTOs.Boat.Request;
//using static RaceBoard.Service.Mappings.AutoMapperProfile;

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
                .ForMember(dest => dest.Culture, opt => opt.MapFrom(src => new Culture { Id = src.Id }))
                .ForMember(dest => dest.Language, opt => opt.MapFrom(src => new Domain.Language { Id = src.IdLanguage }))
                .ForMember(dest => dest.TimeZone, opt => opt.MapFrom(src => new TimeZone { Id = src.IdTimeZone }));

            CreateMap<UserIdentificationRequest, UserIdentification>()
                .ForMember(x => x.User, opt => opt.MapFrom(x => x.IdUser != null ? new User() { Id = x.IdUser } : null))
                .ForMember(x => x.Type, opt => opt.MapFrom(x => x.IdType != null ? new IdentificationType() { Id = x.IdType } : null));

            CreateMap<UserIdentificationSearchFilterRequest, UserIdentificationSearchFilter>()
                .ForMember(x => x.User, opt => opt.MapFrom(x => x.IdUser != null ? new User() { Id = x.IdUser.Value } : null))
                .ForMember(x => x.Type, opt => opt.MapFrom(x => x.IdType != null ? new IdentificationType() { Id = x.IdType.Value } : null));

            CreateMap<PrivacyPolicyAgreementRequest, PrivacyPolicyAgreement>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.AgreementDate, opt => opt.Ignore())
                .ForMember(dest => dest.PrivacyPolicy, opt => opt.MapFrom(src => new PrivacyPolicy { Id = src.IdPrivacyPolicy }));

            CreateMap<int, Role>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src));

            CreateMap<ActionSearchFilterRequest, ActionSearchFilter>();
            
            CreateMap<ActionRoleRequest, ActionRole>()
                .ForMember(dest => dest.Action, opt => opt.MapFrom(src => new Action() { Id = src.IdAction }))
                .ForMember(dest => dest.Condition, opt => opt.MapFrom(src => new AuthorizationCondition() { Id = src.IdCondition } ));

            CreateMap<RolePermissionsRequest, RolePermissions>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => new Role() { Id = src.IdRole }))
                .ForMember(dest => dest.Permissions, opt => opt.MapFrom(src => src.Permissions));

            CreateMap<CompetitionRequest, Competition>()
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => new City() { Id = src.IdCity }))
                .ForMember(t => t.Organizations, opt =>
                {
                    opt.PreCondition(s => s.IdsOrganization?.Length > 0);
                    opt.MapFrom(s => s.IdsOrganization.ToList());
                });

            CreateMap<int, Organization>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src));

            CreateMap<OrganizationRequest, Organization>()
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => new City { Id = src.IdCity }));

            CreateMap<BoatRequest, Boat>();

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

        private T CreateObject<T>(int? id)
        {
            string typeFullName = typeof(T).AssemblyQualifiedName;

            Type type = Type.GetType(typeFullName);

            dynamic instance = Activator.CreateInstance(type);

            if (id.HasValue)
                instance.Id = id.Value;

            return (T)instance;
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

using AutoMapper;
using RaceBoard.Common;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Domain;
using RaceBoard.Domain.Spreadsheet.Abstract;
using RaceBoard.Domain.Spreadsheet.Sections;
using RaceBoard.Domain.StudioManagement;
using RaceBoard.Domain.Upload;
using RaceBoard.DTOs._Pagination.Request;
using RaceBoard.DTOs._Pagination.Response;
using RaceBoard.DTOs.ApprovalStatus.Response;
using RaceBoard.DTOs.Authentication.Response;
using RaceBoard.DTOs.Billing.Request;
using RaceBoard.DTOs.Billing.Response;
using RaceBoard.DTOs.File.Response;
using RaceBoard.DTOs.Hiring.Request;
using RaceBoard.DTOs.Hiring.Response;
using RaceBoard.DTOs.IdentificationType.Response;
using RaceBoard.DTOs.Language.Response;
using RaceBoard.DTOs.Password.Response;
using RaceBoard.DTOs.Payment.Request;
using RaceBoard.DTOs.Payment.Response;
using RaceBoard.DTOs.Payroll.Request;
using RaceBoard.DTOs.Payroll.Response;
using RaceBoard.DTOs.Permissions.Request;
using RaceBoard.DTOs.Permissions.Response;
using RaceBoard.DTOs.PrivacyPolicy.Response;
using RaceBoard.DTOs.Project.Request;
using RaceBoard.DTOs.Project.Response;
using RaceBoard.DTOs.RecordingType.Response;
using RaceBoard.DTOs.Script.Request;
using RaceBoard.DTOs.Script.Response;
using RaceBoard.DTOs.ScriptImport.Request;
using RaceBoard.DTOs.ScriptImport.Response;
using RaceBoard.DTOs.ScriptImportSettings.Response;
using RaceBoard.DTOs.Spending.Request;
using RaceBoard.DTOs.Spending.Response;
using RaceBoard.DTOs.Spreadsheet.Request;
using RaceBoard.DTOs.Spreadsheet.Request.Abstract;
using RaceBoard.DTOs.Spreadsheet.Request.Sections;
using RaceBoard.DTOs.Studio.Response;
using RaceBoard.DTOs.StudioManagement.Request;
using RaceBoard.DTOs.StudioManagement.Response;
using RaceBoard.DTOs.Translation.Response;
using RaceBoard.DTOs.User.Request;
using RaceBoard.DTOs.User.Response;
using RaceBoard.DTOs.User.Response.Settings;
using RaceBoard.Translations.Entities;
using File = RaceBoard.Domain.File;
using TimeZone = RaceBoard.Domain.TimeZone;
using Action = RaceBoard.Domain.Action;
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

            CreateMap<HiringRequest, Hiring>()
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => new User { Id = src.IdUser }))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => new HiringType { Id = src.IdType }))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => new Role { Id = src.IdRole }));

            CreateMap<HiringSearchFilterRequest, HiringSearchFilter>()
                  .ForMember(t => t.Type, opt =>
                  {
                      opt.PreCondition(s => s.IdType != null);
                      opt.MapFrom(s => new HiringType() { Id = s.IdType.Value });
                  })
                  .ForMember(t => t.Provider, opt =>
                  {
                      opt.PreCondition(s => s.Name != null);
                      opt.MapFrom(s => new User() { Firstname = s.Name });
                  })
                  .ForMember(t => t.Roles, opt =>
                  {
                      opt.PreCondition(s => s.IdsRole?.Length > 0);
                      opt.MapFrom(s => s.IdsRole.ToList());
                  });

            CreateMap<StudioManagementSearchFilterRequest, StudioManagementSearchFilter>();

            CreateMap<int, Role>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src));

            CreateMap<ProjectRequest, Project>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => new ProjectType { Id = src.IdType }));

            CreateMap<ProjectSearchFilterRequest, ProjectSearchFilter>()
                  .ForMember(t => t.Type, opt =>
                  {
                      opt.PreCondition(s => s.IdType != null);
                      opt.MapFrom(s => new ProjectType() { Id = s.IdType.Value });
                  });

            CreateMap<ScriptRequest, Script>()
                .ForMember(dest => dest.Project, opt => opt.MapFrom(src => new Project { Id = src.IdProject }))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => new ScriptStatus { Id = src.IdStatus }));

            CreateMap<ScriptSearchFilterRequest, ScriptSearchFilter>()
                .ForMember(t => t.Id, opt =>
                {
                    opt.PreCondition(s => s.IdScript != null);
                    opt.MapFrom(s => s.IdScript.Value);
                })
                .ForMember(t => t.CreationUser, opt =>
                 {
                     opt.PreCondition(s => s.IdCreationUser != null);
                     opt.MapFrom(s => new User() { Id = s.IdCreationUser.Value });
                 })
                .ForMember(t => t.Project, opt =>
                {
                    opt.PreCondition(s => s.IdProject != null);
                    opt.MapFrom(s => new Project() { Id = s.IdProject.Value });
                })
                .ForMember(t => t.Status, opt =>
                {
                    opt.PreCondition(s => s.IdStatus != null);
                    opt.MapFrom(s => new ScriptStatus() { Id = s.IdStatus.Value });
                });

            CreateMap<ScriptItemRequest, ScriptItem>()
                .ForMember(dest => dest.Script, opt => opt.MapFrom(src => new Script { Id = src.IdScript }))
                .ForMember(dest => dest.Actor, opt => opt.MapFrom(src => new User { Id = src.IdActor }))
                .ForMember(t => t.RecordingType, opt =>
                {
                    opt.PreCondition(s => s.IdRecordingType != null);
                    opt.MapFrom(s => new RecordingType() { Id = s.IdRecordingType.Value });
                });

            CreateMap<ScriptRoleRequest, ScriptRole>()
                .ForMember(dest => dest.Script, opt => opt.MapFrom(src => new Script { Id = src.IdScript }))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => new Role { Id = src.IdRole }))
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => new User { Id = src.IdUser }));

            CreateMap<ScriptCostsRequest, ScriptCosts>()
                .ForMember(dest => dest.Script, opt => opt.MapFrom(src => new Script { Id = src.IdScript }));

            CreateMap<ScriptApprovalRequest, ScriptApproval>()
                .ForMember(dest => dest.Script, opt => opt.MapFrom(src => new Script { Id = src.IdScript }))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => new Role { Id = src.IdRole }))
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => new User { Id = src.IdUser }))
                .ForMember(dest => dest.ApprovalStatus, opt => opt.MapFrom(src => new ApprovalStatus { Id = src.IdStatusApproval }));

            CreateMap<PaymentComplaintRequest, PaymentComplaint>().ConvertUsing(typeof(PaymentComplaintRequestToPaymentComplaintConverter<PaymentComplaintRequest, PaymentComplaint>));

            CreateMap<PaymentSearchFilterRequest, PaymentSearchFilter>()
                .ForMember(x => x.Scripts, opt => opt.MapFrom(x => x.IdsScript != null ? x.IdsScript.Select(id => new Script { Id = id }): null))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => CreateObject<PaymentStatus>(src.IdPaymentStatus)))
                .ForMember(dest => dest.Method, opt => opt.MapFrom(src => CreateObject<PaymentMethod>(src.IdPaymentMethod)))
                .ForMember(dest => dest.Provider, opt => opt.MapFrom(src => CreateObject<User>(src.IdProvider)))
                .ForMember(dest => dest.ProviderRole, opt => opt.MapFrom(src => CreateObject<Role>(src.IdProviderRole)))
                .ForMember(dest => dest.ProviderHiringType, opt => opt.MapFrom(src => CreateObject<HiringType>(src.IdProviderHiringType)));

            CreateMap<PaymentComplaintSearchFilterRequest, PaymentComplaintSearchFilter>()
                .ForMember(x => x.Payments, opt => opt.MapFrom(x => x.IdsPayment.Select(id => new Payment { Id = id })))
                .ForMember(x => x.Script, opt => opt.MapFrom(x => x.IdScript != null ? new Script() { Id = x.IdScript.Value } : null))
                .ForMember(x => x.Status, opt => opt.MapFrom(x => x.IdPaymentComplaintStatus != null ? new PaymentComplaintStatus() { Id = x.IdPaymentComplaintStatus.Value } : null))
                .ForMember(x => x.User, opt => opt.MapFrom(x => x.IdUser != null ? new User() { Id = x.IdUser.Value } : null));
            CreateMap<PaymentComplaintItemSearchFilterRequest, PaymentComplaintItemSearchFilter>();


            CreateMap<SpreadsheetRequest, Spreadsheet>()
                .ForMember(x => x.Type, opt => opt.MapFrom(x => new SpreadsheetType() { Id = x.IdType }));
            CreateMap<ActorSpreadsheetRequest, ActorSpreadsheet>()
                .ForMember(x => x.Type, opt => opt.MapFrom(x => new SpreadsheetType() { Id = x.IdType }));
            CreateMap<StaffSpreadsheetRequest, StaffSpreadsheet>()
                .ForMember(x => x.Type, opt => opt.MapFrom(x => new SpreadsheetType() { Id = x.IdType }));
            CreateMap<CreditSpreadsheetRequest, CreditSpreadsheet>()
                .ForMember(x => x.Type, opt => opt.MapFrom(x => new SpreadsheetType() { Id = x.IdType }));
            CreateMap<PaymentSpreadsheetItemRequest, PaymentSpreadsheetItem>();
            CreateMap<ActorSpreadsheetItemRequest, ActorSpreadsheet.Item>();
            CreateMap<StaffSpreadsheetItemRequest, StaffSpreadsheet.Item>();
            CreateMap<CreditSpreadsheetItemRequest, CreditSpreadsheet.Item>();
            CreateMap<SpreadsheetHeaderRequest, SpreadsheetHeader>();
            CreateMap<SpreadsheetObservationsRequest, SpreadsheetObservations>();
            CreateMap<SpreadsheetProgramRequest, SpreadsheetProgram>();
            CreateMap<SpreadsheetTotalsRequest, SpreadsheetTotals>();

            CreateMap<StudioManagementSearchFilterRequest, StudioManagementSearchFilter>();

            CreateMap<StudioManagementRequest, StudioManagement>();
            CreateMap<StudioManagementTaxInformationRequest, StudioManagementTaxInformation>();
            CreateMap<StudioManagementAddressRequest, StudioManagementAddress>();

            CreateMap<PayrollSearchFilterRequest, PayrollSearchFilter>()
                .ForMember(x => x.Script, opt => opt.MapFrom(x => new Script() { Id = x.IdScript }))
                .ForMember(x => x.Provider, opt => opt.MapFrom(x => new User() { Id = x.IdProvider }))
                .ForMember(x => x.ProviderRole, opt => opt.MapFrom(x => new Role() { Id = x.IdProviderRole }));

            CreateMap<BillingSearchFilterRequest, BillingSearchFilter>()
                .ForMember(x => x.Project, opt => opt.MapFrom(x => new Project() { Id = x.IdProject }));

            CreateMap<SpendingSearchFilterRequest, SpendingSearchFilter>()
                .ForMember(x => x.Period, opt => opt.MapFrom(x => x.IdPeriod != null ? new SpendingPeriod { Id = x.IdPeriod.Value } : null))
                .ForMember(x => x.Item, opt => opt.MapFrom(x => x.IdItem != null ? new SpendingItem { Id = x.IdItem.Value } : null))
                .ForMember(x => x.Category, opt => opt.MapFrom(x => x.IdCategory != null ? new SpendingItemCategory { Id = x.IdCategory.Value } : null));

            CreateMap<ActionSearchFilterRequest, ActionSearchFilter>();
            
            CreateMap<ActionRoleRequest, ActionRole>()
                .ForMember(dest => dest.Action, opt => opt.MapFrom(src => new Action() { Id = src.IdAction }))
                .ForMember(dest => dest.Condition, opt => opt.MapFrom(src => new AuthorizationCondition() { Id = src.IdCondition } ));

            CreateMap<RolePermissionsRequest, RolePermissions>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => new Role() { Id = src.IdRole }))
                .ForMember(dest => dest.Permissions, opt => opt.MapFrom(src => src.Permissions));


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

            CreateMap<Studio, StudioResponse>();

            CreateMap<HiringType, HiringTypeResponse>();
            CreateMap<Hiring, HiringResponse>();

            CreateMap<User, UserResponse>();
            CreateMap<User, UserSimpleResponse>();

            CreateMap<Role, RoleResponse>();

            CreateMap<ProjectType, ProjectTypeResponse>();
            CreateMap<Project, ProjectResponse>();
            CreateMap<Project, ProjectSimpleResponse>();

            CreateMap<RecordingType, RecordingTypeResponse>();

            CreateMap<ApprovalStatus, ApprovalStatusResponse>();

            CreateMap<Script, ScriptResponse>();
            CreateMap<Script, ScriptSimpleResponse>();
            CreateMap<ScriptStatus, ScriptStatusResponse>();

            CreateMap<ScriptItem, ScriptItemResponse>()
                .ForMember(dest => dest.IdScript, opt => opt.MapFrom(src => src.Script.Id))
                .ForMember(dest => dest.IdActor, opt => opt.MapFrom(src => src.Actor.Id))
                .ForMember(dest => dest.IdRecordingType, opt => opt.MapFrom(src => src.RecordingType.Id));
            
            CreateMap<ScriptRole, ScriptRoleResponse>()
                .ForMember(dest => dest.IdScript, opt => opt.MapFrom(src => src.Script.Id))
                .ForMember(dest => dest.IdRole, opt => opt.MapFrom(src => src.Role.Id))
                .ForMember(dest => dest.IdUser, opt => opt.MapFrom(src => src.User.Id));
            
            CreateMap<ScriptCosts, ScriptCostsResponse>()
                .ForMember(dest => dest.IdScript, opt => opt.MapFrom(src => src.Script.Id));
            
            CreateMap<ScriptCostsTemplate, ScriptCostsTemplateResponse>()
                .ForMember(dest => dest.IdProjectType, opt => opt.MapFrom(src => src.ProjectType.Id));
            
            CreateMap<ScriptApproval, ScriptApprovalResponse>()
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date));
            CreateMap<ScriptImportSettings, ScriptImportSettingsResponse>();
            CreateMap<ScriptImportSettingsTable, ScriptImportSettingsTableResponse>();
            CreateMap<ScriptImportSettingsTableLayout, ScriptImportSettingsTableLayoutResponse>();
            CreateMap<ScriptImportSettingsValidation, ScriptImportSettingsValidationResponse>();
            CreateMap<ScriptImportSettingsAnnotation, ScriptImportSettingsAnnotationResponse>();
            CreateMap<ScriptImportSettingsLoopCountMethod, ScriptImportSettingsLoopCountMethodResponse>();

            CreateMap<ScriptImport, ScriptImportResponse>();
            CreateMap<ScriptImportFile, ScriptImportFileResponse>();
            CreateMap<ScriptImportFileType, ScriptImportFileTypeResponse>();

            CreateMap<File, FileResponse>();

            CreateMap<Payment, PaymentResponse>();
            CreateMap<PaymentStatus, PaymentStatusResponse>();
            CreateMap<PaymentMethod, PaymentMethodResponse>();
            CreateMap<PaymentComplaint, PaymentComplaintResponse>();
            CreateMap<PaymentComplaintItem, PaymentComplaintItemResponse>();
            CreateMap<PaymentComplaintStatus, PaymentComplaintStatusResponse>();

            CreateMap<SpreadsheetType, SpreadsheetTypeResponse>();

            CreateMap<StudioManagement, StudioManagementResponse>();
            CreateMap<StudioManagementTaxInformation, StudioManagementTaxInformationResponse>();
            CreateMap<StudioManagementAddress, StudioManagementAddressResponse>();
            CreateMap<StudioManagementImage, StudioManagementImageResponse>();

            CreateMap<IdentificationType, IdentificationTypeResponse>();

            CreateMap<UserIdentification, UserIdentificationResponse>();

            CreateMap<Payroll, PayrollResponse>();
            CreateMap<Billing, BillingResponse>();

            CreateMap<Spending, SpendingResponse>();
            CreateMap<SpendingPeriod, SpendingPeriodResponse>();
            CreateMap<SpendingItem, SpendingItemResponse>();
            CreateMap<SpendingItemAmount, SpendingItemAmountResponse>();
            CreateMap<SpendingItemCategory, SpendingItemCategoryResponse>();

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

        public class PaymentComplaintRequestToPaymentComplaintConverter<TValue, TVersion> : ITypeConverter<PaymentComplaintRequest, PaymentComplaint>
        {
            public PaymentComplaint Convert(PaymentComplaintRequest source, PaymentComplaint destination, ResolutionContext context)
            {
                var complaint = new PaymentComplaint();

                if (source == null)
                    return complaint;

                complaint.Payment = new Payment()
                {
                    Id = source.IdPayment
                };

                complaint.Status = new PaymentComplaintStatus()
                {
                    Id = source.IdStatus
                };

                complaint.Items = new List<PaymentComplaintItem>()
                {
                    { new PaymentComplaintItem() { Message = source.Message } }
                };

                return complaint;
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

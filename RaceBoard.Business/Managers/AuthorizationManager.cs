using RaceBoard.Business.Managers.Abstract;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Business.Validators.Interfaces;
using RaceBoard.Common.Exceptions;
using RaceBoard.Data;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;
using RaceBoard.Translations.Interfaces;
using Enums = RaceBoard.Domain.Enums;

namespace RaceBoard.Business.Managers
{
    public class AuthorizationManager : AbstractManager, IAuthorizationManager
    {
        private readonly IAuthorizationRepository _authorizationRepository;
        private readonly ICustomValidator<RolePermissions> _rolePermissionValidator;

        private readonly IOrganizationMemberRepository _organizationMemberRepository;
        private readonly IChampionshipMemberRepository _championshipMemberRepository;
        private readonly ITeamMemberRepository _teamMemberRepository;

        #region Permissions Matrix

        private static int[] _allRoles = new int[]
        {
            (int)Enums.UserRole.Manager,
            (int)Enums.UserRole.Auxiliary,
            (int)Enums.UserRole.Jury,
            //(int)Enums.UserRole.Competitor
            (int)Enums.TeamMemberRole.Leader,
            (int)Enums.TeamMemberRole.Helm,
            (int)Enums.TeamMemberRole.Crew
        };

        private Dictionary<Enums.Action, int[]> _permissions = new Dictionary<Enums.Action, int[]>
        {
            { Enums.Action.Organization_Get, _allRoles },
            { Enums.Action.Organization_Create, new int[] { (int)Enums.UserRole.Manager } },
            { Enums.Action.Organization_Update, new int[] { (int)Enums.UserRole.Manager } },
            { Enums.Action.Organization_Delete, new int[] { (int)Enums.UserRole.Manager } },

            { Enums.Action.OrganizationMember_Get, _allRoles },
            { Enums.Action.OrganizationMember_Create, new int[] { (int)Enums.UserRole.Manager } },
            { Enums.Action.OrganizationMember_Update, new int[] { (int)Enums.UserRole.Manager } },
            { Enums.Action.OrganizationMember_Delete, new int[] { (int)Enums.UserRole.Manager } },

            { Enums.Action.Championship_Get, _allRoles },
            { Enums.Action.Championship_Create, new int[] { (int)Enums.UserRole.Manager } },
            { Enums.Action.Championship_Update, new int[] { (int)Enums.UserRole.Manager } },
            { Enums.Action.Championship_Delete, new int[] { (int)Enums.UserRole.Manager } },

            { Enums.Action.ChampionshipMember_Get, _allRoles },
            { Enums.Action.ChampionshipMember_Create, new int[] { (int)Enums.UserRole.Manager } },
            { Enums.Action.ChampionshipMember_Update, new int[] { (int)Enums.UserRole.Manager } },
            { Enums.Action.ChampionshipMember_Delete, new int[] { (int)Enums.UserRole.Manager } },

            { Enums.Action.ChampionshipGroup_Get, _allRoles },
            { Enums.Action.ChampionshipGroup_Create, new int[] { (int)Enums.UserRole.Manager } },
            { Enums.Action.ChampionshipGroup_Update, new int[] { (int)Enums.UserRole.Manager } },
            { Enums.Action.ChampionshipGroup_Delete, new int[] { (int)Enums.UserRole.Manager } },

            { Enums.Action.ChampionshipFile_Get, _allRoles },
            { Enums.Action.ChampionshipFile_Create, new int[] { (int)Enums.UserRole.Manager } },
            { Enums.Action.ChampionshipFile_Update, new int[] {} },
            { Enums.Action.ChampionshipFile_Delete, new int[] { (int)Enums.UserRole.Manager } },

            { Enums.Action.ChampionshipFlag_Get, _allRoles },
            { Enums.Action.ChampionshipFlag_Create, new int[] { (int)Enums.UserRole.Manager, (int)Enums.UserRole.Auxiliary } },
            { Enums.Action.ChampionshipFlag_Update, new int[] { (int)Enums.UserRole.Manager, (int)Enums.UserRole.Auxiliary } },
            { Enums.Action.ChampionshipFlag_Delete, new int[] { (int)Enums.UserRole.Manager } },

            { Enums.Action.ChampionshipCommitteeBoatReturn_Get, _allRoles },
            { Enums.Action.ChampionshipCommitteeBoatReturn_Create, new int[] { (int)Enums.UserRole.Manager } },
            { Enums.Action.ChampionshipCommitteeBoatReturn_Update, new int[] {} },
            { Enums.Action.ChampionshipCommitteeBoatReturn_Delete, new int[] { (int)Enums.UserRole.Manager } },

            { Enums.Action.ChampionshipNotification_Get, _allRoles },
            { Enums.Action.ChampionshipNotification_Create, new int[] { (int)Enums.UserRole.Manager, (int)Enums.UserRole.Auxiliary } },
            { Enums.Action.ChampionshipNotification_Update, new int[] {} },
            { Enums.Action.ChampionshipNotification_Delete, new int[] { (int)Enums.UserRole.Manager } },

            { Enums.Action.ChampionshipProtest_Get, _allRoles },
            { Enums.Action.ChampionshipProtest_Create, new int[] { (int)Enums.TeamMemberRole.Leader, (int)Enums.TeamMemberRole.Helm, (int)Enums.TeamMemberRole.Crew } },
            { Enums.Action.ChampionshipProtest_Update, new int[] { (int)Enums.TeamMemberRole.Leader, (int)Enums.TeamMemberRole.Helm, (int)Enums.TeamMemberRole.Crew, (int)Enums.UserRole.Jury } },
            { Enums.Action.ChampionshipProtest_Delete, new int[] { } },


            { Enums.Action.Team_Get, _allRoles },
            { Enums.Action.Team_Create, _allRoles },
            { Enums.Action.Team_Update, new int[] { (int)Enums.TeamMemberRole.Leader } },
            { Enums.Action.Team_Delete, new int[] { (int)Enums.TeamMemberRole.Leader } },

            { Enums.Action.TeamMember_Get, _allRoles },
            { Enums.Action.TeamMember_Create, new int[] { (int)Enums.TeamMemberRole.Leader } },
            { Enums.Action.TeamMember_Update, new int[] { (int)Enums.TeamMemberRole.Leader } },
            { Enums.Action.TeamMember_Delete, new int[] { (int)Enums.TeamMemberRole.Leader } },

            { Enums.Action.TeamBoat_Get, _allRoles },
            { Enums.Action.TeamBoat_Create, new int[] { (int)Enums.TeamMemberRole.Leader } },
            { Enums.Action.TeamBoat_Update, new int[] { (int)Enums.TeamMemberRole.Leader } },
            { Enums.Action.TeamBoat_Delete, new int[] { (int)Enums.TeamMemberRole.Leader } },

            { Enums.Action.TeamMemberCheckInOut_Get, _allRoles },
            { Enums.Action.TeamMemberCheckInOut_Create, new int[] { (int)Enums.UserRole.Manager, (int)Enums.UserRole.Auxiliary } },
            { Enums.Action.TeamMemberCheckInOut_Update, new int[] { (int)Enums.TeamMemberRole.Leader } },
            { Enums.Action.TeamMemberCheckInOut_Delete, new int[] { (int)Enums.TeamMemberRole.Leader } },
        };

        #endregion

        protected enum Entity
        {
            Unparented = 0,
            Organization = 1,
            Championship = 2,
            Team = 3
        }

        #region Constructors

        public AuthorizationManager
            (
                IAuthorizationRepository authorizationRepository,
                IOrganizationMemberRepository organizationMemberRepository,
                IChampionshipMemberRepository championshipMemberRepository,
                ITeamMemberRepository teamMemberRepository,
                ICustomValidator<RolePermissions> rolePermissionValidator,
                IRequestContextManager requestContextManager,
                ITranslator translator
            ) : base(requestContextManager, translator)
        {
            _authorizationRepository = authorizationRepository;
            _rolePermissionValidator = rolePermissionValidator;

            _organizationMemberRepository = organizationMemberRepository;
            _championshipMemberRepository = championshipMemberRepository;
            _teamMemberRepository = teamMemberRepository;
        }

        #endregion

        #region IAuthorizationManager implementation

        public Enums.AuthorizationCondition GetUserPermissionToPerformAction(int idAction, int idUser)
        {
            return _authorizationRepository.GetUserPermissionToPerformAction(idAction, idUser);
        }

        public List<AuthorizationCondition> GetAuthorizationConditions(ITransactionalContext? context = null)
        {
            return _authorizationRepository.GetAuthorizationConditions(context);
        }

        public RolePermissions GetRolePermissions(int idRole, ITransactionalContext? context = null)
        {
            return _authorizationRepository.GetRolePermissions(idRole, context);
        }

        private Entity GetParentEntity(Enums.Action action)
        {
            //if (action.ToString().StartsWith(Entity.Organization.ToString()))
            //    return Entity.Organization;

            //if (action.ToString().StartsWith(Entity.Championship.ToString()))
            //    return Entity.Championship;

            //if (action.ToString().StartsWith(Entity.Team.ToString()))
            //    return Entity.Team;

            Entity parent = Entity.Unparented;

            bool isCreate = action.ToString().EndsWith("_Create") ? true : false;

            string entity = action.ToString().Replace("_Get", "").Replace("_Create", "").Replace("_Update", "").Replace("_Delete", "");

            if (entity.Contains(Entity.Organization.ToString()))
            {
                if (entity.Replace(Entity.Organization.ToString(), "").Length == 0 && isCreate)
                    parent = Entity.Unparented;
                else
                    parent = Entity.Organization;
            }
            if (entity.Contains(Entity.Championship.ToString()))
            {
                if (entity.Replace(Entity.Championship.ToString(), "").Length == 0 && isCreate)
                    parent = Entity.Organization;
                else
                    parent = Entity.Championship;
            }
            if (entity.Contains(Entity.Team.ToString()))
            {
                if (entity.Replace(Entity.Team.ToString(), "").Length == 0 && isCreate)
                    parent = Entity.Championship;
                else
                    parent = Entity.Team;
            }

            return parent;
        }

        public void ValidatePermission(Enums.Action action, int idEntity, int idUser)
        {
            dynamic? record = null;

            Entity parent = this.GetParentEntity(action);

            if (parent == Entity.Unparented)
            {
                return;
            }

            var user = new User() { Id = idUser };

            if (parent == Entity.Organization)
            {
                var searchFilter = new OrganizationMemberSearchFilter()
                {
                    Organization = new Organization() { Id = idEntity },
                    User = user,
                    IsActive = true
                };
                record = _organizationMemberRepository.Get(searchFilter).Results.FirstOrDefault();
            }

            if (parent == Entity.Championship)
            {
                var memberSearchFilter = new ChampionshipMemberSearchFilter()
                {
                    Championship = new Championship() { Id = idEntity },
                    User = user,
                    IsActive = true
                };
                record = _championshipMemberRepository.Get(memberSearchFilter).Results.FirstOrDefault();
            }

            if (parent == Entity.Team)
            {
                var searchFilter = new TeamMemberSearchFilter()
                {
                    Team = new Team() { Id = idEntity },
                    User = user,
                    IsActive = true
                };
                record = _teamMemberRepository.Get(searchFilter).Results.FirstOrDefault();
            }

            if (record == null)
                throw new FunctionalException(Common.Enums.ErrorType.Forbidden, base.Translate("NeedPermissions"));

            int idRole = record.Role.Id;

            int[] idsRole = _permissions[action];

            if (!idsRole.Contains(idRole))
                throw new FunctionalException(Common.Enums.ErrorType.Forbidden, base.Translate("NeedPermissions"));
        }

        #endregion
    }
}

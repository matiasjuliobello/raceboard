using RaceBoard.Data.Helpers.Interfaces;
using RaceBoard.Data.Repositories.Base.Abstract;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;
using Dapper;
using Enums = RaceBoard.Domain.Enums;
using Domain = RaceBoard.Domain;
using Microsoft.AspNetCore.Rewrite;

namespace RaceBoard.Data.Repositories
{
    public class AuthorizationRepository : AbstractRepository, IAuthorizationRepository
    {
        #region Private Members

        private const Enums.AuthorizationCondition _defaultAuthCondition = Enums.AuthorizationCondition.Deny;

        #endregion

        #region Constructors

        public AuthorizationRepository(IContextResolver contextResolver, IQueryBuilder queryBuilder) : base(contextResolver, queryBuilder)
        {
        }

        #endregion

        #region IAuthorizationRepository implementation

        public Enums.AuthorizationCondition GetUserPermissionToPerformAction(int idAction, int idUser, ITransactionalContext? context = null)
        {
            if (idAction <= 0 || idUser <= 0)
                return _defaultAuthCondition;

            string sql = @"SELECT
                            [Action_Role].Id [Id],
                            [Action].Id [Id],
                            [Action].Name,
                            [Role].Id,
                            [Role].Name,
                            [AuthorizationCondition].Id [Id],
                            [AuthorizationCondition].Name
                        FROM [Action_Role]
                        INNER JOIN [Role] [Role] ON [Role].Id = [Action_Role].IdRole
                        INNER JOIN [Action] [Action] ON [Action].Id = [Action_Role].IdAction
                        INNER JOIN [AuthorizationCondition] [AuthorizationCondition] ON [AuthorizationCondition].Id = [Action_Role].IdAuthorizationCondition";

            QueryBuilder.AddCommand(sql);

            base.AddFilterCriteria(ConditionType.Equal, "Action", "Id", "idAction", idAction);
            //base.AddFilterCriteria(ConditionType.Equal, "Role", "Role", "idRole", idRole);

            var rolePermissions = new List<ActionRole>();

            base.GetReader((x) =>
            {
                rolePermissions = x.Read<ActionRole, Domain.Action, Role, AuthorizationCondition, ActionRole>
                (
                    (actionRole, action, role, condition) =>
                    {
                        actionRole.Action = action;
                        actionRole.Role = role;
                        actionRole.Condition = condition;

                        return actionRole;
                    },
                    splitOn: "Id, Id, Id, Id"
                ).ToList();
            });

            var deniedPermission = rolePermissions.Where(x => x.Condition.Id == (int)Enums.AuthorizationCondition.Deny);

            var rolesWithAccess = rolePermissions.Except(deniedPermission).ToList();
            if (rolesWithAccess.Any())
            {
                var mostPrivilegedRolePermission = rolesWithAccess.OrderBy(x => x.Condition.Id).FirstOrDefault();
                if (mostPrivilegedRolePermission?.Condition?.Id > 0)
                    return (Enums.AuthorizationCondition)mostPrivilegedRolePermission.Condition.Id;
            }
            else
            {
                return _defaultAuthCondition;
            }

            return _defaultAuthCondition;
        }

        public List<AuthorizationCondition> GetAuthorizationConditions(ITransactionalContext? context = null)
        {
            return this._GetAuthorizationConditions(context);
        }

        public RolePermissions GetRolePermissions(int idRole, ITransactionalContext? context = null)
        {
            return this._GetRolePermissions(idRole, context);
        }

        #endregion

        #region Private Methods

        private List<AuthorizationCondition> _GetAuthorizationConditions(ITransactionalContext? context = null)
        {
            string sql = $@"SELECT
                                [AuthorizationCondition].Id [Id],
                                [AuthorizationCondition].Name [Name],
                                [AuthorizationCondition].Description [Description]
                            FROM [AuthorizationCondition]";

            QueryBuilder.AddCommand(sql);

            return base.GetMultipleResults<AuthorizationCondition>(context).ToList();
        }

        private RolePermissions _GetRolePermissions(int idRole, ITransactionalContext? context = null)
        {
            string sql = $@"SELECT
                                [Action_Role].Id [Id],
                                [Action].Id [Id],
                                [Action].Id [Name],
                                [AuthorizationCondition].Id [Id],
                                [AuthorizationCondition].Name [Name]
                            FROM [Action_Role] [Action_Role]
                            INNER JOIN [Action] [Action] ON [Action].Id = [Action_Role].IdAction
                            INNER JOIN [AuthorizationCondition] [AuthorizationCondition] ON [AuthorizationCondition].Id = [Action_Role].IdAuthorizationCondition";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("idRole", idRole);
            QueryBuilder.AddCondition("[Action_Role].IdRole = @idRole");

            var rolePermissions = new RolePermissions()
            {
                Role = new Role() { Id = idRole }
            };

            using (var connection = base.GetConnection())
            {
                connection.Open();

                var transaction = connection.BeginTransaction();
                var actionRoles = connection.Query<ActionRole, Domain.Action, AuthorizationCondition, ActionRole>
                    (
                        QueryBuilder.Build(),
                            (actionRole, action, authorizationCondition) =>
                            {
                                actionRole.Action = action;
                                actionRole.Condition = authorizationCondition;

                                return actionRole;
                            },
                            param: QueryBuilder.GetParameters(), transaction: transaction,
                            splitOn: "Id, Id, Id"
                    ).AsList();

                rolePermissions.Permissions = actionRoles;
            }

            return rolePermissions;
        }

        #endregion
    }
}
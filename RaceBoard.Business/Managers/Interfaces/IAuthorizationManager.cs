using RaceBoard.Data;
using RaceBoard.Domain;
using Enums = RaceBoard.Domain.Enums;

namespace RaceBoard.Business.Managers.Interfaces
{
    public interface IAuthorizationManager
    {
        Enums.AuthorizationCondition GetUserPermissionToPerformAction(int idAction, int idUser);
        List<AuthorizationCondition> GetAuthorizationConditions(ITransactionalContext? context = null);
        RolePermissions GetRolePermissions(int idRole, ITransactionalContext? context = null);
    }
}
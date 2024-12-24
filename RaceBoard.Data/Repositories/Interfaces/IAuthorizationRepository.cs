using RaceBoard.Domain;
using Enums = RaceBoard.Domain.Enums;

namespace RaceBoard.Data.Repositories.Interfaces
{
    public interface IAuthorizationRepository
    {
        ITransactionalContext GetTransactionalContext(TransactionContextScope scope = TransactionContextScope.Internal);

        Enums.AuthorizationCondition GetUserPermissionToPerformAction(int idAction, int idUser, ITransactionalContext? context = null);
        List<AuthorizationCondition> GetAuthorizationConditions(ITransactionalContext? context = null);
        RolePermissions GetRolePermissions(int idRole, ITransactionalContext? context = null);
    }
}

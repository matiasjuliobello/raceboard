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

        #region Constructors

        public AuthorizationManager
            (
                IAuthorizationRepository authorizationRepository,
                ICustomValidator<RolePermissions> rolePermissionValidator,
                ITranslator translator
            ) : base(translator)
        {
            _authorizationRepository = authorizationRepository;
            _rolePermissionValidator = rolePermissionValidator;
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

        #endregion
    }
}

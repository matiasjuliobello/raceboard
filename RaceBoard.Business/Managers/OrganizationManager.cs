using RaceBoard.Business.Managers.Abstract;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Data;
using RaceBoard.Translations.Interfaces;
using RaceBoard.Domain;

namespace RaceBoard.Business.Managers
{
    public class OrganizationManager : AbstractManager, IOrganizationManager
    {
        private readonly IOrganizationRepository _organizationRepository;

        #region Constructors

        public OrganizationManager
            (
                IOrganizationRepository organizationRepository,
                ITranslator translator
            ) : base(translator)
        {
            _organizationRepository = organizationRepository;
        }

        #endregion

        #region IOrganizationManager implementation

        public void Create(Organization organization, ITransactionalContext? context = null)
        {
            //_organizationValidator.SetTransactionalContext(context);
            //if (!_organizationValidator.IsValid(organization, Scenario.Create))
            //    throw new FunctionalException(ErrorType.ValidationError, _organizationValidator.Errors);

            if (context == null)
                context = _organizationRepository.GetTransactionalContext(TransactionContextScope.Internal);

            try
            {
                _organizationRepository.Create(organization, context);

                _organizationRepository.ConfirmTransactionalContext(context);

            }
            catch (Exception)
            {
                _organizationRepository.CancelTransactionalContext(context);
                throw;
            }
        }

        public void Update(Organization organization, ITransactionalContext? context = null)
        {
            //_organizationValidator.SetTransactionalContext(context);
            //if (!_organizationValidator.IsValid(organization, Scenario.Create))
            //    throw new FunctionalException(ErrorType.ValidationError, _organizationValidator.Errors);

            if (context == null)
                context = _organizationRepository.GetTransactionalContext(TransactionContextScope.Internal);

            try
            {
                _organizationRepository.Update(organization, context);

                _organizationRepository.ConfirmTransactionalContext(context);

            }
            catch (Exception)
            {
                _organizationRepository.CancelTransactionalContext(context);
                throw;
            }
        }

        public void Delete(int id, ITransactionalContext? context = null)
        {
            //_organizationValidator.SetTransactionalContext(context);
            //if (!_organizationValidator.IsValid(organization, Scenario.Create))
            //    throw new FunctionalException(ErrorType.ValidationError, _organizationValidator.Errors);

            if (context == null)
                context = _organizationRepository.GetTransactionalContext(TransactionContextScope.Internal);

            try
            {
                _organizationRepository.Delete(id, context);

                _organizationRepository.ConfirmTransactionalContext(context);

            }
            catch (Exception)
            {
                _organizationRepository.CancelTransactionalContext(context);
                throw;
            }
        }

        #endregion
    }
}
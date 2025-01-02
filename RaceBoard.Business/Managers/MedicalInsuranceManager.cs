using RaceBoard.Business.Managers.Abstract;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;
using RaceBoard.Translations.Interfaces;

namespace RaceBoard.Business.Managers
    {
        public class MedicalInsuranceManager : AbstractManager, IMedicalInsuranceManager
        {
            private readonly IMedicalInsuranceRepository _medicalInsuranceRepository;

            #region Constructors

            public MedicalInsuranceManager
                (
                    IMedicalInsuranceRepository medicalInsuranceRepository,
                    ITranslator translator,
                    IRequestContextManager requestContextManager
                ) : base(requestContextManager, translator)
            {
                _medicalInsuranceRepository = medicalInsuranceRepository;
            }

            #endregion

            #region IMedicalInsuranceManager implementation

            public PaginatedResult<MedicalInsurance> Get(MedicalInsuranceSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
            {
                return _medicalInsuranceRepository.Get(searchFilter, paginationFilter, sorting, context);
            }

            #endregion

            #region Private Methods

            #endregion
        }
    }

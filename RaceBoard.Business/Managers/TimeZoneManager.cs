using RaceBoard.Business.Managers.Abstract;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Data;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Translations.Interfaces;
using TimeZone = RaceBoard.Domain.TimeZone;

namespace RaceBoard.Business.Managers
{
    public class TimeZoneManager : AbstractManager, ITimeZoneManager
    {
        private readonly ITimeZoneRepository _timeZoneRepository;

        #region Constructors

        public TimeZoneManager
            (
                ITimeZoneRepository timeZoneRepository,
                IRequestContextManager requestContextManager,
                ITranslator translator
            ) : base(requestContextManager, translator)
        {
            _timeZoneRepository = timeZoneRepository;
        }

        #endregion

        #region ITimeZoneManager implementation

        public List<TimeZone> Get(ITransactionalContext? context = null)
        {
            return _timeZoneRepository.Get(context);
        }

        #endregion

        #region Private Methods

        #endregion
    }
}
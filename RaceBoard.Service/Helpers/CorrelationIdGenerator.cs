using RaceBoard.Common;
using RaceBoard.Service.Helpers.Interfaces;

namespace RaceBoard.Service.Helpers
{
    public class CorrelationIdGenerator : ICorrelationIdGenerator
    {
        private string _correlationId;

        public string Get()
        {
            if (_correlationId == null)
                _correlationId = IdGenerator.BuildUniqueId();

            return _correlationId;
        }

        public void Set(string correlationId)
        {
            _correlationId = correlationId;
        }
    }
}

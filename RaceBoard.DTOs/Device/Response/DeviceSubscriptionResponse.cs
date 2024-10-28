using RaceBoard.DTOs.Competition.Response;
using RaceBoard.DTOs.RaceClass.Response;

namespace RaceBoard.DTOs.Device.Response
{
    public class DeviceSubscriptionResponse
    {
        public CompetitionSimpleResponse Competition { get; set; }
        public RaceClassResponse[] RaceClasses { get; set; }
    }
}
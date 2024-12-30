using RaceBoard.DTOs.Championship.Response;
using RaceBoard.DTOs.RaceClass.Response;

namespace RaceBoard.DTOs.Device.Response
{
    public class DeviceSubscriptionResponse
    {
        public ChampionshipSimpleResponse Championship { get; set; }
        public RaceClassResponse[] RaceClasses { get; set; }
    }
}
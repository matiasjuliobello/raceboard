namespace RaceBoard.Domain
{
    public class DeviceSubscription
    {
        public Device Device { get; set; }
        public Championship Championship { get; set; }
        public List<RaceClass> RaceClasses { get; set; }

        public DeviceSubscription()
        {
            RaceClasses = new List<RaceClass>();
        }
    }
}

namespace RaceBoard.Domain
{
    public class DeviceSubscription
    {
        public Device Device { get; set; }
        public Competition Competition { get; set; }
        public List<RaceClass> RaceClasses { get; set; }

        public DeviceSubscription()
        {
            RaceClasses = new List<RaceClass>();
        }
    }
}

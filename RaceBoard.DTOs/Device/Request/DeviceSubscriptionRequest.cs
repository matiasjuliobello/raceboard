namespace RaceBoard.DTOs.Device.Request
{
    public class DeviceSubscriptionRequest
    {
        public int Id { get; set; }
        public int IdDevice { get; set; }
        public int IdChampionship { get; set; }
        public int[] IdsRaceClass {  get; set; }
    }
}

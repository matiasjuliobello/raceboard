using RaceBoard.DTOs.Person.Response;

namespace RaceBoard.DTOs.Boat.Response
{
    public class BoatOwnerResponse
    {
        public int Id { get; set; }
        public BoatResponse Boat { get; set; }
        public PersonSimpleResponse Person { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public bool IsActive { get; set; }
    }
}

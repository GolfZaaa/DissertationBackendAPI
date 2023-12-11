using BackendAPI.Models;

namespace BackendAPI.DTOs.ReservationsDtos
{
    public class ReservationsDto
    {
        public int Id { get; set; }
        //public DateTime DateTimeCreateReservations { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int CountPeople { get; set; }
        //public int Price { get; set; }
        public string UserId { get; set; }
        public int LocationId { get; set; }
    }
}

using System.ComponentModel.DataAnnotations.Schema;

namespace BackendAPI.Models
{
    public class Room
    {
        public int Id { get; set; }
        public string RoomsName { get; set; }
        public int Capacity { get; set; }
        public string Image { get; set; }
        public int StatusRooms { get; set; }
        public int CategoryId { get; set; }
    }
}

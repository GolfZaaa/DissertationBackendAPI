using System.ComponentModel.DataAnnotations.Schema;

namespace BackendAPI.Models
{
    public class Location
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Capacity { get; set; }
        public string Image { get; set; }
        public string PlaceDescription { get; set; }
        public int Status { get; set; }
        public int CategoryId { get; set; }
    }
}

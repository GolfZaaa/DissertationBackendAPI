namespace BackendAPI.Models
{
    public class LocationImages
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public int LocationId { get; set; }
        public Location Location { get; set; }
    }
}

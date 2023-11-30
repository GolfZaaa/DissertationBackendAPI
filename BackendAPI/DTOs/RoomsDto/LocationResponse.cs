using BackendAPI.Models;

namespace BackendAPI.DTOs.RoomsDto;
    public class LocationResponse
    {
    public int Id { get; set; }
    public string LocationName { get; set; }
        public int Capacity { get; set; }
        public string Image { get; set; }
        public string PlaceDescription { get; set; }
        public int CategoryId { get; set; }
        public List<string> LocationmultiImages { get; set; }

    static public LocationResponse FromLocation(Location location)
    {
        return new LocationResponse
        {
            Id = location.Id,
            LocationName = location.Name,
            Capacity = location.Capacity,
            Image = location.Image,
            PlaceDescription = location.PlaceDescription,
            CategoryId = location.CategoryId,
            LocationmultiImages = location.locationImages.Select(x => x.Image).ToList()
        };
    }

}

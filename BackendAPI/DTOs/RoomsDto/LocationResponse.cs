using BackendAPI.Models;

namespace BackendAPI.DTOs.RoomsDto;
    public class LocationResponse
    {
    public int Id { get; set; }
    public string Name { get; set; }
        public int Capacity { get; set; }
        public string Image { get; set; }
        public string PlaceDescription { get; set; }
        public CategoryLocations Category { get; set; }
        public List<string> LocationmultiImages { get; set; }

    static public LocationResponse FromLocation(Location location)
    {
        if (location == null)
        {
            return null;
        }

        //if (location.Category != null)
        //{
        //    var category = new CategoryLocations
        //    {
        //        Name = location.Category.Name
        //    };
        //}
        var response = new LocationResponse
        {
            Id = location.Id,
            Name = location.Name,
            Capacity = location.Capacity,
            Image = location.Image,
            PlaceDescription = location.PlaceDescription,
            Category = location.Category,
            LocationmultiImages = location.locationImages.Select(x => x.Image).ToList()
        };

    

        return response;
    }

}

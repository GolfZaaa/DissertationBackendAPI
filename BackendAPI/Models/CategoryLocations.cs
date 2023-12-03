using System.Text.Json.Serialization;

namespace BackendAPI.Models;
public class CategoryLocations
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Servicefees { get; set; }
    public DateTime DateTimeCreate { get; set; }
    [JsonIgnore]
    public ICollection<Location> Locations { get; set; }
}


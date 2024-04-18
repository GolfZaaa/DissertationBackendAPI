namespace BackendAPI.Models;
public class CartItem
{
    public int Id { get; set; }
    public Location Locations { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int CountPeople { get; set; }
    public string Objectives { get; set; }
    public bool Selected { get; set; }

}



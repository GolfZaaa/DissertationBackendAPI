namespace BackendAPI.Models;
public class CartItem
{
    //public int Id { get; set; }
    //public Location Locations { get; set; }
    //public double TotalHour { get; set; }
    //public int TotalPrice { get; set; }

    public int Id { get; set; }
    public Location Locations { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int CountPeople { get; set; }

}



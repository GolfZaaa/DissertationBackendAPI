namespace BackendAPI.Models;
public class ReservationCartItem
{
    public int Id { get; set; }
    public Location Locations { get; set; }
    public double TotalHour { get; set; }
    public int TotalPrice { get; set; }
}



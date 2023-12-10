namespace BackendAPI.Models;
    public class ReservationCart
    {
    public int Id { get; set; }
    public List<ReservationCartItem> Items { get; set; } = new List<ReservationCartItem>();
    public ApplicationUser User { get; set; }
    }

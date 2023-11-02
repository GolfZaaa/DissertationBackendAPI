namespace BackendAPI.Models;
public class LoginAttempt
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public string UserId { get; set; }
    public DateTime DateTimeLogin { get; set; }
    public int CountTimeLogin { get; set; }
}

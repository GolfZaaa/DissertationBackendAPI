namespace BackendAPI.Models;
public class Agency
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime CreatedDate { get; set; }
    public int StatusOnOff { get; set; } = 1;

}

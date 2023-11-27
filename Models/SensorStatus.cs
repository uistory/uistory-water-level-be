namespace StatusApi.Models;

public class SensorStatus
{
    public int Id { get; set; }
    public int Distance { get; set; }
    public double TankContentLiters { get; set; }
    public string? Unit { get; set; }
    public DateTime CreatedAt { get; set; }
}
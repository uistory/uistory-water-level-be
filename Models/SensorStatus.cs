namespace StatusApi.Models;
using Microsoft.AspNetCore.Identity;

public class SensorStatus
{
    public int Id { get; set; }
    public int Distance { get; set; }
    public double TankContentLiters { get; set; }
    public string? Unit { get; set; }
    public DateTime CreatedAt { get; set; }

    // Foreign key for the user
    public string UserId { get; set; }
    public IdentityUser User { get; set; }
}
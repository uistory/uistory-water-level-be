using StatusApi.Models;
using water_level_dotnetcore_api.Data;

public class SensorStatusService : ISensorStatusService
{
    private readonly DataContext _context;

    public SensorStatusService(DataContext context)
    {
        _context = context;
    }

    public double CalculateTankContentLiters(double distance)
    {
        int tankSize = 2000; //mm
        int tankCapacity = 12000; //liters
        return (1 - (double)(distance / tankSize)) * tankCapacity;
    }

    public async Task<double> CalculateRecentlyUsedLiters(List<SensorStatus> filteredItems)
    {
        var last = filteredItems.LastOrDefault();
        var first = filteredItems.FirstOrDefault();
        double totalWaterConsumed = 0;
        if (last != null && first != null)
        {
            totalWaterConsumed = last.TankContentLiters - first.TankContentLiters;
        }
        return totalWaterConsumed;
    }
}

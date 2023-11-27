namespace StatusApi.Models;

public interface ISensorStatusService
{
    double CalculateTankContentLiters(double distance);
    Task<double> CalculateRecentlyUsedLiters(List<SensorStatus> filteredItems);
}
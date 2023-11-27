using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StatusApi.Models;
using System;
using System.Threading.Tasks;
using water_level_dotnetcore_api.Data;

namespace WaterLevelBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SensorStatusController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly ILogger<SensorStatusController> _logger;
        private readonly ISensorStatusService _sensorStatusService;

        public SensorStatusController(DataContext context, ILogger<SensorStatusController> logger, ISensorStatusService sensorStatusService)
        {
            _context = context;
            _logger = logger;
            _sensorStatusService = sensorStatusService;
        }

        // GET: api/SensorStatus/SensorStatusItemsList
        [HttpGet("list")]
        public async Task<ActionResult<List<SensorStatus>>> GetSensorStatusItemsListAsync()
        {
            return await _context.SensorStatusItems.ToListAsync();
        }

        // GET: api/SensorStatus/LastSensorStatusItem
        [HttpGet]
        public async Task<ActionResult<SensorStatus>> GetLastSensorStatusItemAsync()
        {
            try
            {
                var item = await _context.SensorStatusItems.OrderBy(c => c.CreatedAt).LastOrDefaultAsync();
                return item;
            }
            catch (Exception error)
            {
                return BadRequest($"An error occurred: {error.Message}");

            }

        }

        [HttpGet("dashboard")]
        public async Task<ActionResult<DashboardData>> GetDashboardDataAsync()
        {
            try
            {
                var dashboard = new DashboardData();
                var item = await _context.SensorStatusItems.OrderBy(c => c.CreatedAt).LastOrDefaultAsync();
                dashboard.sensorStatus = item;

                DateTime today = DateTime.UtcNow.Date;
                DateTime yesterday = today.AddDays(-1);
                var filteredItems = await _context.SensorStatusItems
                .Where(x => (x.CreatedAt.Year == yesterday.Year
                        && x.CreatedAt.Month == yesterday.Month
                        && x.CreatedAt.Day >= yesterday.Day) && x.CreatedAt.Year == today.Year
                        && x.CreatedAt.Month == today.Month
                        && x.CreatedAt.Day <= today.Day)
                        .ToListAsync();

                dashboard.RecentlyUsedLiters = await _sensorStatusService.CalculateRecentlyUsedLiters(filteredItems);
                return dashboard;
            }
            catch (Exception error)
            {
                return BadRequest($"An error occurred: {error.Message}");

            }

        }

        // GET: api/SensorStatus/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SensorStatus>> GetItemAsync(int id)
        {
            var item = await _context.SensorStatusItems.FindAsync(id);

            if (item == null)
            {
                return NotFound();
            }

            return item;
        }

        // POST: api/SensorStatus
        [HttpPost]
        public async Task<ActionResult<SensorStatus>> PostSensorStatusAsync(SensorStatus item)
        {
            item.CreatedAt = DateTime.UtcNow;
            item.TankContentLiters = _sensorStatusService.CalculateTankContentLiters(item.Distance);
            _logger.LogInformation($"Received sensor status item with Id: {item.Id}");
            _context.SensorStatusItems.Add(item);
            await _context.SaveChangesAsync();
            return await GetItemAsync(item.Id);
        }

        // DELETE: api/SensorStatus/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItemAsync(int id)
        {
            var item = await _context.SensorStatusItems.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            _context.SensorStatusItems.Remove(item);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/SensorStatus/DeleteRecordsFromToday
        [HttpDelete("DeleteRecordsFromToday")]
        public async Task<IActionResult> DeleteRecordsFromTodayAsync()
        {
            try
            {
                // Get today's date in UTC
                DateTime today = DateTime.UtcNow.Date;
                // Find and delete all records created today
                var recordsToDelete = await _context.SensorStatusItems
                    .Where(x => x.CreatedAt.Year == today.Year
                        && x.CreatedAt.Month == today.Month
                        && x.CreatedAt.Day == today.Day)
                        .ToListAsync();

                if (recordsToDelete == null || recordsToDelete.Count == 0)
                {
                    // No records found for today
                    return NotFound("No records found for today");
                }

                _context.SensorStatusItems.RemoveRange(recordsToDelete);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception error)
            {
                return BadRequest($"An error occurred: {error.Message}");
            }
        }
    }
}

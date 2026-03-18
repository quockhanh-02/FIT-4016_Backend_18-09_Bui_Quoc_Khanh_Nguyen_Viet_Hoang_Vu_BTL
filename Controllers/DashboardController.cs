using Microsoft.AspNetCore.Mvc;
using FleetManagement.Data;
using System.Linq;

namespace FleetManagement.Controllers
{
    [Route("api/[controller]")] 
    // Route: api/Dashboard

    [ApiController] 
    // Đánh dấu API controller
    public class DashboardController : ControllerBase
    {
        private readonly AppDbContext _context; 
        // DbContext để truy vấn database

        public DashboardController(AppDbContext context)
        {
            _context = context; 
            // Inject DbContext
        }

        [HttpGet("stats")] 
        // GET: api/Dashboard/stats
        public IActionResult GetStats()
        {
            var vehicles = _context.Vehicles.Count(); 
            // Đếm tổng số xe

            var drivers = _context.Drivers.Count(); 
            // Đếm tổng số tài xế

            var trips = _context.Trips.Count(); 
            // Đếm tổng số chuyến đi

            var accidents = _context.Accidents.Count(); 
            // Đếm tổng số tai nạn

            // Lấy tổng số lít xăng (tránh null bằng cách ép kiểu nullable)
            var fuel = _context.FuelLogs.Sum(f => (double?)f.Liters) ?? 0;

            return Ok(new
            {
                vehicles, 
                // Tổng xe

                drivers, 
                // Tổng tài xế

                trips, 
                // Tổng chuyến

                accidents, 
                // Tổng tai nạn

                fuel 
                // Tổng số lít xăng
            });
        }
    }
}
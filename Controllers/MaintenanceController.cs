using FleetManagement.Data;
using FleetManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FleetManagement.Controllers
{
    [ApiController] 
    // Đánh dấu đây là API Controller

    [Route("api/[controller]")] 
    // Route: api/Maintenance
    public class MaintenanceController : ControllerBase
    {
        private readonly AppDbContext _context; 
        // DbContext để thao tác database

        public MaintenanceController(AppDbContext context)
        {
            _context = context; 
            // Inject DbContext
        }

        // =========================
        // GET ALL
        // =========================
        [HttpGet] 
        // GET: api/Maintenance
        public async Task<ActionResult<IEnumerable<Maintenance>>> GetAll()
        {
            var data = await _context.Maintenances
                .Include(m => m.Vehicle) 
                // Load thêm thông tin xe

                .ToListAsync();

            return Ok(data); 
            // Trả về danh sách bảo trì
        }

        // =========================
        // CREATE
        // =========================
        [HttpPost] 
        // POST: api/Maintenance
        public async Task<ActionResult> Create(Maintenance maintenance)
        {
            // Kiểm tra xe có tồn tại không
            var vehicleExists = await _context.Vehicles
                .AnyAsync(v => v.Id == maintenance.VehicleId);

            if (!vehicleExists)
                return BadRequest("Vehicle not found"); 
                // Nếu không có xe → báo lỗi

            _context.Maintenances.Add(maintenance); 
            // Thêm bản ghi bảo trì

            await _context.SaveChangesAsync(); 
            // Lưu vào DB

            return Ok(maintenance); 
            // Trả về dữ liệu vừa tạo
        }

        // =========================
        // DELETE
        // =========================
        [HttpDelete("{id}")] 
        // DELETE: api/Maintenance/5
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.Maintenances.FindAsync(id); 
            // Tìm bản ghi theo ID

            if (item == null)
                return NotFound(); 
                // Không tồn tại → 404

            _context.Maintenances.Remove(item); 
            // Xóa

            await _context.SaveChangesAsync(); 
            // Lưu

            return NoContent(); 
            // 204 thành công
        }
    }
}
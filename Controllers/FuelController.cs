using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FleetManagement.Data;
using FleetManagement.Models;
using System.Collections.Generic;
using System.Linq;

namespace FleetManagement.Controllers
{
    [Route("api/[controller]")] 
    // Route: api/FuelLogs

    [ApiController] 
    // Đánh dấu API controller
    public class FuelLogsController : ControllerBase
    {
        private readonly AppDbContext _context; 
        // DbContext để thao tác DB

        public FuelLogsController(AppDbContext context)
        {
            _context = context; 
            // Inject DbContext
        }

        // =========================
        // GET: api/FuelLogs
        // =========================
        [HttpGet]
        public ActionResult<IEnumerable<FuelLog>> GetFuelLogs()
        {
            return _context.FuelLogs
                .OrderByDescending(f => f.FuelDate) 
                // Sắp xếp theo ngày mới nhất

                .ToList(); 
                // Lấy danh sách
        }

        // =========================
        // GET: api/FuelLogs/5
        // =========================
        [HttpGet("{id}")]
        public ActionResult<FuelLog> GetFuelLog(int id)
        {
            var fuelLog = _context.FuelLogs.Find(id); 
            // Tìm theo ID

            if (fuelLog == null)
                return NotFound(); 
                // Không tồn tại → 404

            return fuelLog; 
            // Trả dữ liệu
        }

        // =========================
        // POST: api/FuelLogs
        // =========================
        [HttpPost]
        public ActionResult<FuelLog> CreateFuelLog(FuelLog fuelLog)
        {
            if (fuelLog == null)
                return BadRequest(); 
                // Dữ liệu null

            // Nếu chưa có ngày → set thời gian hiện tại
            if (fuelLog.FuelDate == default)
                fuelLog.FuelDate = DateTime.Now;

            _context.FuelLogs.Add(fuelLog); 
            // Thêm vào DB

            _context.SaveChanges(); 
            // Lưu

            return Ok(fuelLog); 
            // Trả về object vừa tạo
        }

        // =========================
        // PUT: api/FuelLogs/5
        // =========================
        [HttpPut("{id}")]
        public IActionResult UpdateFuelLog(int id, FuelLog fuelLog)
        {
            if (id != fuelLog.Id)
                return BadRequest(); 
                // ID không khớp

            var existing = _context.FuelLogs.Find(id); 
            // Tìm bản ghi

            if (existing == null)
                return NotFound(); 
                // Không tồn tại

            // Cập nhật dữ liệu
            existing.VehicleId = fuelLog.VehicleId; 
            // Xe

            existing.Liters = fuelLog.Liters; 
            // Số lít

            existing.TotalCost = fuelLog.TotalCost; 
            // Chi phí

            existing.FuelDate = fuelLog.FuelDate; 
            // Ngày đổ

            _context.SaveChanges(); 
            // Lưu

            return Ok(existing); 
            // Trả dữ liệu sau update
        }

        // =========================
        // DELETE: api/FuelLogs/5
        // =========================
        [HttpDelete("{id}")]
        public IActionResult DeleteFuelLog(int id)
        {
            var fuelLog = _context.FuelLogs.Find(id); 
            // Tìm theo ID

            if (fuelLog == null)
                return NotFound(); 
                // Không tồn tại

            _context.FuelLogs.Remove(fuelLog); 
            // Xóa

            _context.SaveChanges(); 
            // Lưu

            return Ok(); 
            // Thành công
        }
    }
}
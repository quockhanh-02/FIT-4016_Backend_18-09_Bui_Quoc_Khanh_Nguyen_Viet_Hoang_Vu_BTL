using FleetManagement.Data;
using FleetManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FleetManagement.Controllers;

[ApiController] 
// Đánh dấu đây là API Controller (tự động validate, binding dữ liệu)
[Route("api/[controller]")] 
// Route: api/Vehicles
public class VehiclesController : ControllerBase
{
    private readonly AppDbContext _context; 
    // Biến dùng để truy cập database

    public VehiclesController(AppDbContext context)
    {
        _context = context; 
        // Inject DbContext từ Dependency Injection
    }

    // =========================
    // GET ALL
    // =========================
    [HttpGet] 
    // GET: api/Vehicles
    public async Task<ActionResult<IEnumerable<Vehicle>>> GetAll()
    {
        return await _context.Vehicles.ToListAsync(); 
        // Lấy toàn bộ danh sách xe từ DB
    }

    // =========================
    // GET BY ID
    // =========================
    [HttpGet("{id}")] 
    // GET: api/Vehicles/5
    public async Task<ActionResult<Vehicle>> GetById(int id)
    {
        var vehicle = await _context.Vehicles.FindAsync(id); 
        // Tìm xe theo ID

        if (vehicle == null)
            return NotFound("Vehicle not found"); 
            // Nếu không có → trả về 404

        return vehicle; 
        // Trả về dữ liệu xe
    }

    // =========================
    // CREATE
    // =========================
    [HttpPost] 
    // POST: api/Vehicles
    public async Task<ActionResult<Vehicle>> Create([FromBody] Vehicle vehicle)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState); 
            // Nếu dữ liệu không hợp lệ → trả lỗi

        _context.Vehicles.Add(vehicle); 
        // Thêm xe vào DB

        await _context.SaveChangesAsync(); 
        // Lưu thay đổi

        return CreatedAtAction(
            nameof(GetById), 
            // Trả về link GET theo ID

            new { id = vehicle.Id }, 
            // Truyền ID mới tạo

            vehicle 
            // Trả về object vừa tạo
        );
    }

    // =========================
    // UPDATE
    // =========================
    [HttpPut("{id}")] 
    // PUT: api/Vehicles/5
    public async Task<IActionResult> Update(int id, [FromBody] Vehicle updatedVehicle)
    {
        if (id != updatedVehicle.Id)
            return BadRequest("ID mismatch"); 
            // ID trên URL và body không khớp

        var existingVehicle = await _context.Vehicles.FindAsync(id); 
        // Tìm xe trong DB

        if (existingVehicle == null)
            return NotFound("Vehicle not found"); 
            // Không tồn tại → 404

        // Update allowed fields
        existingVehicle.LicensePlate = updatedVehicle.LicensePlate; 
        // Cập nhật biển số

        existingVehicle.FuelNorm = updatedVehicle.FuelNorm; 
        // Cập nhật định mức nhiên liệu

        await _context.SaveChangesAsync(); 
        // Lưu thay đổi

        return NoContent(); 
        // Trả về 204 (thành công nhưng không có dữ liệu)
    }

    // =========================
    // DELETE
    // =========================
    [HttpDelete("{id}")] 
    // DELETE: api/Vehicles/5
    public async Task<IActionResult> Delete(int id)
    {
        var vehicle = await _context.Vehicles.FindAsync(id); 
        // Tìm xe theo ID

        if (vehicle == null)
            return NotFound("Vehicle not found"); 
            // Không tồn tại → 404

        _context.Vehicles.Remove(vehicle); 
        // Xóa xe

        await _context.SaveChangesAsync(); 
        // Lưu thay đổi

        return NoContent(); 
        // Trả về 204
    }
}
using FleetManagement.Data;
using FleetManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FleetManagement.Controllers;

[ApiController] 
// Đánh dấu đây là API controller

[Route("api/[controller]")] 
// Route: api/Drivers
public class DriversController : ControllerBase
{
    private readonly AppDbContext _context; 
    // DbContext để thao tác database

    public DriversController(AppDbContext context)
    {
        _context = context; 
        // Inject DbContext
    }

    // ========================================
    // GET ALL
    // ========================================
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Driver>>> GetAll()
    {
        return Ok(await _context.Drivers.ToListAsync()); 
        // Lấy toàn bộ danh sách tài xế
    }

    // ========================================
    // GET BY ID
    // ========================================
    [HttpGet("{id}")]
    public async Task<ActionResult<Driver>> Get(int id)
    {
        var driver = await _context.Drivers.FindAsync(id); 
        // Tìm theo ID

        if (driver == null)
            return NotFound(new { message = "Driver not found" }); 
            // Không tồn tại → 404

        return Ok(driver); 
        // Trả dữ liệu
    }

    // ========================================
    // CREATE
    // ========================================
    [HttpPost]
    public async Task<ActionResult<Driver>> Create([FromBody] Driver driver)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState); 
            // Validate dữ liệu

        _context.Drivers.Add(driver); 
        // Thêm vào DB

        await _context.SaveChangesAsync(); 
        // Lưu

        return CreatedAtAction(nameof(Get), new { id = driver.Id }, driver); 
        // Trả về object vừa tạo + location
    }

    // ========================================
    // UPDATE
    // ========================================
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Driver driver)
    {
        if (id != driver.Id)
            return BadRequest(new { message = "ID mismatch" }); 
            // ID không khớp

        var existingDriver = await _context.Drivers.FindAsync(id); 
        // Tìm bản ghi

        if (existingDriver == null)
            return NotFound(new { message = "Driver not found" }); 
            // Không tồn tại

        // Cập nhật từng field
        existingDriver.FullName = driver.FullName; 
        // Tên tài xế

        existingDriver.LicenseNumber = driver.LicenseNumber; 
        // Số bằng lái

        existingDriver.LicenseExpiry = driver.LicenseExpiry; 
        // Hạn bằng lái

        try
        {
            await _context.SaveChangesAsync(); 
            // Lưu thay đổi
        }
        catch (DbUpdateException)
        {
            return StatusCode(500, new { message = "Database error" }); 
            // Lỗi DB
        }

        return NoContent(); 
        // 204 thành công
    }

    // ========================================
    // DELETE
    // ========================================
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var driver = await _context.Drivers.FindAsync(id); 
        // Tìm theo ID

        if (driver == null)
            return NotFound(new { message = "Driver not found" }); 
            // Không tồn tại

        _context.Drivers.Remove(driver); 
        // Xóa

        try
        {
            await _context.SaveChangesAsync(); 
            // Lưu
        }
        catch (DbUpdateException)
        {
            return StatusCode(500, new { message = "Cannot delete (data constraint exists)" }); 
            // Lỗi do ràng buộc khóa ngoại
        }

        return NoContent(); 
        // 204 thành công
    }
}
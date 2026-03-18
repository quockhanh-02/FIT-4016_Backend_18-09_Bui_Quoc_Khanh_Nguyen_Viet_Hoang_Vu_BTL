using FleetManagement.Data;
using FleetManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FleetManagement.Controllers
{
    [ApiController] 
    // Đánh dấu đây là API Controller

    [Route("api/[controller]")] 
    // Route: api/Trips
    public class TripsController : ControllerBase
    {
        private readonly AppDbContext _context; 
        // DbContext để thao tác database

        public TripsController(AppDbContext context)
        {
            _context = context; 
            // Inject DbContext
        }

        // ============================
        // GET ALL
        // ============================
        [HttpGet] 
        // GET: api/Trips
        public async Task<ActionResult<IEnumerable<Trip>>> GetAll()
        {
            var trips = await _context.Trips
                .Include(t => t.Vehicle) 
                // Load thêm thông tin xe

                .Include(t => t.Driver) 
                // Load thêm thông tin tài xế

                .ToListAsync();

            return Ok(trips); 
            // Trả về danh sách chuyến đi
        }

        // ============================
        // GET BY ID
        // ============================
        [HttpGet("{id}")] 
        // GET: api/Trips/5
        public async Task<ActionResult<Trip>> GetById(int id)
        {
            var trip = await _context.Trips
                .Include(t => t.Vehicle)
                .Include(t => t.Driver)
                .FirstOrDefaultAsync(t => t.Id == id); 
                // Tìm chuyến đi theo ID

            if (trip == null)
                return NotFound("Trip not found"); 
                // Không tìm thấy → 404

            return Ok(trip); 
            // Trả dữ liệu
        }

        // ============================
        // CREATE
        // ============================
        [HttpPost] 
        // POST: api/Trips
        public async Task<ActionResult<Trip>> Create([FromBody] Trip trip)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState); 
                // Validate dữ liệu đầu vào

            var vehicleExists = await _context.Vehicles
                .AnyAsync(v => v.Id == trip.VehicleId);
            // Kiểm tra xe có tồn tại không

            if (!vehicleExists)
                return BadRequest("Vehicle does not exist");

            var driverExists = await _context.Drivers
                .AnyAsync(d => d.Id == trip.DriverId);
            // Kiểm tra tài xế có tồn tại không

            if (!driverExists)
                return BadRequest("Driver does not exist");

            _context.Trips.Add(trip); 
            // Thêm chuyến đi

            await _context.SaveChangesAsync(); 
            // Lưu DB

            // Load lại navigation để trả về đầy đủ
            await _context.Entry(trip)
                .Reference(t => t.Vehicle)
                .LoadAsync();

            await _context.Entry(trip)
                .Reference(t => t.Driver)
                .LoadAsync();

            return CreatedAtAction(
                nameof(GetById), 
                new { id = trip.Id }, 
                trip
            );
        }

        // ============================
        // UPDATE
        // ============================
        [HttpPut("{id}")] 
        // PUT: api/Trips/5
        public async Task<IActionResult> Update(int id, [FromBody] Trip trip)
        {
            if (id != trip.Id)
                return BadRequest("ID mismatch"); 
                // ID không khớp

            var existingTrip = await _context.Trips.FindAsync(id); 
            // Tìm trong DB

            if (existingTrip == null)
                return NotFound("Trip not found");

            // Cập nhật dữ liệu
            existingTrip.StartLocation = trip.StartLocation; 
            // Điểm bắt đầu

            existingTrip.EndLocation = trip.EndLocation; 
            // Điểm kết thúc

            existingTrip.VehicleId = trip.VehicleId; 
            // Xe

            existingTrip.DriverId = trip.DriverId; 
            // Tài xế

            await _context.SaveChangesAsync(); 
            // Lưu

            return NoContent(); 
            // 204 thành công
        }

        // ============================
        // DELETE
        // ============================
        [HttpDelete("{id}")] 
        // DELETE: api/Trips/5
        public async Task<IActionResult> Delete(int id)
        {
            var trip = await _context.Trips.FindAsync(id); 
            // Tìm chuyến

            if (trip == null)
                return NotFound("Trip not found");

            _context.Trips.Remove(trip); 
            // Xóa

            await _context.SaveChangesAsync(); 
            // Lưu

            return NoContent(); 
            // 204
        }
    }
}
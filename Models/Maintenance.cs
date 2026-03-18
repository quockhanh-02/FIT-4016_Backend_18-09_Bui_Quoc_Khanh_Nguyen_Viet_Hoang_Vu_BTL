using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FleetManagement.Models
{
    public class Maintenance
    {
        public int Id { get; set; } 
        // Khóa chính của bảng Maintenance

        [Required] 
        public int VehicleId { get; set; } 
        // ID của xe (khóa ngoại)

        [Required] 
        public DateTime MaintenanceDate { get; set; } 
        // Ngày bảo trì

        [Required] 
        public decimal Cost { get; set; } 
        // Chi phí bảo trì

        [MaxLength(500)] 
        public string? Notes { get; set; } 
        // Ghi chú thêm (có thể null)

        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = "Pending"; 
        // Trạng thái: Pending (chưa xong) / Completed (đã xong)

        // =========================
        // Navigation (liên kết bảng)
        // =========================
        public Vehicle? Vehicle { get; set; } 
        // Đối tượng xe liên kết
    }
}
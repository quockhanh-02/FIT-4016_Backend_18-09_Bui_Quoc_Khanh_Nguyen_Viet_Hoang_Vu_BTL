using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FleetManagement.Models
{
    public class Vehicle
    {
        [Key] // Khóa chính của bảng
        public int Id { get; set; }

        [Required(ErrorMessage = "License plate is required")] // Bắt buộc nhập
        [MaxLength(20, ErrorMessage = "License plate cannot exceed 20 characters")] // Giới hạn độ dài
        public string LicensePlate { get; set; } = string.Empty; // Biển số xe

        [Range(0, 1000, ErrorMessage = "Fuel norm must be between 0 and 1000")] // Giới hạn giá trị
        public decimal FuelNorm { get; set; } // Định mức tiêu hao nhiên liệu

        // Vehicle type: "Car" or "Motorbike"
        [Required(ErrorMessage = "Vehicle type is required")] // Bắt buộc nhập
        [MaxLength(20)] // Giới hạn độ dài
        public string VehicleType { get; set; } = string.Empty; // Loại phương tiện

        // =========================
        // Navigation properties (liên kết bảng)
        // =========================

        public ICollection<FuelLog> FuelLogs { get; set; } = new List<FuelLog>(); 
        // Danh sách lịch sử đổ xăng của xe

        public ICollection<Trip> Trips { get; set; } = new List<Trip>(); 
        // Danh sách chuyến đi của xe

        public ICollection<Maintenance> Maintenances { get; set; } = new List<Maintenance>(); 
        // Danh sách bảo trì của xe
    }
}
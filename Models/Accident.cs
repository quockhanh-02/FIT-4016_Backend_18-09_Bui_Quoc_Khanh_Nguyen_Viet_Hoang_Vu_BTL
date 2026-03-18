using System;
using System.ComponentModel.DataAnnotations;

namespace FleetManagement.Models
{
    public class Accident
    {
        [Key] 
        public int Id { get; set; } 
        // Khóa chính của bảng Accident

        [Required] 
        public int VehicleId { get; set; } 
        // ID của xe (khóa ngoại)

        [Required] 
        public int DriverId { get; set; } 
        // ID của tài xế (khóa ngoại)

        [MaxLength(255)] 
        public string? Location { get; set; } 
        // Địa điểm xảy ra tai nạn

        public double DamageCost { get; set; } 
        // Chi phí thiệt hại (hiện đang dùng kiểu double)

        public string? Description { get; set; } 
        // Mô tả chi tiết tai nạn

        public DateTime AccidentDate { get; set; } 
        // Ngày xảy ra tai nạn

        // =========================
        // Navigation properties
        // =========================
        public Vehicle? Vehicle { get; set; } 
        // Liên kết tới xe

        public Driver? Driver { get; set; } 
        // Liên kết tới tài xế
    }
}
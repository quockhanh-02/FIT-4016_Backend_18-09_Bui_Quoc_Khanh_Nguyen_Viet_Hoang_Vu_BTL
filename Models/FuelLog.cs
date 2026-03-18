using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FleetManagement.Models
{
    public class FuelLog
    {
        [Key] 
        public int Id { get; set; } 
        // Khóa chính của bảng FuelLog

        [Required] 
        public int VehicleId { get; set; } 
        // ID của xe (khóa ngoại)

        [ForeignKey("VehicleId")] 
        public Vehicle? Vehicle { get; set; } 
        // Liên kết tới bảng Vehicle (navigation property)

        [Required] 
        public DateTime FuelDate { get; set; } 
        // Ngày đổ xăng

        [Required] 
        public int Odometer { get; set; } 
        // Số km trên công tơ tại thời điểm đổ xăng

        [Required] 
        [Column(TypeName = "decimal(18,2)")] 
        public decimal Liters { get; set; } 
        // Số lít xăng đã đổ

        [Required] 
        [Column(TypeName = "decimal(18,2)")] 
        public decimal TotalCost { get; set; } 
        // Tổng chi phí đổ xăng
    }
}
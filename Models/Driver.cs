using System.ComponentModel.DataAnnotations;

namespace FleetManagement.Models;

public class Driver
{
    public int Id { get; set; } 
    // Khóa chính của bảng Driver

    [Required] 
    public string FullName { get; set; } = string.Empty; 
    // Họ và tên tài xế (bắt buộc)

    [Required] 
    public string LicenseNumber { get; set; } = string.Empty; 
    // Số bằng lái (bắt buộc)

    public DateTime LicenseExpiry { get; set; } 
    // Ngày hết hạn bằng lái

    public ICollection<Trip>? Trips { get; set; } 
    // Danh sách các chuyến đi của tài xế (navigation property)
}
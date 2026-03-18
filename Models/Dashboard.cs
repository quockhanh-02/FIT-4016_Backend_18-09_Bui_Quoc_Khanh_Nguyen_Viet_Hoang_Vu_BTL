namespace FleetManagement.Models
{
    public class Dashboard
    {
        public int Vehicles { get; set; } 
        // Tổng số xe

        public int Drivers { get; set; } 
        // Tổng số tài xế

        public int Trips { get; set; } 
        // Tổng số chuyến đi

        public int Accidents { get; set; } 
        // Tổng số tai nạn

        public int FuelLogs { get; set; } 
        // Tổng số lần đổ xăng (số bản ghi)

        public double Fuel { get; set; }  
        // Tổng số lít xăng đã sử dụng
    }
}
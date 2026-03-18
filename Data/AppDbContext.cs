using Microsoft.EntityFrameworkCore;
using FleetManagement.Models;

namespace FleetManagement.Data
{
    public class AppDbContext : DbContext
    {
        // Constructor nhận options từ Program.cs (kết nối database)
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // =========================
        // Khai báo các bảng trong DB
        // =========================

        public DbSet<Vehicle> Vehicles { get; set; } 
        // Bảng Vehicles (danh sách xe)

        public DbSet<FuelLog> FuelLogs { get; set; } 
        // Bảng FuelLogs (lịch sử đổ xăng)

        public DbSet<Driver> Drivers { get; set; } 
        // Bảng Drivers (tài xế)

        public DbSet<Trip> Trips { get; set; } 
        // Bảng Trips (chuyến đi)

        public DbSet<Maintenance> Maintenances { get; set; } 
        // Bảng bảo trì xe

        public DbSet<User> Users { get; set; } 
        // Bảng người dùng (login)

        public DbSet<Accident> Accidents { get; set; } 
        // Bảng tai nạn (mới thêm)

        // =========================
        // Cấu hình quan hệ giữa các bảng
        // =========================
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // =========================
            // Trip - Vehicle (1-n)
            // =========================
            modelBuilder.Entity<Trip>()
                .HasOne(t => t.Vehicle) // 1 Trip thuộc 1 Vehicle
                .WithMany(v => v.Trips) // 1 Vehicle có nhiều Trip
                .HasForeignKey(t => t.VehicleId) // Khóa ngoại
                .OnDelete(DeleteBehavior.Restrict); 
                // Không cho xóa Vehicle nếu còn Trip

            // =========================
            // Trip - Driver (1-n)
            // =========================
            modelBuilder.Entity<Trip>()
                .HasOne(t => t.Driver) // 1 Trip thuộc 1 Driver
                .WithMany(d => d.Trips) // 1 Driver có nhiều Trip
                .HasForeignKey(t => t.DriverId)
                .OnDelete(DeleteBehavior.Restrict); 
                // Không cho xóa Driver nếu còn Trip

            // =========================
            // Maintenance - Vehicle (1-n)
            // =========================
            modelBuilder.Entity<Maintenance>()
                .HasOne(m => m.Vehicle) // 1 Maintenance thuộc 1 Vehicle
                .WithMany(v => v.Maintenances) // 1 Vehicle có nhiều Maintenance
                .HasForeignKey(m => m.VehicleId)
                .OnDelete(DeleteBehavior.Restrict); 
                // Không cho xóa xe nếu còn bảo trì

            // =========================
            // Accident - Vehicle (1-n)
            // =========================
            modelBuilder.Entity<Accident>()
                .HasOne(a => a.Vehicle) // 1 tai nạn thuộc 1 xe
                .WithMany() // Không khai báo collection ở Vehicle
                .HasForeignKey(a => a.VehicleId)
                .OnDelete(DeleteBehavior.Restrict); 
                // Không cho xóa xe nếu còn tai nạn

            // =========================
            // Accident - Driver (1-n)
            // =========================
            modelBuilder.Entity<Accident>()
                .HasOne(a => a.Driver) // 1 tai nạn thuộc 1 tài xế
                .WithMany() // Không khai báo collection ở Driver
                .HasForeignKey(a => a.DriverId)
                .OnDelete(DeleteBehavior.Restrict); 
                // Không cho xóa tài xế nếu còn tai nạn
        }
    }
}
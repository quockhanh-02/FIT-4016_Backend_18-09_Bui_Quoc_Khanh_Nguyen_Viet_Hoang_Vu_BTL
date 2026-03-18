namespace FleetManagement.Models
{
    public class User
    {
        public int Id { get; set; } 
        // Khóa chính của người dùng

        public string Username { get; set; } 
        // Tên đăng nhập

        public string Password { get; set; } 
        // Mật khẩu (hiện tại đang lưu plain text - chưa an toàn)

        public string Role { get; set; } 
        // Vai trò (ví dụ: Admin, User)
    }
}
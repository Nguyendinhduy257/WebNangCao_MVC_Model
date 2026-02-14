using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebNangCao_MVC_Model.Models
{
    [Table("Users")] // Tên bảng trong Database
    public class User
    {
        public int Id { get; set; }

        public string FullName { get; set; } = string.Empty; // Tương ứng với Register.Name

        public string Username { get; set; } = string.Empty;// Tương ứng với Register.Username

        public string Email { get; set; } = string.Empty;    // Tương ứng với Register.Email

        public string PasswordHash { get; set; } = string.Empty; // Tương ứng với Register.Password (Đã mã hóa)

        public string Role { get; set; } = "student"; // Tương ứng với Register.Role

        // Thêm trường này để quản lý thời gian tạo
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
using System.ComponentModel.DataAnnotations.Schema;
namespace WebNangCao_MVC_Model.Models
{
    [Table("UserGroups")] // Tên bảng trong Database, sử dụng thư viện System.ComponentModel.DataAnnotations.Schema để đặt tên bảng
    public class UserGroup
    {
        //đây là bảng trung gian nối giữa User và Group để quản lý mối quan hệ nhiều-nhiều
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public int GroupId { get; set; }
        public Group Group { get; set; } = null!;

        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
    }
}

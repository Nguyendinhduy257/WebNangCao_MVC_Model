using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebNangCao_MVC_Model.Models
{
    [Table("Groups")]
    public class Group
    {
        public int Id { get; set; }
        public string GroupName { get; set; } = string.Empty; // VD: "Lớp Tiếng Anh B2"
        public string Description { get; set; } = string.Empty;

        // Navigation properties
        public ICollection<UserGroup> UserGroups { get; set; } = new List<UserGroup>();
        // Nếu file Exam của bạn đã có IdGroup, bạn có thể thêm List<Exam> ở đây
    }
}
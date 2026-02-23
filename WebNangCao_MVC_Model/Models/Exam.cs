namespace WebNangCao_MVC_Model.Models
{
    public class Exam
    {
        public int Id { get; set; } // <--- Đảm bảo bạn có dòng này và viết đúng chữ Id
        public string Title { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime StartTime { get; set; }
        // ... các thuộc tính khác
    }
}

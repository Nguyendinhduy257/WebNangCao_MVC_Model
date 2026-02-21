namespace WebNangCao_MVC_Model.Models
{
    public class ExamResult
    {
        public int Id { get; set; }

        // Cần khớp kiểu dữ liệu với Id của bảng User/Student (int hoặc string)
        public int StudentId { get; set; }

        public int ExamId { get; set; }
        public double Score { get; set; } // Điểm số

        // ... các thuộc tính khác
    }
}

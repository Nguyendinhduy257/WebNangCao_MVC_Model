namespace WebNangCao_MVC_Model.Models
{
    public class ExamResult
    {
        public int Id { get; set; }

        // Cần khớp kiểu dữ liệu với Id của bảng User/Student (int hoặc string)
        public int StudentId { get; set; }

        public int ExamId { get; set; }
        public double Score { get; set; } // Điểm số
        public DateTime SubmitTime { get; set; }

        public User Student { get; set; }
        public Exam Exam { get; set; }
        public virtual ICollection<ExamResultDetail> ExamResultDetails { get; set; } = new List<ExamResultDetail>();
        // ... các thuộc tính khác
    }
}

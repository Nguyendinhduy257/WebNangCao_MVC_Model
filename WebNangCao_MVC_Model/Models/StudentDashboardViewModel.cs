namespace WebNangCao_MVC_Model.ViewModels
{
    public class StudentDashboardViewModel
    {
        public int TotalExams { get; set; }
        public double AverageScore { get; set; }
        public int CompletedExams { get; set; }
        public int UpcomingExams { get; set; }

        // Sau này bạn có thể thêm List<Exam> để hiển thị danh sách bài thi ở dưới
    }
}
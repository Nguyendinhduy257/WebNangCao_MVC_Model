using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WebNangCao_MVC_Model.Data;
using WebNangCao_MVC_Model.Models;
using WebNangCao_MVC_Model.ViewModels;
// Nhớ using thư viện chứa Models của bạn (Ví dụ: Exam, ExamResult...)
namespace WebNangCao_MVC_Model.Controllers
{
    // Tên class bắt buộc phải có chữ "Controller" ở cuối
    //[Authorize] //yêu cầu người dùng phải đăng nhập mới được truy cập vào các Action trong controller này
    public class StudentController : Controller
    {
        private readonly AppDbContext _context;
        public StudentController(AppDbContext context)
        {
            _context = context;
        }
        // Tên hàm (Action) phải TRÙNG với tên file View (Dashboard)
        public IActionResult Dashboard()
        {
            //lấy ID current user đang đăng nhập
            /*
            var userId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            if(string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account"); // Nếu không lấy được UserId, chuyển hướng về trang đăng nhập
            }
            int studentId = int.Parse(userId); // Chuyển UserId từ string sang int 
            */


            //Gán cứng ID của 1 sinh viên có sẵn trong Database để test
            int studentId = 1;


            //truy vấn database để lấy thông tin dashboard của sinh viên
            var model=new StudentDashboardViewModel();
            // Gán từng thuộc tính
            model.TotalExams = _context.Exams.Count(e => e.IsActive);
            model.CompletedExams = _context.ExamResults.Count(r => r.StudentId == studentId);

            // Lấy danh sách kết quả trước để tính điểm trung bình
            var results = _context.ExamResults.Where(r => r.StudentId == studentId);
            model.AverageScore = results.Any() ? Math.Round(results.Average(r => r.Score), 1) : 0;

            model.UpcomingExams = _context.Exams.Count(e => e.StartTime > DateTime.UtcNow);
            // Lệnh View() này sẽ tự động đi tìm file: Views/Student/Dashboard.cshtml
            return View(model);
        }
        // Hàm này chỉ dùng để tạo dữ liệu mẫu chạy thử
        public IActionResult SeedData()
        {
            // 1. Tạo dữ liệu cho bảng Exams (Bài thi)
            // Nếu bảng chưa có bài thi nào thì mới thêm vào để tránh bị trùng lặp khi F5 nhiều lần
            if (!_context.Exams.Any())
            {
                _context.Exams.AddRange(
                    // Bài 1: Đã diễn ra trong quá khứ (cách đây 10 ngày)
                    new Exam { Title = "Toán học Học kỳ 1", IsActive = true, StartTime = DateTime.UtcNow.AddDays(-10) },

                    // Bài 2: Sắp tới (cách hiện tại 5 ngày)
                    new Exam { Title = "Tiếng Anh giữa kỳ", IsActive = true, StartTime = DateTime.UtcNow.AddDays(5) },

                    // Bài 3: Sắp tới (cách hiện tại 2 ngày)
                    new Exam { Title = "Vật lý 15 phút", IsActive = true, StartTime = DateTime.UtcNow.AddDays(2) },

                    // Bài 4: Bài thi đã đóng (IsActive = false)
                    new Exam { Title = "Hóa học thử nghiệm", IsActive = false, StartTime = DateTime.UtcNow.AddDays(-20) }
                );
                _context.SaveChanges(); // Lưu xuống DB để EF Core tự tạo ID
            }

            // 2. Tạo dữ liệu cho bảng ExamResults (Kết quả thi)
            if (!_context.ExamResults.Any())
            {
                // Lấy bài thi đầu tiên trong Database ra
                var firstExam = _context.Exams.FirstOrDefault();

                if (firstExam != null)
                {
                    _context.ExamResults.Add(
                        // Giả lập: Sinh viên ID = 1 đã thi bài này và được 8.5 điểm
                        new ExamResult { StudentId = 1, ExamId = firstExam.Id, Score = 8.5 }
                    );
                    _context.SaveChanges();
                }
            }

            return Content("🎉 Đã bơm dữ liệu mẫu vào PostgreSQL thành công! Bạn hãy sửa URL quay lại /Student/Dashboard để xem kết quả nhé.");
        }
    }
}
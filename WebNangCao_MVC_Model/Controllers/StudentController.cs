using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WebNangCao_MVC_Model.Data;
using WebNangCao_MVC_Model.Models;
using WebNangCao_MVC_Model.ViewModels;
using System.Security.Claims; //Dùng để lấy thông tin UserId từ Claims khi đã đăng nhập
using Microsoft.AspNetCore.Authentication; //Dùng để gọi SignInAsync, SignOutAsync khi đăng nhập/đăng xuất
using Microsoft.AspNetCore.Authentication.Cookies; //Dùng để gọi CookieAuthenticationDefaults
// Nhớ using thư viện chứa Models của bạn (Ví dụ: Exam, ExamResult...)
namespace WebNangCao_MVC_Model.Controllers
{
    // Tên class bắt buộc phải có chữ "Controller" ở cuối
    [Authorize] //yêu cầu người dùng phải đăng nhập mới được truy cập vào các Action trong controller này
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
            // ==========================================
            // 👤 LẤY ID CỦA SINH VIÊN ĐANG ĐĂNG NHẬP
            // ==========================================
            // Lệnh FindFirstValue sẽ tự động lôi cái NameIdentifier (chính là user.Id)
            // mà chúng ta đã cất vào Cookie ở bên AccountController ra.
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Bắt lỗi an toàn: Nếu vì lý do nào đó mất ID, đuổi về trang đăng nhập
            if (string.IsNullOrEmpty(userIdString))
            {
                return RedirectToAction("Index", "Account"); 
            }

            // Ép kiểu từ chuỗi sang số nguyên (int) để so sánh với Database
            int studentId = int.Parse(userIdString);


            // ==========================================
            // 📊 TRUY VẤN DỮ LIỆU THẬT TỪ DATABASE
            // ==========================================
            var model = new StudentDashboardViewModel();
            
            model.TotalExams = _context.Exams.Count(e => e.IsActive);
            
            // Giờ đây, truy vấn sẽ lấy ĐÚNG điểm của sinh viên đang đăng nhập (studentId)
            model.CompletedExams = _context.ExamResults.Count(r => r.StudentId == studentId);

            var results = _context.ExamResults.Where(r => r.StudentId == studentId);
            model.AverageScore = results.Any() ? Math.Round(results.Average(r => r.Score), 1) : 0;

            model.UpcomingExams = _context.Exams.Count(e => e.StartTime > DateTime.UtcNow);
            
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
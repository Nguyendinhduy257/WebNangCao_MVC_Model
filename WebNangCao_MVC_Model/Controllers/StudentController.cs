using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // Bắt buộc phải có để dùng Include() và ToListAsync()
using WebNangCao_MVC_Model.Data;
using WebNangCao_MVC_Model.Models;
using WebNangCao_MVC_Model.ViewModels;
using System.Security.Claims;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace WebNangCao_MVC_Model.Controllers
{
    [Authorize]
    public class StudentController : Controller
    {
        private readonly AppDbContext _context;

        public StudentController(AppDbContext context)
        {
            _context = context;
        }

        // Đổi thành async Task<IActionResult> để truy vấn Database mượt hơn
        public async Task<IActionResult> Dashboard()
        {
            // ==========================================
            // 1. LẤY ID CỦA SINH VIÊN ĐANG ĐĂNG NHẬP
            // ==========================================
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString))
            {
                return RedirectToAction("Index", "Account");
            }
            int studentId = int.Parse(userIdString);

            // ==========================================
            // 2. TÌM CÁC LỚP (GROUP) MÀ SINH VIÊN ĐANG THAM GIA
            // ==========================================
            // (Lấy ID các lớp mà sinh viên đang học thật trong Database):
            var studentGroupIds = await _context.UserGroups
                .Where(ug => ug.UserId == studentId)
                .Select(ug => ug.GroupId)
                .ToListAsync();

            // ==========================================
            // 3. LẤY BÀI THI & KẾT QUẢ TỪ DATABASE
            // ==========================================
            // Chỉ lấy các bài thi đang Active và thuộc về Group của sinh viên này
            var rawExams = await _context.Exams
                .Include(e => e.Questions) // Include để đếm tổng số câu hỏi
                .Where(e => e.IsActive && studentGroupIds.Contains(e.IdGroup))
                .ToListAsync();

            // Lấy kết quả thi của sinh viên này để biết bài nào đã làm rồi
            var userResults = await _context.ExamResults
                .Where(r => r.StudentId == studentId)
                .ToListAsync();
            var completedExamIds = userResults.Select(r => r.ExamId).ToList();

            // ==========================================
            // 4. MAP DỮ LIỆU SANG VIEW MODEL VÀ TÍNH TRẠNG THÁI
            // ==========================================
            var examListVM = new List<ExamItemViewModel>();

            // BẮT BUỘC: Dùng UtcNow để đồng bộ với thời gian PostgreSQL đang lưu
            var currentTime = DateTime.UtcNow;

            foreach (var exam in rawExams)
            {
                string status = "";

                // Logic phân loại trạng thái bài thi MỚI NHẤT
                if (completedExamIds.Contains(exam.Id))
                {
                    status = "Đã hoàn thành"; // Đã có điểm trong DB
                }
                else if (currentTime < exam.StartTime)
                {
                    status = "Sắp tới"; // Chưa tới giờ mở đề
                }
                // SỬ DỤNG exam.EndTime THAY VÌ CỘNG DURATION
                else if (currentTime >= exam.StartTime && currentTime <= exam.EndTime)
                {
                    status = "Có thể làm"; // Đang trong khung giờ cho phép thi
                }
                else
                {
                    status = "Đã hoàn thành"; // Đã qua hạn chót (EndTime)
                }

                examListVM.Add(new ExamItemViewModel
                {
                    Id = exam.Id,
                    Title = exam.Title,
                    StartTime = exam.StartTime,
                    Duration = exam.Duration,
                    TotalQuestions = exam.Questions?.Count ?? 0,
                    Status = status,
                    IdGroup = exam.IdGroup,
                    SubjectName = "Môn học " + exam.IdGroup, // Giả lập tên môn
                    Difficulty = "Trung bình" // Giả lập độ khó
                });
            }

            // ==========================================
            // 5. TỔNG HỢP VÀ GỬI RA VIEW
            // ==========================================
            var model = new StudentDashboardViewModel
            {
                TotalExams = rawExams.Count,
                CompletedExams = userResults.Count,
                UpcomingExams = examListVM.Count(e => e.Status == "Sắp tới"),
                AverageScore = userResults.Any() ? Math.Round(userResults.Average(r => r.Score), 1) : 0,
                Exams = examListVM.OrderBy(e => e.StartTime).ToList() // Sắp xếp bài thi theo thời gian
            };

            return View(model);
        }
        // ==========================================
        // HÀM BƠM DỮ LIỆU MẪU ĐÃ ĐƯỢC CẬP NHẬT CHUẨN XÁC
        // ==========================================
        public IActionResult SeedData()
        {
            if (!_context.Exams.Any())
            {
                var now = DateTime.UtcNow; // BẮT BUỘC DÙNG UTC

                _context.Exams.AddRange(
                    // Đang diễn ra (Có thể làm) - Thuộc Lớp 1
                    new Exam
                    {
                        Title = "Kiểm tra Toán học Học kỳ 1",
                        IsActive = true,
                        StartTime = now.AddMinutes(-10),
                        EndTime = now.AddDays(1), // THÊM ENDTIME VÀO ĐÂY
                        Duration = 60,
                        IdGroup = 1
                    },

                    // Sắp tới - Thuộc Lớp 2
                    new Exam
                    {
                        Title = "Tiếng Anh giữa kỳ",
                        IsActive = true,
                        StartTime = now.AddDays(2),
                        EndTime = now.AddDays(3), // THÊM ENDTIME VÀO ĐÂY
                        Duration = 90,
                        IdGroup = 2
                    },

                    // Đã làm xong (Quá hạn) - Thuộc Lớp 1
                    new Exam
                    {
                        Title = "Vật lý 15 phút",
                        IsActive = true,
                        StartTime = now.AddDays(-5),
                        EndTime = now.AddDays(-4), // THÊM ENDTIME VÀO ĐÂY
                        Duration = 15,
                        IdGroup = 1
                    }
                );
                _context.SaveChanges();
            }

            if (!_context.ExamResults.Any())
            {
                var pastExam = _context.Exams.FirstOrDefault(e => e.Title.Contains("Vật lý"));
                if (pastExam != null)
                {
                    _context.ExamResults.Add(
                        new ExamResult { StudentId = 1, ExamId = pastExam.Id, Score = 8.5 }
                    );
                    _context.SaveChanges();
                }
            }

            return Content("Đã bơm dữ liệu mẫu THÀNH CÔNG! Hãy truy cập lại /Student/Dashboard để xem các thẻ bài thi xịn xò nhé.");
        }
    }
}
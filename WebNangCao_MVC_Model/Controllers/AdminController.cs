using Microsoft.EntityFrameworkCore;
using WebNangCao_MVC_Model.Data;
using WebNangCao_MVC_Model.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
namespace WebNangCao_MVC_Model.Controllers
{
    //Phân quyền (chỉ có admin mới được vào phần này)
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        //Chìa khoá nội bộ của công ti Admin, chỉ có Admin được quyền truy cập
        private readonly AppDbContext _context;
        //DI (Dependancy Injection) để chọc thẳng vào database
        public AdminController(AppDbContext context) {
            _context = context;
        }
        //Action hiển thị màn hình Dashboard
        public async Task<IActionResult> Index() {
            var viewModel = new AdminDashboardViewModel();
            //Múc dữ liệu lên Dashboard
            var today = DateTime.UtcNow.Date;
            var sevenDaysAgo = today.AddDays(-7);
            //Đếm user đang hoạt động
            viewModel.TotalActiveUsers = await _context.Users.CountAsync(u => u.IsActive);
            //Đếm tổng số đề thi
            viewModel.TotalExams = await _context.Exams.CountAsync();
            //Đếm đê thi tạo trong hôm nay
            viewModel.TotalExamsToday = await _context.Exams.CountAsync(e => e.CreatedAt >= today);
            // 1. Khai báo danh sách các Role em muốn lọc
var targetRoles = new[] { "student", "teacher" };
// 1. Lấy danh sách 5 người ĐANG CHỜ DUYỆT (Để Admin bấm nút "Duyệt")
viewModel.PendingUsers = await _context.Users
    .Where(u => targetRoles.Contains(u.Role) && u.IsActive == false)
    .OrderByDescending(u => u.CreatedAt) // Ai mới đăng ký thì hiện lên đầu
    .Take(5)
    .ToListAsync();

// 2. Lấy danh sách 5 người ĐANG HOẠT ĐỘNG (Để Admin theo dõi)
// Chỗ này ta nên tạo thêm một biến List<User> ActiveUsers trong ViewModel nhé
viewModel.ActiveUsers = await _context.Users
    .Where(u => targetRoles.Contains(u.Role) && u.IsActive == true)
    .OrderByDescending(u => u.LastLoginAt) // CỰC KỲ QUAN TRỌNG: Ai vừa mới online thì hiện lên đầu
    .Take(5)
    .ToListAsync();
    // Lấy 10 log hoạt động gần nhất
    viewModel.RecentLogs = await _context.ActivityLogs
                .Include(l => l.User) 
                .OrderByDescending(l => l.Timestamp)
                .Take(10)
                .ToListAsync();
    viewModel.SystemHealthStatus = new SystemStatusDto
            {
                IsWebServerOnline = true, // Code chạy đến đây tức là Server đang sống
                IsDatabaseOnline = await _context.Database.CanConnectAsync(), // Ping thẳng xuống Postgres
                StorageUsagePercentage = 65.5m, // Cái này tạm hardcode, thực tế phải đọc từ DriveInfo
                BackupStatus = "Active"
            };

            // (Các chỉ số % tăng trưởng Weekly tạm thời để số 0 hoặc Hardcode, 
            // ta sẽ viết 1 hàm riêng xử lý logic tính toán phức tạp đó sau để Controller không bị rác)
            viewModel.WeeklyLogins = 120;
            viewModel.WeeklyLoginsGrowth = 12.5m;

            // 3. BƯNG MÂM RA CHO KHÁCH (View)
            return View(viewModel);
        }
    }
}
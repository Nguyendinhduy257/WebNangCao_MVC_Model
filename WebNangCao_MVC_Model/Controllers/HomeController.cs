using Microsoft.AspNetCore.Mvc;

namespace WebNangCao_MVC_Model.Controllers
{
    public class HomeController : Controller
    {
        // Action Index: Xử lý khi người dùng truy cập trang chủ ("/")
        // Tương đương với việc render component <LandingPage /> trong React
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        // Action GetStarted: Xử lý logic khi bấm nút "Bắt đầu ngay"
        // Thay thế cho prop onGetStarted trong React
        public IActionResult GetStarted()
        {
            // Tham số 1: Tên Action (Hàm) -> Phải là "Index"
            // Tham số 2: Tên Controller -> "Account"
            // Tham số 3: Dữ liệu truyền đi (Query String)
            return RedirectToAction("Index", "Account", new { activeTab = "login" });
        }
    }
}
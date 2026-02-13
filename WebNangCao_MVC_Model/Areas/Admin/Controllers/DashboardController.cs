using Microsoft.AspNetCore.Mvc;

namespace WebNangCao_MVC_Model.Areas.Admin.Controllers
{
    // CỰC KỲ QUAN TRỌNG: Phải có dòng này thì Code mới biết đây là khu vực Admin
    [Area("Admin")]
    // Route này để truy cập: domain.com/Admin/Dashboard
    //[Route("Admin/[controller]/[action]")]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
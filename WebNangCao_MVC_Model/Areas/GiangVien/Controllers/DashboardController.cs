using Microsoft.AspNetCore.Mvc;

// 1. Sửa Namespace cho đúng chuẩn
namespace WebNangCao_MVC_Model.Areas.GiangVien.Controllers
{
    [Area("GiangVien")]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
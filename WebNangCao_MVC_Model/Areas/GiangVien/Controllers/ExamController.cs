using Microsoft.AspNetCore.Mvc;

namespace WebNangCao_MVC_Model.Areas.GiangVien.Controllers
{
    public class ExamController : Controller
    {
        //Giảng Viên quản lý đ
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Create()
        {
            return View();
        }
    }
}

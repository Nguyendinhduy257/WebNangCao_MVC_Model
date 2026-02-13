using Microsoft.AspNetCore.Mvc;

namespace WebNangCao_MVC_Model.Areas.GiangVien.Controllers
{
    public class GroupController : Controller
    {
        //Giảng viên quản lý lớp học
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

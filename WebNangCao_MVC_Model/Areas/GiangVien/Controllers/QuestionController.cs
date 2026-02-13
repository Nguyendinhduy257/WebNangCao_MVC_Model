using Microsoft.AspNetCore.Mvc;

namespace WebNangCao_MVC_Model.Areas.GiangVien.Controllers
{
    public class QuestionController : Controller
    {
        //Giảng viên quản lý câu hỏi
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

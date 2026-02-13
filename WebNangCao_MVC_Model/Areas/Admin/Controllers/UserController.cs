using Microsoft.AspNetCore.Mvc;

namespace WebNangCao_MVC_Model.Areas.Admin.Controllers
{
    public class UserController : Controller
    {
        //Admin quản lý người dùng
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Edit()
        {
            return View();
        }
    }
}

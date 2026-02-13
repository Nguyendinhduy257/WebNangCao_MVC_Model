using Microsoft.AspNetCore.Mvc;

namespace WebNangCao_MVC_Model.Controllers
{
    public class TestAttemptController : Controller
    {
        //Sinh Vien làm đề thi
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult GiaoDienLamBai()
        {
            return View();
        }
    }
}

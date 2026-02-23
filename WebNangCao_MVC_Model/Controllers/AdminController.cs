using WebNangCao_MVC_Model.Models;
using Microsoft.AspNetCore.Mvc;
namespace WebNangCao_MVC_Model.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index() {
            return View();
        }
    }
}
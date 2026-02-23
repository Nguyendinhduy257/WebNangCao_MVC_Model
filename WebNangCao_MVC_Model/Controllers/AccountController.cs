using System.Linq;
using WebNangCao_MVC_Model.Data;
using FluentValidation;
using FluentValidation.Results; // Để dùng ValidationResult
using Microsoft.AspNetCore.Mvc;
using WebNangCao_MVC_Model.Models;

namespace WebNangCao_MVC_Model.Controllers
{
    public class AccountController : Controller
    {
        // Khai báo Validator
        private readonly IValidator<LoginViewModel> _loginValidator;
        private readonly IValidator<RegisterViewModel> _registerValidator;
        private readonly AppDbContext _context;

        // Constructor Injection
        public AccountController(IValidator<LoginViewModel> loginValidator, IValidator<RegisterViewModel> registerValidator, AppDbContext context)
        {
            _loginValidator = loginValidator;
            _registerValidator = registerValidator;
            _context = context;
        }

        //public IActionResult Index()
        //{
        //    return View(new AuthViewModels());
        //}

        // Thêm tham số mặc định activeTab = "login"
        public IActionResult Index(string activeTab = "login")
        {
            var model = new AuthViewModels();
            // Gán tab cần hiển thị vào Model
            model.ActiveTab = activeTab;
            return View(model);
        }
        public IActionResult Login()
        {
            return View(); // Nó sẽ tự tìm vào Views/Account/Login.cshtml
        }
        

        [HttpPost]
        public IActionResult Login(AuthViewModels model)
        {
            // 1. Gọi Validator thủ công cho phần Login
            ValidationResult result = _loginValidator.Validate(model.Login);

            if (!result.IsValid)
            {
                // Copy lỗi từ FluentValidation sang ModelState để View hiển thị
                foreach (var error in result.Errors)
                {
                    // Lưu ý key: "Login.UsernameOrEmail" để khớp với asp-for trong View
                    ModelState.AddModelError($"Login.{error.PropertyName}", error.ErrorMessage);
                }

                model.ActiveTab = "login";
                return View("Index", model);
            }

            // 2. Logic nghiệp vụ (Kiểm tra DB...)
            //THIS CODE SHOULD BE TERMINATED BROS
            /*if (model.Login.UsernameOrEmail == "admin" && model.Login.Password == "123456")
            {
                return RedirectToAction("Index", "Home");
            }*/
            var user = _context.Users.FirstOrDefault(u => 
            (u.Username == model.Login.UsernameOrEmail || u.Email == model.Login.UsernameOrEmail) 
            && u.PasswordHash == model.Login.Password
            );
            if (user != null)
            {
                return RedirectToAction("Index","Home");
            }
            else 
            {
                ModelState.AddModelError("Login.Password", "Tài khoản hoặc mật khẩu sai");
                model.ActiveTab = "login";
                return View("Index", model);
            }
        }

        [HttpPost]
        public IActionResult Register(AuthViewModels model)
        {
            // 1. Gọi Validator thủ công cho phần Register
            ValidationResult result = _registerValidator.Validate(model.Register);

            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                {
                    // Key: "Register.Name"
                    ModelState.AddModelError($"Register.{error.PropertyName}", error.ErrorMessage);
                }

                model.ActiveTab = "register";
                return View("Index", model);
            }

            // 2. Logic nghiệp vụ (Lưu vào DB...)
            return RedirectToAction("Index", "Home");
        }
    }
}
using FluentValidation;
using FluentValidation.Results; // Dùng để hứng kết quả trả về từ Validator
using Microsoft.AspNetCore.Mvc;
using WebNangCao_MVC_Model.Models;
using WebNangCao_MVC_Model.Data; //để gọi AppDBContext
using System.Linq; //Để dùng FirstOrDefault, Any

namespace WebNangCao_MVC_Model.Controllers
{
    public class AccountController : Controller
    {
        // 1. KHAI BÁO CÁC DEPENDENCY (DỊCH VỤ CẦN DÙNG TỪ HỆ THỐNG)
        private readonly IValidator<LoginViewModel> _loginValidator;
        private readonly IValidator<RegisterViewModel> _registerValidator;
        private readonly AppDbContext _context;

        //khai báo DB ConText
        // _context là kế thừa từ AppDbContext, có nhiệm vụ kết nối và thao tác với Database PostgreSQL

        //var user = _context.Users.FirstOrDefault(u => (u.Username == model.Login.UsernameOrEmail ||
        // u.Email == model.Login.UsernameOrEmail)
        //&& u.PasswordHash == model.Login.Password);
        // ----->_context.Users: truy cập vào bảng Users trong Database thông qua DbSet<User> Users đã khai báo trong AppDbContext
        // ----->FirstOrDefault: tìm kiếm bản ghi đầu tiên thỏa mãn

        private readonly AppDbContext _context;

        //bơm(Inject) DB ConText vào Constructor

        // 2. CONSTRUCTOR INJECTION
        // Hệ thống (thông qua file Program.cs) sẽ tự động "bơm" (inject) 2 cái Validator vào đây
        // 2 hàm loginValidator và registerValidator này sẽ được khởi tạo sẵn tại thư mục Validators, và được đăng ký trong Program.cs rồi nên ta chỉ việc hứng vào đây là xài được luôn
        public AccountController(
            IValidator<LoginViewModel> loginValidator,
            IValidator<RegisterViewModel> registerValidator,
            AppDbContext context
            )
        {
            _loginValidator = loginValidator;
            _registerValidator = registerValidator;
            _context = context; //gắn vào biến toàn cục của Controller
        }

        // KHU VỰC HIỂN THỊ GIAO DIỆN (GET)

        // Hàm này chạy khi người dùng gõ /Account/Index hoặc vào trang đăng nhập
        // Có tham số mặc định là activeTab = "login" để lúc nào mới vào cũng hiện tab Đăng nhập
        public IActionResult Index(string activeTab = "login")
        {
            var model = new AuthViewModels();
            model.ActiveTab = activeTab; // Gán trạng thái để View biết nên mở Tab nào
            return View(model);
        }

        // Nếu người dùng lỡ gõ trực tiếp /Account/Login lên thanh địa chỉ
        // Ta đẩy họ về lại hàm Index ở trên kèm theo tham số báo hiệu mở tab "login"
        [HttpGet]
        public IActionResult Login()
        {
            return RedirectToAction("Index", new { activeTab = "login" });
        }


        // KHU VỰC XỬ LÝ DỮ LIỆU GỬI LÊN (POST)

        [HttpPost]
        public IActionResult Login(AuthViewModels model, string Role)
        {
            // BƯỚC 1: KIỂM TRA LỖI NHẬP LIỆU (VALIDATION)
            ValidationResult result = _loginValidator.Validate(model.Login);

            if (!result.IsValid)
            {
                // Nếu có lỗi (để trống, sai định dạng...)
                foreach (var error in result.Errors)
                {
                    // Từ khóa $"Login.{error.PropertyName}" giúp C# map đúng vào thẻ <span asp-validation-for="Login..."> bên HTML
                    ModelState.AddModelError($"Login.{error.PropertyName}", error.ErrorMessage);
                }

                model.ActiveTab = "login"; // Giữ nguyên tab Đăng nhập
                return View("Index", model); // Trả lại giao diện kèm thông báo lỗi
            }

            //Bước 3: truy vấn PsstGreSQL để đăng nhập
            //tìm user có username hoặc email khớp với dữ liệu nhập vào
            //so sánh mật khẩu đúng
            var user = _context.Users.FirstOrDefault(u => (u.Username == model.Login.UsernameOrEmail || u.Email == model.Login.UsernameOrEmail)
                && u.PasswordHash == model.Login.Password);
            //nếu tìm thấy tài khoản hợp lệ
            if (user != null)
            {
                //lấy Role trực tiếp từ Database của user đó
                string currentRole = user.Role ?? "student"; //mặc định là student
                if (currentRole == "student")
                {
                    //Controller="Student", hàm trong controller = "Dashboard"
                    return RedirectToAction("Dashboard", "Student");
                }
                else if (currentRole == "instructor")
                {
                    //Controller="Instructor", hàm trong controller = "Dashboard"
                    return RedirectToAction("Dashboard", "Instructor");
                }
            }
            //nếu user==null(không tìm thấy trong DB)
            ModelState.AddModelError("login.Password", "Tài khoản hoặc mật khẩu không chính xác.");
            model.ActiveTab = "login";
            return View("Index", model);
        }

        [HttpPost]
        public IActionResult Register(AuthViewModels model)
        {
            // BƯỚC 1: KIỂM TRA LỖI FORM ĐĂNG KÝ
            ValidationResult result = _registerValidator.Validate(model.Register);

            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                {
                    // Tương tự, map lỗi vào thuộc tính Register (VD: Register.Name, Register.Email)
                    ModelState.AddModelError($"Register.{error.PropertyName}", error.ErrorMessage);
                }

                model.ActiveTab = "register"; // Giữ người dùng ở lại tab Đăng ký để họ sửa lỗi
                return View("Index", model);
            }

            //Bước 4: thêm tài khoản mới vào PostGreSQL
            bool isExist = _context.Users.Any(u => u.Username == model.Register.Username || u.Email == model.Register.Email);
            if (isExist)
            {
                ModelState.AddModelError("Register.Username", "Tên đăng nhập hoặc Email đã tồn tại!");
                model.ActiveTab = "register";
                return View("Index", model);
            }
            // Tạo đối tượng User mới từ dữ liệu form gửi lên
            var newUser = new User
            {
                FullName = model.Register.Name,
                Username = model.Register.Username,
                Email = model.Register.Email,
                PasswordHash = model.Register.Password, // ⚠️ Cảnh báo: Thực tế đi làm phải mã hóa (Hash) mật khẩu nhé!
                Role = model.Register.Role ?? "student"
            };
            //nếu chưa có tài khoản nào thì sẽ tạo và lưu dữ liệu vào Database
            // Lưu xuống Database
            _context.Users.Add(newUser);
            _context.SaveChanges();

            // Đăng ký xong, điều hướng vào thẳng Dashboard tương ứng
            if (newUser.Role == "student")
            {
                return RedirectToAction("Dashboard", "Student");
            }
            else
            {
                return RedirectToAction("Dashboard", "Instructor");
            }
        }
    }
}
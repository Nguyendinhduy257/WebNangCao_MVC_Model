using FluentValidation;
using FluentValidation.AspNetCore;
using WebNangCao_MVC_Model.Validators;
using WebNangCao_MVC_Model.Data;
using Microsoft.EntityFrameworkCore; //Dùng để cấu hình DbContext với PostgreSQL
using Microsoft.AspNetCore.Authentication.Cookies; //Dùng để cấu hình Cookie Authentication
//using static WebNangCao_MVC_Model.Validators.AuthValidators;
var builder = WebApplication.CreateBuilder(args);

//----Đọc chuỗi kết nối  Database từ file json----
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
//đăng ký dịch vụ DataBase với PostGreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// Add services to the container.
builder.Services.AddControllersWithViews(options => 
{
    // Bịt miệng thằng .NET, không cho nó tự động quăng lỗi tiếng Anh
    options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
});

//Cấu hình Cookie Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "WebNangCaoCookie";
        options.LoginPath = "/Account/Index"; // Nếu chưa đăng nhập mà lén vào trang cấm, sẽ bị đuổi về đây
        options.AccessDeniedPath = "/Account/AccessDenied"; // Đi sai luồng (VD Học viên ráng vào trang Giảng viên)
    });

//Cấu hình FluentValidation
builder.Services.AddFluentValidationAutoValidation(); // Tự động validate
builder.Services.AddFluentValidationClientsideAdapters(); // Hỗ trợ jQuery Unobtrusive (nếu dùng)
builder.Services.AddValidatorsFromAssemblyContaining<LoginValidator>(); // Tự động tìm tất cả validator trong cùng project


var app = builder.Build();

// gọi hàm NapDuLieuVaoDB.cs để nạp dữ liệu vào DB
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        //gọi hàm Seed có trong NapDuLieuDB.cs để nạp dữ liệu vào DB
        NapDuLieuVaoDB.Seed(context);
    }
    catch (Exception ex)
    {
        // Log lỗi nếu có
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Lỗi khi nạp dữ liệu vào DB.");
    }
}
    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }
    
    app.UseHttpsRedirection();

    app.UseStaticFiles();

    app.UseRouting();

    app.UseAuthentication(); // 1. Khám xét người dùng (Đọc thẻ Cookie xem là ai)
    app.UseAuthorization();  // 2. Kiểm tra quyền hạn (Có được phép vào trang này không)

    // 1. ƯU TIÊN CAO NHẤT: Route dành cho Areas (Admin, Teacher)
    // Phải đặt lên đầu để hệ thống check xem "Có phải đang vào Admin không?" trước
    app.MapControllerRoute(
        name: "areas",
        pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

    // 2. ƯU TIÊN THẤP HƠN: Route mặc định (Sinh viên/Trang chủ)
    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    app.Run();

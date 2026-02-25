using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;
using WebNangCao_MVC_Model.Data.Fluent_API; // Import namespace chứa UserConfiguration
using WebNangCao_MVC_Model.Models;
namespace WebNangCao_MVC_Model.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {   
        }
        // --- KHAI BÁO CÁC BẢNG (TABLES) ---
        //Đang xem trên Code trên Mermaid Diagram
        public DbSet<User> Users { get; set; } // Tương ứng với bảng "Users" trong Database
        public DbSet<Exam> Exams { get; set; }
        public DbSet<ExamResult> ExamResults { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
            base.OnModelCreating(modelBuilder);


            //// CÁCH 1: Kích hoạt thủ công từng file trong thư mục FluentAPI (Dùng cách này để dễ kiểm soát)
            //modelBuilder.ApplyConfiguration(new UserConfiguration());

            //// CÁCH 2: Kích hoạt TẤT CẢ file cấu hình trong project (Nhanh, gọn)
            //// modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
            ///
            // Dòng thần thánh này sẽ tự động tìm tất cả các class kế thừa IEntityTypeConfiguration
            // trong assembly hiện tại (bao gồm 3 file trong thư mục Fluent_API của bạn) và áp dụng chúng.
        }
    }
}

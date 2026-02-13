using Microsoft.EntityFrameworkCore;
using WebNangCao_MVC_Model.Models;
namespace WebNangCao_MVC_Model.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            // --- KHAI BÁO CÁC BẢNG (TABLES) ---
            //Đang xem trên Code trên Mermaid Diagram
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebNangCao_MVC_Model.Models;

namespace WebNangCao_MVC_Model.Data.Fluent_API
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // 1. Đặt tên bảng
            builder.ToTable("Users");

            // 2. Khóa chính
            builder.HasKey(x => x.Id);

            // 3. Cấu hình các cột
            builder.Property(x => x.FullName)
                .IsRequired()            // Tương đương [Required]
                .HasMaxLength(100);      // Tương đương [MaxLength(100)]

            builder.Property(x => x.Username)
                .IsRequired()
                .HasMaxLength(50);

            // Tạo chỉ mục DUY NHẤT (Không cho trùng Username)
            builder.HasIndex(x => x.Username).IsUnique();

            builder.Property(x => x.Email)
                .IsRequired()
                .HasMaxLength(150);

            // Tạo chỉ mục DUY NHẤT (Không cho trùng Email)
            builder.HasIndex(x => x.Email).IsUnique();

            builder.Property(x => x.PasswordHash)
                .IsRequired();

            builder.Property(x => x.Role)
                .IsRequired()
                .HasMaxLength(20)
                .HasDefaultValue("student"); // Giá trị mặc định trong SQL

            builder.Property(x => x.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP"); // Tự động lấy giờ server DB khi tạo mới
        }
    }
}
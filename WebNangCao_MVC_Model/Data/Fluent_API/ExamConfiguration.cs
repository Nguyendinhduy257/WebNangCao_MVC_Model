using WebNangCao_MVC_Model.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace WebNangCao_MVC_Model.Data.Fluent_API
{
    public class ExamConfiguration : IEntityTypeConfiguration<Exam>
    {
        public void Configure(EntityTypeBuilder<Exam> builder)
        {
            // Tên bảng
            builder.ToTable("Exams");

            // Khóa chính
            builder.HasKey(e => e.Id);

            // Các thuộc tính
            builder.Property(e => e.Title).IsRequired().HasMaxLength(255);
            builder.Property(e => e.Duration).IsRequired();
            builder.Property(e => e.IsActive).HasDefaultValue(true);
        }
    }
}

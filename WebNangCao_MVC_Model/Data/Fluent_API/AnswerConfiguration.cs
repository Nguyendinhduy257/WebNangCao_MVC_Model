using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebNangCao_MVC_Model.Models;

namespace WebNangCao_MVC_Model.Data.Fluent_API
{
    public class AnswerConfiguration : IEntityTypeConfiguration<Answer>
    {
        public void Configure(EntityTypeBuilder<Answer> builder)
        {
            builder.ToTable("Answers");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.Content)
                   .IsRequired()
                   .HasMaxLength(500);

            builder.Property(a => a.IsCorrect)
                   .IsRequired();

            builder.Property(a => a.QuestionId)
                   .IsRequired();

            // Index cho FK (tăng tốc query)
            builder.HasIndex(a => a.QuestionId);

            // Quan hệ 1-N: Question -> Answers
            builder.HasOne(a => a.Question)
                   .WithMany(q => q.Answers)
                   .HasForeignKey(a => a.QuestionId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
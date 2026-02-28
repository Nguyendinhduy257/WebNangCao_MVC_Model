using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebNangCao_MVC_Model.Migrations
{
    /// <inheritdoc />
    public partial class DuLieuVaoBangExam : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdGroup",
                table: "Exams",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdGroup",
                table: "Exams");
        }
    }
}

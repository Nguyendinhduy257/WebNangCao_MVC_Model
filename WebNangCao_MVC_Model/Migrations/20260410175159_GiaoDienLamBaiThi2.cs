using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebNangCao_MVC_Model.Migrations
{
    /// <inheritdoc />
    public partial class GiaoDienLamBaiThi2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSelfCreated",
                table: "Exams",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSelfCreated",
                table: "Exams");
        }
    }
}

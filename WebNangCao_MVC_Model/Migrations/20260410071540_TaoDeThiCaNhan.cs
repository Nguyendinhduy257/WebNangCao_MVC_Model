using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebNangCao_MVC_Model.Migrations
{
    /// <inheritdoc />
    public partial class TaoDeThiCaNhan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exams_Groups_GroupId",
                table: "Exams");

            migrationBuilder.AlterColumn<int>(
                name: "GroupId",
                table: "Exams",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<int>(
                name: "StudentId",
                table: "Exams",
                type: "integer",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Exams_Groups_GroupId",
                table: "Exams",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exams_Groups_GroupId",
                table: "Exams");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "Exams");

            migrationBuilder.AlterColumn<int>(
                name: "GroupId",
                table: "Exams",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Exams_Groups_GroupId",
                table: "Exams",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RGL_LMS.Migrations
{
    /// <inheritdoc />
    public partial class viewcourse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EmployeeId",
                table: "ViewCourses",
                type: "varchar(50)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "ViewCourses",
                type: "varchar(100)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "ViewCourses",
                type: "varchar(20)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "ViewCourses");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "ViewCourses");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "ViewCourses");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RGL_LMS.Migrations
{
    /// <inheritdoc />
    public partial class viewcount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ViewCount",
                table: "ViewCourses",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ViewCount",
                table: "ViewCourses");
        }
    }
}

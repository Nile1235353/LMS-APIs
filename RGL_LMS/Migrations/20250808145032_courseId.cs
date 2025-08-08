using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RGL_LMS.Migrations
{
    /// <inheritdoc />
    public partial class courseId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ViewCourses_Courses_CourseId1",
                table: "ViewCourses");

            migrationBuilder.DropIndex(
                name: "IX_ViewCourses_CourseId1",
                table: "ViewCourses");

            migrationBuilder.DropColumn(
                name: "CourseId1",
                table: "ViewCourses");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CourseId1",
                table: "ViewCourses",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ViewCourses_CourseId1",
                table: "ViewCourses",
                column: "CourseId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ViewCourses_Courses_CourseId1",
                table: "ViewCourses",
                column: "CourseId1",
                principalTable: "Courses",
                principalColumn: "Id");
        }
    }
}

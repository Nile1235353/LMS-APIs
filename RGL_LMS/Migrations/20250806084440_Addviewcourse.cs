using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RGL_LMS.Migrations
{
    /// <inheritdoc />
    public partial class Addviewcourse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ViewCourses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ViewCourseId = table.Column<string>(type: "varchar(50)", nullable: true),
                    CourseId = table.Column<string>(type: "varchar(50)", nullable: true),
                    CourseId1 = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<string>(type: "varchar(50)", nullable: true),
                    Email = table.Column<string>(type: "varchar(150)", nullable: true),
                    Department = table.Column<string>(type: "varchar(100)", nullable: true),
                    Title = table.Column<string>(type: "varchar(200)", nullable: true),
                    Description = table.Column<string>(type: "varchar(1000)", nullable: true),
                    VideoLink = table.Column<string>(type: "varchar(500)", nullable: true),
                    Status = table.Column<string>(type: "varchar(50)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedUser = table.Column<string>(type: "varchar(50)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ViewCourses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ViewCourses_Courses_CourseId1",
                        column: x => x.CourseId1,
                        principalTable: "Courses",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ViewCourses_CourseId1",
                table: "ViewCourses",
                column: "CourseId1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ViewCourses");
        }
    }
}

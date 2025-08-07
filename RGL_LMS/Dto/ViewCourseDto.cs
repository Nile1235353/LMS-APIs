
namespace RGL_LMS.DTO
{
    public class ViewCourseDto
    {
        public int Id { get; set; }
        public string? ViewId { get; set; }
        public int? ViewCount { get; set; } = 0;
        public string? CourseId { get; set; }
        public string? EmployeeId { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Name { get; set; }
        public string? UserId { get; set; }
        public string? Email { get; set; }
        public string? Department { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? VideoLink { get; set; }
        public string? Status { get; set; }
        public string? CreatedUser { get; set; }
    }
}

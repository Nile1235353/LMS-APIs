
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RGL_LMS.Models
{
    public class ViewCourse
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string? ViewCourseId { get; set; } // Unique ViewCourse ID like VC-0001

        [Column(TypeName = "varchar(50)")]
        public string? CourseId { get; set; } // FK to Courses.CourseId

        public Courses? Course { get; set; }

        [Column(TypeName = "int")]
        public int? ViewCount { get; set; } = 0;

        [Column(TypeName = "varchar(50)")]
        public string? UserId { get; set; }

        [Column(TypeName = "varchar(150)")]
        public string? Email { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string? Department { get; set; }

        [Column(TypeName = "varchar(200)")]
        public string? Title { get; set; }

        [Column(TypeName = "varchar(1000)")]
        public string? Description { get; set; }

        [Column(TypeName = "varchar(500)")]
        public string? VideoLink { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string? Status { get; set; } // finished or unfinished

        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string? CreatedUser { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string? EmployeeId { get; set; }

        [Column(TypeName = "varchar(20)")]
        public string? PhoneNumber { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string? Name { get; set; }




    }
}

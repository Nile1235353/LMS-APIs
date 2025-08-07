using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RGL_LMS.Models
{
    public class Courses
    {
        [Key]

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string? CourseId { get; set; }  

        [Column(TypeName = "varchar(200)")]
        public string Title { get; set; } = null!;

        [Column(TypeName = "varchar(1000)")]
        public string? Description { get; set; }
        [Column(TypeName = "varchar(500)")]
        public string VideoLink { get; set; } = null!;

        [Column(TypeName = "varchar(100)")]
        public string? Department { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string? Role { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string? UserId { get; set; }

        [Column(TypeName = "varchar(150)")]
        public string? Name { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string? CreatedUser { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? UpdatedDate { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string? UpdatedUser { get; set; }

        [Column(TypeName = "bit")]
        public bool? IsActive { get; set; } = true;
    }
}

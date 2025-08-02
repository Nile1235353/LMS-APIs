
using RGL_LMS.Models;
using System.ComponentModel.DataAnnotations;

namespace RGL_LMS.Dto
{
    public class CourseDto
    {
        public int? Id { get; set; }
        public string? CourseId { get; set; } 
  
        public string? Title { get; set; } 
        public string? Description { get; set; }
      

        [Required]
        [Url]
        public string VideoLink { get; set; } = null!; 
        public string? Department { get; set; }

        public UserRole Role { get; set; }
        public string? UserId { get; set; }
        public string? Name { get; set; }
        public string? CreatedUser { get; set; }
        public string? UpdatedUser { get; set; }
        public bool? IsActive { get; set; } = true;
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}

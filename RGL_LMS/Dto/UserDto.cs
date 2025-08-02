using RGL_LMS.Models;

namespace RGL_LMS.Dto
{
    public class UserDto
    {
        //public string UserId { get; set; } = null!;
        public string? UserId { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }

        //public string? Role { get; set; }
        public UserRole Role { get; set; }
        public int? SrNo { get; set; }
        public string? NRC { get; set; }
        public string? PhoneNumber { get; set; }
        public string? EmployeeId { get; set; }
        public string? Department { get; set; }
        public string? Position { get; set; }
        public string? Remark { get; set; } 
        public string? CreatedUser { get; set; }
        public string? UpdatedUser { get; set; }
        public Boolean? IsActive { get; set; }
       
    }
}

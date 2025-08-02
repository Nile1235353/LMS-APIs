using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RGL_LMS.Models
{
    //public enum UserRole
    //{
    //    Admin,
    //    Instructor,
    //    Learner
    //}
    public enum UserRole
    {
        Admin = 0,
        Instructor = 1,
        Learner = 2
    }

    public class Users 

    {
        

       [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(TypeName = "varchar(50)")]
        public string? UserId { get; set; }
       
        [Column(TypeName = "varchar(200)")]
        public string? FullName { get; set; }
        [Column(TypeName = "varchar(200)")]
        public string? Email { get; set; }

        [Column(TypeName = "varchar(200)")]
        public string? PasswordHash { get; set; }

        [Column(TypeName = "varchar(150)")]
        public string? Role { get; set; }

        [Column(TypeName = "varchar(200)")]
        public string? NRC { get; set; }
        [Column(TypeName = "varchar(200)")]
        public string? PhoneNumber { get; set; }

        [Column(TypeName = "varchar(200)")]
        public string? EmployeeId { get; set; }

        [Column(TypeName = "varchar(200)")]
        public string? Department { get; set; }

        [Column(TypeName = "varchar(30)")]
        public string? Position { get; set; }

        [Column(TypeName = "int")]
        public int? SrNo { get; set; }

        [Column(TypeName = "varchar(150)")]
        public string? Remark { get; set; }

        [Column(TypeName = "bit")]
        public Boolean? IsActive { get; set; }
      
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string? CreatedUser { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? UpdatedDate { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string? UpdatedUser { get; set; }
    }
}

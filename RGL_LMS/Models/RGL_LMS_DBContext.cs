using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace RGL_LMS.Models
{
    public class RGL_LMS_DBContext : IdentityDbContext<IdentityUser>
    {
        public RGL_LMS_DBContext(DbContextOptions<RGL_LMS_DBContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // modelBuilder.ApplyConfiguration(new RoleConfiguration());
        }

      
        public DbSet<Users> User { get; set; }
        public DbSet<Courses> Courses { get; set; }
       

    }
}

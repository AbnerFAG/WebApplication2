using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Data
{
    public class DemoContext: DbContext
    {
        public DemoContext(DbContextOptions options) 
            : base(options)
        {
        }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<WebApplication1.Models.Student> Student { get; set; } = default!;
        public DbSet<WebApplication1.Models.Enrollment> Enrollment { get; set; } = default!;
    }
}

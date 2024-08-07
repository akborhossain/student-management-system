using Microsoft.EntityFrameworkCore;
using Student_Management_System.Models.Entities;
namespace Student_Management_System.Data
{
    public class ApplicationDBContext: DbContext
    {
        public ApplicationDBContext( DbContextOptions<ApplicationDBContext> options): base(options)
        {
            
        }
        public DbSet<Student> Students { get; set; }
    }
}

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


        public DbSet<User> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // User-Student (One-to-One)
            modelBuilder.Entity<User>()
                .HasOne(u => u.Student)
                .WithOne(s => s.User)
                .HasForeignKey<Student>(s => s.UserId);

            // Student-Registration (One-to-Many)
            modelBuilder.Entity<Student>()
                .HasMany(s => s.Registrations)
                .WithOne(r => r.Student)
                .HasForeignKey(r => r.StudentId);

            // Registration-Course (Many-to-One)
            modelBuilder.Entity<Registration>()
                .HasOne(r => r.Course)
                .WithMany(c => c.Registrations)
                .HasForeignKey(r => r.CourseId);
        }

        public DbSet<Student_Management_System.Models.Entities.Course> Course { get; set; } = default!;
        public DbSet<Student_Management_System.Models.Entities.Registration> Registration { get; set; } = default!;

    }
}

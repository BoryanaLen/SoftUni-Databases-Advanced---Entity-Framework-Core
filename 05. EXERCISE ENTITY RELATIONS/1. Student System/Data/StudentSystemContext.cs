using Microsoft.EntityFrameworkCore;
using P01_StudentSystem.Data.Models;


namespace P01_StudentSystem.Data
{
    public class StudentSystemContext : DbContext
    {
        public StudentSystemContext()
        {

        }

        public StudentSystemContext( DbContextOptions options) 
            : base(options)
        {

        }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Homework> HomeworkSubmissions { get; set; }
        public DbSet<Resource> Resources { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<StudentCourse> StudentCourses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureStudentEntity(modelBuilder);
            ConfigureCourseEntity(modelBuilder);
            ConfigureResourceeEntity(modelBuilder);
            ConfigureHomeworkEntity(modelBuilder);
            ConfigureStudentCourseEntity(modelBuilder);
        }

        private void ConfigureStudentCourseEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudentCourse>()
                .HasKey(sc => new { sc.StudentId, sc.CourseId });
        }

        private void ConfigureHomeworkEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Homework>()
                .HasKey(h => h.HomeworkId);

            modelBuilder.Entity<Homework>()
                .Property(h => h.Content)
                .IsUnicode(false);
        }

        private void ConfigureResourceeEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Resource>()
                .HasKey(r => r.ResourceId);

            modelBuilder.Entity<Resource>()
                .Property(r => r.Name)
                .HasMaxLength(50)
                .IsUnicode()
                .IsRequired();

            modelBuilder.Entity<Resource>()
               .Property(r => r.Url)
               .IsUnicode(false);
        }

        private void ConfigureCourseEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>()
                .HasKey(c => c.CourseId);

            modelBuilder.Entity<Course>()
                .HasMany(c => c.StudentsEnrolled)
                .WithOne(se => se.Course)
                .HasForeignKey(c => c.CourseId);

            modelBuilder.Entity<Course>()
               .HasMany(c => c.HomeworkSubmissions)
               .WithOne(hs => hs.Course)
               .HasForeignKey(c => c.CourseId);

            modelBuilder.Entity<Course>()
               .HasMany(c => c.Resources)
               .WithOne(r => r.Course)
               .HasForeignKey(c => c.CourseId);

            modelBuilder.Entity<Course>()
                .Property(c => c.Name)
                .HasMaxLength(80)
                .IsUnicode()
                .IsRequired();

            modelBuilder.Entity<Course>()
                .Property(c => c.Description)
                .IsUnicode();
        }

        private void ConfigureStudentEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>()
                .HasKey(s => s.StudentId);

            modelBuilder.Entity<Student>()
                .HasMany(s => s.CourseEnrollments)
                .WithOne(ce => ce.Student)
                .HasForeignKey(s => s.StudentId);

            modelBuilder.Entity<Student>()
               .HasMany(s => s.HomeworkSubmissions)
               .WithOne(hs => hs.Student)
               .HasForeignKey(s => s.StudentId);

            modelBuilder.Entity<Student>()
                .Property(s => s.Name)
                .HasMaxLength(100)
                .IsUnicode()
                .IsRequired();

            modelBuilder.Entity<Student>()
               .Property(s => s.PhoneNumber)
               .IsUnicode(false)
               .IsRequired(false)
               .HasColumnType("char(10)");

            modelBuilder.Entity<Student>()
              .Property(s => s.Birthday)
              .IsRequired(false);
        }
    }
}

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace StudentClass.Models
{
    public class DatabaseDbContext:IdentityDbContext<AppUser>
    {
        public DatabaseDbContext(DbContextOptions<DatabaseDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudentInClass>()
                .HasKey(x => new { x.StudentId, x.ClassId });
            modelBuilder.Entity<StudentInClass>()
                .HasOne(si => si.Student)
                .WithMany(si => si.StudentInClasses)
                .HasForeignKey(si => si.StudentId);
            modelBuilder.Entity<StudentInClass>()
                .HasOne(si => si.Class)
                .WithMany(si => si.StudentInClasses)
                .HasForeignKey(si => si.ClassId);
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Student> students { get; set; }
        public DbSet<Class> classes { get; set; }
        public DbSet<StudentInClass> studentInClass { get; set; }


    }
}